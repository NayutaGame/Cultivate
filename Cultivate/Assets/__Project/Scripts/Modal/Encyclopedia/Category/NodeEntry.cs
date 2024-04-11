
using System;
using UnityEngine;

public class NodeEntry : Entry
{
    public virtual string GetName() => GetId();

    public Sprite GetSprite() => Encyclopedia.SpriteCategory["摇曳"].Sprite;
    
    private string _description;
    public string GetDescription() => _description;
    private bool _withInPool;
    public bool WithInPool => _withInPool;

    private Func<Map, bool> _canCreate;
    private Action<Map> _create;

    public NodeEntry(string id, string description, bool withInPool, Action<Map> create, Func<Map, bool> canCreate = null)
        : base(id)
    {
        _description = description;
        _withInPool = withInPool;
        _create = create;
        _canCreate = canCreate ?? (map => true);
    }

    public bool CanCreate(Map map) => _canCreate(map);
    public void Create(Map map) => _create(map);

    public static implicit operator NodeEntry(string id) => Encyclopedia.NodeCategory[id];
}
