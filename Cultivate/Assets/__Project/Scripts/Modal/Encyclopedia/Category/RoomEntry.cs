
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class RoomEntry : Entry
{
    public virtual string GetName() => GetId();
    
    [NonSerialized] private string _description;
    public string GetDescription() => _description;

    [NonSerialized] private Bound _ladderBound;
    public Bound LadderBound => _ladderBound;

    [NonSerialized] private Bound _difficultyBound;
    public Bound DifficultyBound => _difficultyBound;
    
    [NonSerialized] private bool _withInPool;
    public bool WithInPool => _withInPool;

    [NonSerialized] private Func<Map, Room, bool> _canCreate;
    [NonSerialized] private Func<Map, Room, Cell> _create;

    [NonSerialized] private SpriteEntry _spriteEntry;

    public RoomEntry(
        string id,
        string description,
        Bound ladderBound,
        Bound difficultyBound,
        bool withInPool,
        Func<Map, Room, Cell> create,
        Func<Map, Room, bool> canCreate = null
        ) : base(id)
    {
        _description = description;
        _ladderBound = ladderBound;
        _difficultyBound = difficultyBound;
        _withInPool = withInPool;
        _create = create;
        _canCreate = canCreate ?? ((map, room) => _ladderBound.Contains(room.Ladder));

        _spriteEntry = ((SpriteEntry)($"Event{description}")) ?? Encyclopedia.SpriteCategory.MissingEventIllustration();
    }

    public bool CanCreate(Map map, Room room) => _canCreate(map, room);
    public Cell Create(Map map, Room room) => _create(map, room);

    public static implicit operator RoomEntry(string id) => Encyclopedia.RoomCategory[id];
    
    public Sprite GetSprite() => _spriteEntry.Sprite;
}
