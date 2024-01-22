
using System;

public abstract class NodeEntry : Entry
{
    private string _description;
    public string Description => _description;
    private bool _withInPool;
    public bool WithInPool => _withInPool;

    private Func<Map, int, bool> _canCreate;
    private Action<RunNode> _create;

    public NodeEntry(string name, string description, bool withInPool, Action<RunNode> create, Func<Map, int, bool> canCreate = null) : base(name)
    {
        _description = description;
        _withInPool = withInPool;
        _create = create;
        _canCreate = canCreate ?? ((map, x) => true);
    }

    public bool CanCreate(Map map, int x) => _canCreate(map, x);
    public void Create(RunNode runNode) => _create(runNode);

    public static implicit operator NodeEntry(string name) => Encyclopedia.NodeCategory[name];

    public virtual string GetTitle()
        => GetName();
}
