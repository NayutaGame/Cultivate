
public class DeckChangedSignal : Signal
{
    public DeckIndex FromIndex;
    public DeckIndex ToIndex;

    public DeckChangedSignal(DeckIndex fromIndex, DeckIndex toIndex)
    {
        FromIndex = fromIndex;
        ToIndex = toIndex;
    }
}
