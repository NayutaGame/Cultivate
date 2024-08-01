
using System;
using System.Collections.Generic;
using CLLibrary;

public class Map : Addressable
{
    // Map ->> Step ->> Node ->> Panel
    
    private JingJie _jingJie;
    public JingJie JingJie => _jingJie;
    public void SetJingJie(JingJie jingJie) => _jingJie = jingJie;
    
    private StepItem _stepItem;
    
    public StepItem CurrStepItem
        => _stepItem;
    
    public int Step;
    public bool IsLastStep()
        => Step >= StepDescriptors.Length - 1;
    
    public int Choice;
    public RunNode CurrNode
        => Choosing ? null : _stepItem._nodes[Choice];
    
    public StepDescriptor[] StepDescriptors;
    public StepDescriptor CurrStepDescriptor => StepDescriptors[Step];
    
    public EntityPool EntityPool;
    public Pool<NodeEntry> AdventurePool;
    public Pool<NodeEntry> InsertedAdventurePool;
    public bool Choosing;
    
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Map()
    {
        _accessors = new()
        {
            { "StepItem", () => _stepItem },
        };

        _stepItem = new();
    }

    public void DrawNode()
        => CurrStepDescriptor.Draw(this);

    public void CreateEntry()
        => CurrNode.Entry.Create(this, CurrNode.Ladder);

    public void InsertAdventure(NodeEntry nodeEntry)
    {
        InsertedAdventurePool.Populate(nodeEntry);
        InsertedAdventurePool.Shuffle();
    }

    public void InitEntityPool()
    {
        EntityPool = new();
        EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal().FilterObj(e => e.IsInPool()));
        EntityPool.Shuffle();
    }

    public void InitAdventurePool()
    {
        AdventurePool = new();
        AdventurePool.Populate(Encyclopedia.NodeCategory.Traversal.FilterObj(e => e.WithInPool));
        AdventurePool.Shuffle();
    }
}
