
public class MutateMergeTarget : MergeTarget
{
    private RunSkill _skill;
    private RunSkill _mutator;
    
    public MutateMergeTarget(
        string mergeType,
        SkillEntry resultEntry,
        JingJie? resultJingJie,
        WuXing? resultWuXing,
        RunSkill skill,
        RunSkill mutator) : base(mergeType, true, null, resultEntry, resultJingJie, resultWuXing, null)
    {
        _skill = skill;
        _mutator = mutator;
    }

    public override void Execute(MergeDetails d, SkillInventory hand)
    {
        _skill.Mutate(_mutator);
     
        // mutator to be removed
        // hand.Remove(d.Lhs);
    }
}