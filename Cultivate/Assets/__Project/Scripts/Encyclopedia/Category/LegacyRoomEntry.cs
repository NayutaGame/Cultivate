
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class LegacyRoomEntry : Entry
{
    [NonSerialized] private string _description;
    [NonSerialized] private Bound _ladderBound;
    [NonSerialized] private Bound _difficultyBound;
    [NonSerialized] private bool _withInPool;
    [NonSerialized] private Func<Map, LegacyRoom, bool> _canCreate;
    [NonSerialized] private Func<Map, LegacyRoom, Cell> _create;
    [NonSerialized] private SpriteEntry _spriteEntry;

    public LegacyRoomEntry(
        string id,
        string name,
        string description,
        Bound ladderBound,
        Bound difficultyBound,
        bool withInPool,
        Func<Map, LegacyRoom, Cell> create,
        Func<Map, LegacyRoom, bool> canCreate = null
        ) : base(id, name)
    {
        _description = description;
        _ladderBound = ladderBound;
        _difficultyBound = difficultyBound;
        _withInPool = withInPool;
        _create = create;
        _canCreate = canCreate ?? ((map, room) => _ladderBound.Contains(room.Ladder));
        
        _spriteEntry = Encyclopedia.SpriteCategory.FromName($"Event{description}") ?? Encyclopedia.SpriteCategory.MissingEventIllustration();
    }
    
    public string GetDescription() => _description;
    public Bound LadderBound => _ladderBound;
    public Bound DifficultyBound => _difficultyBound;
    public bool WithInPool => _withInPool;

    public bool CanCreate(Map map, LegacyRoom room) => _canCreate(map, room);
    public Cell Create(Map map, LegacyRoom room) => _create(map, room);
    
    public Sprite GetSprite() => _spriteEntry.Sprite;
}
