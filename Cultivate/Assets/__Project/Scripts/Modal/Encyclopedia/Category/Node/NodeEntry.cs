
using System;

public abstract class NodeEntry : Entry
{
    public string GetName() => GetId();
    public virtual string GetTitle() => GetId();
    
    private string _description;
    public string Description => _description;
    private bool _withInPool;
    public bool WithInPool => _withInPool;

    private Func<Map, int, int, bool> _canCreate;
    private Action<Map, RunNode, int, int> _create;

    public NodeEntry(string id, string description, bool withInPool, Action<Map, RunNode, int, int> create, Func<Map, int, int, bool> canCreate = null)
        : base(id)
    {
        _description = description;
        _withInPool = withInPool;
        _create = create;
        _canCreate = canCreate ?? ((map, level, step) => true);
    }

    public bool CanCreate(Map map, int level, int step) => _canCreate(map, level, step);
    public void Create(Map map, RunNode runNode, int level, int step) => _create(map, runNode, level, step);

    public static implicit operator NodeEntry(string id) => Encyclopedia.NodeCategory[id];
}
