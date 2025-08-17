
public class SkillMovedDetails : RunClosureDetails
{
    public DeckIndex FromIndex;
    public DeckIndex ToIndex;

    public SkillMovedDetails(DeckIndex fromIndex, DeckIndex toIndex)
    {
        FromIndex = fromIndex;
        ToIndex = toIndex;
    }
}
