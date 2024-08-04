
using System;
using CLLibrary;
using UnityEngine;

public class MergePreresult
{
    public readonly string MergeType;
    public readonly bool Valid;
    public readonly string ErrorMessage;
    public readonly JingJie? ResultJingJie;
    public readonly WuXing? ResultWuXing;
    public readonly SkillEntry ResultEntry;
    public readonly Predicate<SkillEntry> Pred;

    private MergePreresult(string mergeType, bool valid, string errorMessage, JingJie? resultJingJie, WuXing? resultWuXing, SkillEntry resultEntry, Predicate<SkillEntry> pred)
    {
        MergeType = mergeType;
        Valid = valid;
        ErrorMessage = errorMessage;
        ResultJingJie = resultJingJie;
        ResultWuXing = resultWuXing;
        ResultEntry = resultEntry;
        Pred = pred;
    }

    public static Tuple<Func<RunSkill, RunSkill, JingJie, bool>, Func<RunSkill, RunSkill, JingJie, MergePreresult>>[]
        MergeRules =
            new Tuple<Func<RunSkill, RunSkill, JingJie, bool>, Func<RunSkill, RunSkill, JingJie, MergePreresult>>[]
            {
                new(IsCongruent, FromCongruent),
                new(IsSameName, FromSameName),
                // from formula
                new(IsSameWuXing, FromSameWuXing),
                new(IsXiangShengWuXing, FromXiangShengWuXing),
                new(IsSameJingJie, FromSameJingJie),
                new(IsHuaShenReroll, FromHuaShenReroll),
            };

    public static bool IsCongruent(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => lhs.GetEntry() == rhs.GetEntry() && lhs.GetJingJie() == rhs.GetJingJie() &&
           lhs.GetJingJie() < rhs.GetEntry().HighestJingJie;

    public static MergePreresult FromCongruent(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => new(
            mergeType:              "全等合成",
            valid:                  true,
            errorMessage:           null,
            resultJingJie:          (rhs.GetJingJie() + 2).ClampUpper(rhs.GetEntry().HighestJingJie),
            resultWuXing:           rhs.GetWuXing(),
            resultEntry:            rhs.GetEntry(),
            pred:                   null);

    public static bool IsSameName(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => lhs.GetEntry() == rhs.GetEntry() &&
           Mathf.Max(lhs.GetJingJie(), rhs.GetJingJie()) < rhs.GetEntry().HighestJingJie;

    public static MergePreresult FromSameName(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => new(
            mergeType:              "同名合成",
            valid:                  true,
            errorMessage:           null,
            resultJingJie:          (Mathf.Max(lhs.GetJingJie(), rhs.GetJingJie()) + 1).ClampUpper(rhs.GetEntry().HighestJingJie),
            resultWuXing:           rhs.GetWuXing(),
            resultEntry:            rhs.GetEntry(),
            pred:                   null);

    public static bool IsSameWuXing(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => lhs.GetWuXing() == rhs.GetWuXing() && lhs.GetJingJie() == rhs.GetJingJie() &&
           lhs.GetJingJie() < lhs.GetEntry().HighestJingJie;
    
    public static MergePreresult FromSameWuXing(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => new(
            mergeType:              "同五行合成",
            valid:                  rhs.GetJingJie() <= playerJingJie && lhs.GetJingJie() <= playerJingJie,
            errorMessage:           "玩家境界需要不低于卡牌境界",
            resultJingJie:          rhs.GetJingJie() + 1,
            resultWuXing:           rhs.GetWuXing(),
            resultEntry:            null,
            pred:                   skillEntry => skillEntry != lhs.GetEntry() && skillEntry != rhs.GetEntry());

    public static bool IsXiangShengWuXing(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => WuXing.XiangSheng(lhs.GetWuXing(), rhs.GetWuXing()) && lhs.GetJingJie() == rhs.GetJingJie() &&
           lhs.GetJingJie() < lhs.GetEntry().HighestJingJie;

    public static MergePreresult FromXiangShengWuXing(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => new(
            mergeType:              "相生五行合成",
            valid:                  rhs.GetJingJie() <= playerJingJie && lhs.GetJingJie() <= playerJingJie,
            errorMessage:           "玩家境界需要不低于卡牌境界",
            resultJingJie:          rhs.GetJingJie() + 1,
            resultWuXing:           WuXing.XiangShengNext(lhs.GetWuXing(), rhs.GetWuXing()).Value,
            resultEntry:            null,
            pred:                   null);

    public static bool IsSameJingJie(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => lhs.GetJingJie() == rhs.GetJingJie() && lhs.GetJingJie() < lhs.GetEntry().HighestJingJie;

    public static MergePreresult FromSameJingJie(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => new(
            mergeType:              "同境界合成",
            valid:                  rhs.GetJingJie() <= playerJingJie && lhs.GetJingJie() <= playerJingJie,
            errorMessage:           "玩家境界需要不低于卡牌境界",
            resultJingJie:          rhs.GetJingJie() + 1,
            resultWuXing:           null,
            resultEntry:            null,
            pred:                   skillEntry => skillEntry.WuXing.HasValue && skillEntry.WuXing != lhs.GetWuXing() &&
                                                  skillEntry.WuXing != rhs.GetWuXing());

    public static bool IsHuaShenReroll(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => lhs.GetJingJie() == rhs.GetJingJie() && lhs.GetJingJie() == JingJie.HuaShen;

    public static MergePreresult FromHuaShenReroll(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => new(
            mergeType:              "化神置换",
            valid:                  rhs.GetJingJie() <= playerJingJie && lhs.GetJingJie() <= playerJingJie,
            errorMessage:           "玩家境界需要不低于卡牌境界",
            resultJingJie:          rhs.GetJingJie(),
            resultWuXing:           null,
            resultEntry:            null,
            pred:                   skillEntry => skillEntry != lhs.GetEntry() && skillEntry != rhs.GetEntry());

    public static MergePreresult FromDefault(RunSkill lhs, RunSkill rhs, JingJie playerJingJie)
        => new(
            mergeType:              "无法合成",
            valid:                  false,
            errorMessage:           "非同名卡合成时需要相同境界",
            resultJingJie:          null,
            resultWuXing:           null,
            resultEntry:            null,
            pred:                   null);
}
