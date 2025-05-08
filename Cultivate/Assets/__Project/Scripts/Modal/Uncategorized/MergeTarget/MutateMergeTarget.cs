
public class MutateMergeTarget : MergeTarget
{
    private bool _rhsIsMutator;
    
    public MutateMergeTarget(
        string mergeType,
        SkillEntry resultEntry,
        JingJie? resultJingJie,
        WuXing? resultWuXing,
        bool rhsIsMutator
        ) : base(mergeType, true, null, resultEntry, resultJingJie, resultWuXing, null)
    {
        _rhsIsMutator = rhsIsMutator;
    }

    public override void Execute(MergeDetails d, SkillInventory hand)
    {
        RunSkill skill = _rhsIsMutator ? d.Lhs : d.Rhs;
        RunSkill mutator = _rhsIsMutator ? d.Rhs : d.Lhs;
        
        skill.Mutate(mutator);
        
        hand.Replace(d.Rhs.ToDeckIndex().Index, skill);
        hand.Remove(d.Lhs);
    }
}