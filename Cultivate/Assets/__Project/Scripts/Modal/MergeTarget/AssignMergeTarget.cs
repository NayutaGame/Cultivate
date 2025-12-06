
using System.Collections.Generic;

public class AssignMergeTarget : MergeTarget
{
    public AssignMergeTarget(
        string mergeType,
        SkillEntry resultEntry,
        JingJie resultJingJie,
        WuXing resultWuXing,
        List<SkillEntry> lhsMutators,
        List<SkillEntry> rhsMutators) :
        base(mergeType, true, null, resultEntry, resultJingJie, resultWuXing, lhsMutators, rhsMutators, null)
    {
    }

    public override void Execute(MergeDetails d, RunSkillListModel hand)
    {
        RunSkill result = RunSkill.FromMutation(d.MergeTarget.ResultEntry, d.MergeTarget.ResultJingJie, d.MergeTarget.ResultMutators);
        hand.Replace(d.Rhs.ToDeckIndex().Index, result);
        hand.Remove(d.Lhs);
    }
}