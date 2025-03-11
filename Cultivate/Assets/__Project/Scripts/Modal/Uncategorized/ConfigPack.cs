
using UnityEngine;

public class ConfigPack : IPack
{
    public PackEntry Entry;
    public bool IsEquipped;

    public ConfigPack(PackEntry entry)
    {
        Entry = entry;
        IsEquipped = false;
    }

    public string GetName()
        => Entry.Name;

    public WuXing? GetWuXing()
        => Entry.WuXing;

    public string GetDescription()
        => Entry.Description;

    public string GetTrivia()
        => Entry.Trivia;

    public Sprite GetSprite()
        => Entry.GetSprite();

    public bool Equipped()
        => IsEquipped;

    public bool IsUnlocked()
        => AppManager.Instance.ConfigManager.PackIsGenerallyUnlocked(Entry);

    public string GetUnlockCondition()
        => AppManager.Instance.ConfigManager.GetPackUnlockCondition(Entry);
}
