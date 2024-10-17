
public class FieldChangedSignal : Signal
{
    public DeckIndex FromIndex;
    public DeckIndex ToIndex;

    public FieldChangedSignal(DeckIndex fromIndex, DeckIndex toIndex)
    {
        FromIndex = fromIndex;
        ToIndex = toIndex;
    }
}
