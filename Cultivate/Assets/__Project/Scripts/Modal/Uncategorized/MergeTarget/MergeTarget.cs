
using System;

public abstract class MergeTarget
{
    public readonly string MergeType;
    public readonly bool Valid;
    public readonly string ErrorMessage;
    public readonly SkillEntry ResultEntry;
    public readonly JingJie? ResultJingJie;
    public readonly WuXing? ResultWuXing;
    public readonly Predicate<SkillEntry> Pred;

    public MergeTarget(
        string mergeType,
        bool valid,
        string errorMessage,
        SkillEntry resultEntry,
        JingJie? resultJingJie,
        WuXing? resultWuXing,
        Predicate<SkillEntry> pred)
    {
        MergeType = mergeType;
        Valid = valid;
        ErrorMessage = errorMessage;
        ResultEntry = resultEntry;
        ResultJingJie = resultJingJie;
        ResultWuXing = resultWuXing;
        Pred = pred;
    }

    public virtual void Execute(MergeDetails d, SkillInventory hand)
    {
        
    }
}
