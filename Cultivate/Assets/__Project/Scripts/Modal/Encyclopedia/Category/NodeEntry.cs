
using System;
using UnityEngine;

public class NodeEntry : Entry
{
    public virtual string GetName() => GetId();

    public Sprite GetSprite() => Encyclopedia.SpriteCategory["摇曳"].Sprite;
    
    private string _description;
    public string GetDescription() => _description;

    private CLLibrary.Bound _ladderBound;
    public CLLibrary.Bound LadderBound => _ladderBound;
    
    private bool _withInPool;
    public bool WithInPool => _withInPool;

    private Func<Map, int, bool> _canCreate;
    private Action<Map, int> _create;

    public NodeEntry(string id, string description, CLLibrary.Bound ladderBound, bool withInPool, Action<Map, int> create, Func<Map, int, bool> canCreate = null)
        : base(id)
    {
        _description = description;
        _ladderBound = ladderBound;
        _withInPool = withInPool;
        _create = create;
        _canCreate = canCreate ?? ((map, ladder) => _ladderBound.Contains(ladder));
    }

    public bool CanCreate(Map map, int ladder) => _canCreate(map, ladder);
    public void Create(Map map, int ladder) => _create(map, ladder);

    public static implicit operator NodeEntry(string id) => Encyclopedia.NodeCategory[id];
}
