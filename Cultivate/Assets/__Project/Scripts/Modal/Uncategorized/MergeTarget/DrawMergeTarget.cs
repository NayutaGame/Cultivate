
using System;

public class DrawMergeTarget : MergeTarget
{
    public DrawMergeTarget(string mergeType, JingJie? resultJingJie, WuXing? resultWuXing, Predicate<SkillEntry> pred) : base(mergeType, true, null, null, resultJingJie, resultWuXing, pred)
    {
    }

    public override void Execute(MergeDetails d, SkillInventory hand)
    {
        SkillEntryDescriptor skillEntryDescriptor = new(
            pred: d.MergeTarget.Pred,
            wuXing: d.MergeTarget.ResultWuXing,
            jingJie: d.MergeTarget.ResultJingJie);
        GainSkillBuilder b = new();
        b.Draw(skillEntryDescriptor);
        b.Create(skillEntryDescriptor.JingJie);
        b.RecordDeckIndex(d.Rhs.ToDeckIndex());
        b.Add();
        hand.Remove(d.Lhs);
    }
}