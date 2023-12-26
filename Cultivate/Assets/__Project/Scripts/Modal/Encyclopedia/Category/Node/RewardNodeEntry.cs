using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardNodeEntry : NodeEntry
{
    private SpriteEntry _spriteEntry;
    public SpriteEntry SpriteEntry => _spriteEntry;

    public RewardNodeEntry(string name, string description, SpriteEntry spriteEntry, bool withInPool, Action<RunNode> create, Func<Map, int, bool> canCreate = null) : base(name, description, withInPool, create, canCreate)
    {
        _spriteEntry = spriteEntry;
    }
}
