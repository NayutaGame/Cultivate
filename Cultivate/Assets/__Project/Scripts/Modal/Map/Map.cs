
using System;
using System.Collections.Generic;
using CLLibrary;

public class Map : Addressable
{
    public static readonly int StepItemCapacity = 15;

    private StepItemListModel _stepItems;

    private int _level;
    public int GetLevel() => _level;
    public void SetLevel(int value)
    {
        _level = value;
        GenerateStepItems();
    }

    private int _step;
    private StepItem CurrStepItem => _stepItems[_step];

    private int _choice;
    private void SetChoice(int value)
    {
        _choice = value;
        NodeListModel nodes = CurrStepItem._nodes;
        for (int i = 0; i < nodes.Count(); i++)
        {
            RunNode node = nodes[i];
            if (i == _choice)
            {
                node.SetState(RunNode.RunNodeState.Current);
                node.Entry.Create(this, node);
            }
            else
            {
                node.SetState(RunNode.RunNodeState.Missed);
            }
        }
    }
    public RunNode CurrNode => _step < 0 ? null : CurrStepItem._nodes[_choice];
    
    public DrawDescriptor[][] DrawDescriptors;
    public EntityPool EntityPool;
    public Pool<NodeEntry> AdventurePool;

    public bool Selecting { get; private set; }

    public PanelDescriptor SelectedNode(RunNode selected)
    {
        if (_step >= 0)
            CurrNode.SetState(RunNode.RunNodeState.Passed);

        _step++;

        SetChoice(selected.Position.y);

        Selecting = false;
        return CurrNode.CurrentPanel;
    }

    public void SetToChoose(int step)
    {
        NodeListModel nodes = _stepItems[step]._nodes;
        for (int i = 0; i < nodes.Count(); i++)
            nodes[i].SetState(RunNode.RunNodeState.ToChoose);
    }

    public PanelDescriptor ReceiveSignal(Signal signal)
    {
        PanelDescriptor panelDescriptor = CurrNode.CurrentPanel.ReceiveSignal(signal);
        if (panelDescriptor != null)
        {
            CurrNode.ChangePanel(panelDescriptor);
        }
        else
        {
            TryFinishNode();
        }

        bool commit = RunManager.Instance.Environment?.TryCommit() ?? false;
        if (!commit)
            return panelDescriptor;

        panelDescriptor = RunManager.Instance.Environment.GetActivePanel();
        CurrNode.ChangePanel(panelDescriptor);
        return panelDescriptor;
    }

    private void TryFinishNode()
    {
        if (Selecting)
            return;

        // if it is the last node, try goto next jingjie / commit run

        bool isEndOfJingJie = IsEndOfJingJie(_step);

        if (!isEndOfJingJie)
            SetToChoose(_step + 1);

        Selecting = true;

        CurrNode.ChangePanel(null);

        if (isEndOfJingJie)
            RunManager.Instance.Environment.SetJingJieProcedure(GetLevel() + 1);
    }

    private bool IsEndOfJingJie(int currentX)
    {
        return currentX >= DrawDescriptors[_level].Length - 1;
    }

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Map()
    {
        _accessors = new()
        {
            { "StepItems", () => _stepItems },
        };

        _stepItems = new StepItemListModel();
        StepItemCapacity.Do(i => _stepItems.Add(new StepItem()));
    }

    private void GenerateStepItems()
    {
        DrawDescriptor[] drawDescriptors = DrawDescriptors[_level];

        for (int i = 0; i < drawDescriptors.Length; i++)
        {
            drawDescriptors[i].Draw(this, _stepItems[i], _level, i);
        }

        SetToChoose(0);

        Selecting = true;

        _step = -1;
        _choice = 0;
    }

    public AdventureNodeEntry NextAdventure()
    {
        int currStep = _step;
        for (int step = currStep + 1; step < DrawDescriptors[_level].Length; step++)
        {
            StepItem stepItem = _stepItems[step];
            RunNode node = stepItem._nodes.Traversal().FirstObj(node => node.Entry is AdventureNodeEntry);
            if (node != null)
                return node.Entry as AdventureNodeEntry;
        }
        return null;
    }

    public bool HasAdventureAfterwards(int currStep)
    {
        for (int step = currStep + 1; step < DrawDescriptors[_level].Length; step++)
        {
            if (DrawDescriptors[_level][step].GetNodeType() == DrawDescriptor.NodeType.Adventure)
                return true;
        }

        return false;
    }

    public bool RedrawNextAdventure()
    {
        int currStep = _step;
        for (int step = currStep + 1; step < DrawDescriptors[_level].Length; step++)
        {
            StepItem stepItem = _stepItems[step];
            RunNode node = stepItem._nodes[0];
            if (node is { Entry: AdventureNodeEntry adventureNodeEntry })
            {
                AdventurePool.TryPopItem(out NodeEntry newNodeEntry, pred: n => n != adventureNodeEntry);
                stepItem._nodes[0] = new RunNode(_level, step, 0, newNodeEntry);
                return true;
            }
        }
        return false;
    }
}
