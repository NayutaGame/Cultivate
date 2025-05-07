
public class AssignMergeTarget : MergeTarget
{
    public AssignMergeTarget(string mergeType, SkillEntry resultEntry, JingJie? resultJingJie, WuXing? resultWuXing) : base(mergeType, true, null, resultEntry, resultJingJie, resultWuXing, null)
    {
    }

    public override void Execute(MergeDetails d, SkillInventory hand)
    {
        d.Rhs.SetEntry(d.MergeTarget.ResultEntry);
        d.Rhs.JingJie = d.MergeTarget.ResultJingJie.Value;
        hand.Remove(d.Lhs);
    }
}