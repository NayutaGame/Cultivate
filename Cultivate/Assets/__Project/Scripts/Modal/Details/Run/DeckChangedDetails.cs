
public class DeckChangedDetails : ClosureDetails
{
    public DeckIndex FromIndex;
    public DeckIndex ToIndex;

    public DeckChangedDetails(DeckIndex fromIndex, DeckIndex toIndex)
    {
        FromIndex = fromIndex;
        ToIndex = toIndex;
    }
}
