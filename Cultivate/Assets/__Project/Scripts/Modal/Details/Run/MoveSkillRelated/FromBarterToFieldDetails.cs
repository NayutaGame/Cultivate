
public class FromBarterToFieldDetails : RunClosureDetails
{
    public RunSkill FromSkill;
    public DeckIndex FromIndex;
    public SkillSlot ToSlot;
    public DeckIndex ToIndex;

    public FromBarterToFieldDetails(RunSkill fromSkill, DeckIndex fromIndex, SkillSlot toSlot, DeckIndex toIndex)
    {
        FromSkill = fromSkill;
        FromIndex = fromIndex;
        ToSlot = toSlot;
        ToIndex = toIndex;
    }
}