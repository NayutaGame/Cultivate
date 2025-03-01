
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
}
