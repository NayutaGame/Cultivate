using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardNodeEntry : NodeEntry
{
    private SpriteEntry _spriteEntry;
    public SpriteEntry SpriteEntry => _spriteEntry;

    public RewardNodeEntry(string name, string description, SpriteEntry spriteEntry, Action<RunNode> create, Predicate<int> canCreate = null) : base(name, description, create, canCreate)
    {
        _spriteEntry = spriteEntry;
    }
}
