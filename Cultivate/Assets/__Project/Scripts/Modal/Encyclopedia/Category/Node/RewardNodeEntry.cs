
using System;

public class RewardNodeEntry : NodeEntry
{
    private SpriteEntry _spriteEntry;
    public SpriteEntry SpriteEntry => _spriteEntry;

    public RewardNodeEntry(string id, string description, SpriteEntry spriteEntry, bool withInPool, Action<Map, RunNode, int, int> create, Func<Map, int, int, bool> canCreate = null)
        : base(id, description, withInPool, create, canCreate)
    {
        _spriteEntry = spriteEntry;
    }
}
