
using System;
using System.Collections.Generic;
using UnityEngine;

public class ConfigPack : AnnotatablePack
{
    public PackEntry Entry;
    public bool IsEquipped;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "Pack",                       thisObject => ((PackConstraint)thisObject).Pack },
    };
    public object Get(string s) => Accessor[s](this);
    public ConfigPack(PackEntry entry)
    {
        Entry = entry;
        IsEquipped = false;
    }

    public string GetName()
        => Entry.GetName();

    public WuXing GetWuXing()
        => Entry.WuXing;

    public Description GetDescription()
        => Entry.GetDescription();

    public string GetTrivia()
        => Entry.Trivia;

    public Sprite GetSprite()
        => Entry.GetSprite();

    public bool Equipped()
        => IsEquipped;

    public bool IsUnlocked()
        => AppManager.Instance.ConfigManager.PackIsGenerallyUnlocked(Entry);

    public Description GetUnlockCondition()
        => AppManager.Instance.ConfigManager.GetPackUnlockCondition(Entry);

    public bool CanShowAnnotation()
        => true;
}
