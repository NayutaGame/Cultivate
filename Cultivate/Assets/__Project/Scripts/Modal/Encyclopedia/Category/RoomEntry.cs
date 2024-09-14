
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
    private Action<Map, Room> _create;

    public RoomEntry(
        string id,
        string description,
        CLLibrary.Bound ladderBound,
        bool withInPool,
        Action<Map, Room> create,
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
    public void Create(Map map, Room room) => _create(map, room);

    public static implicit operator RoomEntry(string id) => Encyclopedia.RoomCategory[id];
}
