
using System;
using System.Collections.Generic;
using UnityEngine;

public class PackConstraint : AnnotatablePack
{
    public PackDescriptor Descriptor;
    public SpriteEntry SpriteEntry;
    public ConfigPack Pack;
    public int SlotIndex;

    public bool IsEmpty => Pack == null;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Pack",                       thisObject => ((PackConstraint)thisObject).Pack },
    };
    public object Get(string s) => Accessor[s](this);
    public PackConstraint(PackDescriptor descriptor, SpriteEntry spriteEntry, int slotIndex)
    {
        Descriptor = descriptor;
        SpriteEntry = spriteEntry;
        Pack = null;
        SlotIndex = slotIndex;
    }

    public Sprite GetConstraintSprite()
        => SpriteEntry.Sprite;

    public bool IsUnlocked()
        => AppManager.Instance.ConfigManager.ConstraintIsUnlocked(this);

    public Description GetUnlockCondition()
        => AppManager.Instance.ConfigManager.GetConstraintUnlockCondition(this);

    public bool CanShowAnnotation()
        => Pack != null;

    public string GetName()
        => Pack.GetName();

    public WuXing? GetWuXing()
        => Pack.GetWuXing();

    public Description GetDescription()
        => Pack.GetDescription();

    public string GetTrivia()
        => Pack.GetTrivia();

    public Sprite GetSprite()
        => Pack.GetSprite();
}
