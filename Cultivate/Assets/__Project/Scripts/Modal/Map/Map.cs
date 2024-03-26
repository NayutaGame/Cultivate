
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Serialization;

public class Map : Addressable
{
    private static readonly int StepItemCapacity = 15;

    private StepItemListModel _stepItems;

    public StepItem CurrStepItem
        => _stepItems[Step];
    
    public RunNode CurrNode
        => _stepItems[Step]._nodes[Choice];

    public int Level;
    public bool IsLastLevel()
        => Level >= DrawDescriptors.Length - 1;
    
    public int Step;
    public bool IsLastStep()
        => Step >= DrawDescriptors[Level].Length - 1;
    
    public int Choice;
    
    public DrawDescriptor[][] DrawDescriptors;
    public EntityPool EntityPool;
    public Pool<NodeEntry> AdventurePool;
    [FormerlySerializedAs("Selecting")] public bool Choosing;
    
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

    public void CreateEntry()
        => CurrNode.Entry.Create(this, CurrNode, Level, Step);

    public void DrawStepItems()
    {
        for (int i = 0; i < DrawDescriptors[Level].Length; i++)
            DrawDescriptors[Level][i].Draw(this, _stepItems[i], Level, i);
    }

    public AdventureNodeEntry NextAdventure()
    {
        for (int step = Step + 1; step < DrawDescriptors[Level].Length; step++)
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
        for (int step = currStep + 1; step < DrawDescriptors[Level].Length; step++)
        {
            if (DrawDescriptors[Level][step].GetNodeType() == DrawDescriptor.NodeType.Adventure)
                return true;
        }

        return false;
    }

    public bool RedrawNextAdventure()
    {
        for (int step = Step + 1; step < DrawDescriptors[Level].Length; step++)
        {
            StepItem stepItem = _stepItems[step];
            RunNode node = stepItem._nodes[0];
            if (node is { Entry: AdventureNodeEntry adventureNodeEntry })
            {
                AdventurePool.TryPopItem(out NodeEntry newNodeEntry, pred: n => n != adventureNodeEntry);
                stepItem._nodes[0] = new RunNode(newNodeEntry);
                return true;
            }
        }
        return false;
    }
}
