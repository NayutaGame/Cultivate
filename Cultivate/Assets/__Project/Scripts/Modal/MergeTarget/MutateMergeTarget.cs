
using System.Collections.Generic;

public class MutateMergeTarget : MergeTarget
{
    public MutateMergeTarget(
        string mergeType,
        SkillEntry resultEntry,
        JingJie resultJingJie,
        WuXing resultWuXing,
        List<SkillEntry> oldMutators,
        SkillEntry newMutator
        ) : base(mergeType, true, null, resultEntry, resultJingJie, resultWuXing, oldMutators, new List<SkillEntry>{ newMutator }, null)
    {
    }

    public override void Execute(MergeDetails d, RunSkillListModel hand)
    {
        RunSkill result = RunSkill.FromMutation(d.MergeTarget.ResultEntry, d.MergeTarget.ResultJingJie, d.MergeTarget.ResultMutators);
        hand.Replace(d.Rhs.ToDeckIndex().Index, result);
        hand.Remove(d.Lhs);
    }
}