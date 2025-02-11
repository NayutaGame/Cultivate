
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
}
