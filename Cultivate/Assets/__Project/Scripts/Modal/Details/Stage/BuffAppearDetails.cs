
public class BuffAppearDetails : ClosureDetails
{
    public StageEntity Owner;
    public BuffEntry Entry;
    public int InitialStack;

    public BuffAppearDetails(StageEntity owner, BuffEntry entry, int initialStack)
    {
        Owner = owner;
        Entry = entry;
        InitialStack = initialStack;
    }
}
