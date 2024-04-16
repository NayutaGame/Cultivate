
public struct DeckIndex
{
    private bool _inField;
    public bool InField => _inField;
    private int _index;
    public int Index => _index;

    public DeckIndex(bool inField, int index)
    {
        _inField = inField;
        _index = index;
    }

    public Address ToAddress()
    {
        if (_inField)
            return new Address($"Run.Environment.Hero.Slots#{_index}");
        else
            return new Address($"Run.Environment.Hand#{_index}");
    }
}
