
public interface IDeckIndex
{
    bool InField { get; }
    int Index { get; }
    string ToString();
    DeckIndex Reify();
}