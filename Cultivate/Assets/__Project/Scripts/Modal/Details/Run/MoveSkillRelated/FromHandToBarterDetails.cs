
public class FromHandToBarterDetails : RunClosureDetails
{
    public DeckIndex FromIndex;
    public RunSkill FromSkill;
    
    public FromHandToBarterDetails(DeckIndex fromIndex, RunSkill fromSkill)
    {
        FromIndex = fromIndex;
        FromSkill = fromSkill;
    }

    public static FromHandToBarterDetails FromHandIndex(DeckIndex fromIndex)
        => new(fromIndex, RunManager.Instance.Environment.SkillFromDeckIndex(fromIndex));
}