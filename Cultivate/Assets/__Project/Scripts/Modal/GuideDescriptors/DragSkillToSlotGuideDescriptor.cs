
public class DragSkillToSlotGuideDescriptor : GuideDescriptor
{
    private SkillDescriptor _from;
    private DeckIndex _to;

    public DragSkillToSlotGuideDescriptor(SkillDescriptor from, DeckIndex to)
    {
        _from = from;
        _to = to;
    }

    public Address FindStart()
    {
        DeckIndex? deckIndex = RunManager.Instance.Environment.FindDeckIndex(_from);
        if (deckIndex == null)
            return null;
        return deckIndex.Value.ToAddress();
    }

    public Address FindEnd()
    {
        return _to.ToAddress();
    }
}
