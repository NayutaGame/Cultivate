
using System;
using UnityEngine;

public abstract class NodeEntry : Entry
{
    // public enum NodeType
    // {
    //     Rest,
    //     Adventure,
    //     Encounter,
    //     Ascension,
    //     Success,
    //     Normal,
    //     Elite,
    //     Boss,
    // }
    
    public virtual string GetName() => GetId();

    public Sprite GetSprite() => Encyclopedia.SpriteCategory["摇曳"].Sprite;
    
    private string _description;
    public string GetDescription() => _description;
    private bool _withInPool;
    public bool WithInPool => _withInPool;

    private Func<Map, JingJie, int, bool> _canCreate;
    private Action<Map, RunNode, JingJie, int> _create;

    public NodeEntry(string id, string description, bool withInPool, Action<Map, RunNode, JingJie, int> create, Func<Map, JingJie, int, bool> canCreate = null)
        : base(id)
    {
        _description = description;
        _withInPool = withInPool;
        _create = create;
        _canCreate = canCreate ?? ((map, JingJie, step) => true);
    }

    public bool CanCreate(Map map, JingJie jingJie, int step) => _canCreate(map, jingJie, step);
    public void Create(Map map, RunNode runNode, JingJie jingJie, int step) => _create(map, runNode, jingJie, step);

    public static implicit operator NodeEntry(string id) => Encyclopedia.NodeCategory[id];
}
