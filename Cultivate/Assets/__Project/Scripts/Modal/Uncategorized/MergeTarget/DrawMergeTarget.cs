
using System;

public class DrawMergeTarget : MergeTarget
{
    public DrawMergeTarget(string mergeType, JingJie resultJingJie, WuXing resultWuXing, Predicate<SkillEntry> pred)
        : base(mergeType, true, null, null, resultJingJie, resultWuXing, null, null, pred)
    {
    }

    public override void Execute(MergeDetails d, SkillInventory hand)
    {
        SkillEntryQuery drawStrategy = SkillEntryQuery.FromPredWuXingBaseJingJieBound(
            predicate: d.MergeTarget.Pred,
            wuXing: d.MergeTarget.ResultWuXing,
            baseJingJieBound: new(JingJie.LianQi, d.MergeTarget.ResultJingJie));
        GainSkillBuilder b = new();
        b.Draw(drawStrategy, d.MergeTarget.ResultJingJie, d.Rhs.ToDeckIndex());
        b.Execute();
        hand.Remove(d.Lhs);
    }
}