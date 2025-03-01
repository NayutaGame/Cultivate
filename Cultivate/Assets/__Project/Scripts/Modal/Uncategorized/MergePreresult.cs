
using System;
using CLLibrary;
using UnityEngine;

public class MergePreresult
{
    public readonly string MergeType;
    public readonly bool Valid;
    public readonly string ErrorMessage;
    public readonly SkillEntry ResultEntry;
    public readonly JingJie? ResultJingJie;
    public readonly WuXing? ResultWuXing;
    public readonly Predicate<SkillEntry> Pred;

    public MergePreresult(
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

    public static Tuple<Func<RunSkill, RunSkill, JingJie, bool>, Func<RunSkill, RunSkill, JingJie, MergePreresult>>[] MergeRules = null;
}
