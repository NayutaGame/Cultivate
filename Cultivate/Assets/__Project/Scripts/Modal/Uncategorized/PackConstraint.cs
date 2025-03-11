
using System;
using System.Collections.Generic;
using UnityEngine;

public class PackConstraint : Addressable
{
    public PackDescriptor Descriptor;
    public SpriteEntry SpriteEntry;
    public ConfigPack Pack;
    public int SlotIndex;

    public bool IsEmpty => Pack == null;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public PackConstraint(PackDescriptor descriptor, SpriteEntry spriteEntry, int slotIndex)
    {
        _accessors = new()
        {
            { "Pack", () => Pack },
        };
        
        Descriptor = descriptor;
        SpriteEntry = spriteEntry;
        Pack = null;
        SlotIndex = slotIndex;
    }

    public Sprite GetConstraintSprite()
        => SpriteEntry.Sprite;

    public bool IsUnlocked()
        => AppManager.Instance.ConfigManager.ConstraintIsUnlocked(this);

    public string GetUnlockCondition()
        => AppManager.Instance.ConfigManager.GetConstraintUnlockCondition(this);
}
