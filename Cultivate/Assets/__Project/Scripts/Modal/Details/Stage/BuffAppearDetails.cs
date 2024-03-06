
public class BuffAppearDetails : EventDetails
{
    public BuffEntry _entry;
    public int _initialStack;

    public BuffAppearDetails(BuffEntry entry, int initialStack)
    {
        _entry = entry;
        _initialStack = initialStack;
    }
}
