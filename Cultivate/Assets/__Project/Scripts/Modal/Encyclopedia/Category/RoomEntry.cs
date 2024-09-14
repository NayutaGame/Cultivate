
using System;
using UnityEngine;

public class RoomEntry : Entry
{
    public virtual string GetName() => GetId();

    public Sprite GetSprite() => Encyclopedia.SpriteCategory["摇曳"].Sprite;
    
    private string _description;
    public string GetDescription() => _description;

    private CLLibrary.Bound _ladderBound;
    public CLLibrary.Bound LadderBound => _ladderBound;
    
    private bool _withInPool;
    public bool WithInPool => _withInPool;

    private Func<Map, Room, bool> _canCreate;
    private Func<Map, Room, PanelDescriptor> _create;

    public RoomEntry(
        string id,
        string description,
        CLLibrary.Bound ladderBound,
        bool withInPool,
        Func<Map, Room, PanelDescriptor> create,
        Func<Map, Room, bool> canCreate = null
        ) : base(id)
    {
        _description = description;
        _ladderBound = ladderBound;
        _withInPool = withInPool;
        _create = create;
        _canCreate = canCreate ?? ((map, room) => _ladderBound.Contains(room.Ladder));
    }

    public bool CanCreate(Map map, Room room) => _canCreate(map, room);
    public PanelDescriptor Create(Map map, Room room) => _create(map, room);

    public static implicit operator RoomEntry(string id) => Encyclopedia.RoomCategory[id];
}
