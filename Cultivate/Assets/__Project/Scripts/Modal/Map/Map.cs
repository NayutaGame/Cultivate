
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class Map : Addressable
{
    public static readonly int StepItemCapacity = 15;

    private StepItemListModel _stepItems;

    private RunNode this[int x, int y] => _stepItems[x]._nodes[y];
    private RunNode this[Vector2Int pos] => this[pos.x, pos.y];

    public EntityPool EntityPool;
    public CyclicPool<NodeEntry> _b;
    public CyclicPool<NodeEntry> _r;
    public CyclicPool<NodeEntry> _a;
    public Dictionary<JingJie, NodeEntry[]> _priorityNodes;
    public Dictionary<JingJie, CyclicPool<NodeEntry>[]> _normalPools;

    public bool Selecting { get; private set; }
    private Vector2Int _heroPosition;

    public RunNode CurrentNode
    {
        get
        {
            if (_heroPosition.x < 0)
                return null;
            return this[_heroPosition];
        }
    }

    public PanelDescriptor SelectedNode(RunNode selected)
    {
        if (_heroPosition.x >= 0)
            this[_heroPosition].State = RunNode.RunNodeState.Passed;

        _heroPosition = new(_heroPosition.x + 1, selected.Position.y);

        _stepItems[_heroPosition.x].SelectedNode(selected.Position.y);

        Selecting = false;
        return CurrentNode.CurrentPanel;
    }

    public PanelDescriptor ReceiveSignal(Signal signal)
    {
        PanelDescriptor panelDescriptor = CurrentNode.CurrentPanel.ReceiveSignal(signal);
        if (panelDescriptor != null)
        {
            CurrentNode.ChangePanel(panelDescriptor);
        }
        else
        {
            TryFinishNode();
        }

        bool commit = RunManager.Instance.Environment?.TryCommit() ?? false;
        if (!commit)
            return panelDescriptor;

        panelDescriptor = RunManager.Instance.Environment.GetActivePanel();
        CurrentNode.ChangePanel(panelDescriptor);
        return panelDescriptor;
    }

    private void TryFinishNode()
    {
        if (Selecting)
            return;

        // if it is the last node, try goto next jingjie / commit run

        bool isEndOfJingJie = IsEndOfJingJie(_heroPosition.x);

        if (!isEndOfJingJie)
            _stepItems[_heroPosition.x + 1].SetToChoose();

        Selecting = true;

        CurrentNode.ChangePanel(null);

        if (isEndOfJingJie)
            RunManager.Instance.Environment.SetJingJieProcedure(GetJingJie() + 1);
    }

    private bool IsEndOfJingJie(int currentX)
    {
        return currentX >= _normalPools[_jingJie].Length - 1;
    }

    private JingJie _jingJie;
    public JingJie GetJingJie() => _jingJie;
    public void SetJingJie(JingJie jingJie)
    {
        _jingJie = jingJie;
        RefreshNodes();
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

    private void RefreshNodes()
    {
        NodeEntry[] priorityNodes = _priorityNodes[_jingJie];
        CyclicPool<NodeEntry>[] pools = _normalPools[_jingJie];

        for (int x = 0; x < pools.Length; x++)
        {
            NodeEntry priorityNode = x < priorityNodes.Length ? priorityNodes[x] : null;
            if (priorityNode != null)
            {
                SetStepItemFromPriority(x, priorityNode);
                continue;
            }

            CyclicPool<NodeEntry> pool = pools[x];
            SetStepItemFromPool(x, pool);
        }

        _stepItems[0].SetToChoose();

        Selecting = true;
        _heroPosition = new Vector2Int(-1, 0);
    }

    private void SetStepItemFromPriority(int x, NodeEntry nodeEntry)
    {
        StepItem stepItem = _stepItems[x];

        stepItem._nodes.Clear();

        if (nodeEntry is BattleNodeEntry battleNode)
        {
            DrawEntityDetails d = new DrawEntityDetails(_jingJie, x <= 3, 3 < x && x <= 6, 6 < x);
            stepItem._nodes.Add(new BattleRunNode(this, new Vector2Int(x, 0), _jingJie, battleNode, d));
        }
        else
        {
            stepItem._nodes.Add(new RunNode(this, new Vector2Int(x, 0), _jingJie, nodeEntry));
        }
    }

    private void SetStepItemFromPool(int x, CyclicPool<NodeEntry> pool)
    {
        StepItem stepItem = _stepItems[x];

        stepItem._nodes.Clear();

        if (pool == _r)
        {
            for (int y = 0; y < StepItem.Capacity; y++)
            {
                NodeEntry nodeEntry = pool.ForcePopItem(pred: e => e.CanCreate(this, x));
                stepItem._nodes.Add(new RunNode(this, new Vector2Int(x, y), _jingJie, nodeEntry));
            }
        }
        else
        {
            NodeEntry nodeEntry = pool.ForcePopItem(pred: e => e.CanCreate(this, x));
            if (nodeEntry is BattleNodeEntry battleNodeEntry)
            {
                DrawEntityDetails d = new DrawEntityDetails(_jingJie, x <= 3, 3 < x && x <= 6, 6 < x);
                stepItem._nodes.Add(new BattleRunNode(this, new Vector2Int(x, 0), _jingJie, battleNodeEntry, d));
            }
            else
            {
                stepItem._nodes.Add(new RunNode(this, new Vector2Int(x, 0), _jingJie, nodeEntry));
            }
        }
    }

    public AdventureNodeEntry NextAdventure()
    {
        int currX = _heroPosition.x;
        for (int x = currX + 1; x < _normalPools[_jingJie].Length; x++)
        {
            StepItem stepItem = _stepItems[x];
            RunNode node = stepItem._nodes.Traversal().FirstObj(node => node.Entry is AdventureNodeEntry);
            if (node != null)
                return node.Entry as AdventureNodeEntry;
        }
        return null;
    }

    public bool HasAdventureAfterwards(int currX)
    {
        for (int x = currX + 1; x < _normalPools[_jingJie].Length; x++)
        {
            if (_normalPools[_jingJie][x] == _a)
                return true;
        }

        return false;
    }

    public bool RedrawNextAdventure()
    {
        int currX = _heroPosition.x;
        for (int x = currX + 1; x < _normalPools[_jingJie].Length; x++)
        {
            StepItem stepItem = _stepItems[x];
            RunNode node = stepItem._nodes[0];
            if (node is { Entry: AdventureNodeEntry adventureNodeEntry })
            {
                NodeEntry newNodeEntry = _a.ForcePopItem(pred: n => n != adventureNodeEntry);
                stepItem._nodes[0] = new RunNode(this, new Vector2Int(x, 0), _jingJie, newNodeEntry);
                return true;
            }
        }
        return false;
    }
}
