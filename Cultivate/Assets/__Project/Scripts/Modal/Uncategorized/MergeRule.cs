
using System;
using CLLibrary;
using UnityEngine;

public class MergeRule
{
    private string _name;
    private string _errorMessage;
    private Action<MergeDetails> _processMerge;
    private int _order;
    public int Order
        => _order;

    public MergeRule(string name, string errorMessage, Action<MergeDetails> processMerge, int order = 0)
    {
        _name = name;
        _errorMessage = errorMessage;
        _processMerge = processMerge;
        _order = order;
    }

    public void ProcessMerge(MergeDetails d)
        => _processMerge(d);

    public static MergeRule BothMutator = new(
        name: "两张墨染",
        errorMessage: "墨染牌之间无法合成",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = rhs.GetEntry().IsMutator && lhs.GetEntry().IsMutator;
            if (cond)
            {
                d.State = MergeDetails.MergeState.Cancel;
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "两张墨染",
                    errorMessage:           "无法合成原因\n墨染牌之间无法合成");
                return;
            }

            d.State = MergeDetails.MergeState.Continue;
        });

    public static MergeRule Mutate = new(
        name: "墨染",
        errorMessage: null,
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = rhs.GetEntry().IsMutator != lhs.GetEntry().IsMutator;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }
            
            bool rhsIsMutator = rhs.GetEntry().IsMutator;
            
            RunSkill skill = rhsIsMutator ? lhs : rhs;
            RunSkill mutator = rhsIsMutator ? rhs : lhs;

            bool canMutate = skill.CanMutate(mutator);

            if (canMutate)
            {
                d.MergeTarget = new MutateMergeTarget(
                    mergeType:              "墨染",
                    resultEntry:            rhs.GetEntry(),
                    resultJingJie:          (rhs.GetJingJie() + 2).ClampUpper(rhs.GetEntry().HighestJingJie),
                    resultWuXing:           rhs.GetWuXing(),
                    rhsIsMutator:           rhsIsMutator);
                d.State = MergeDetails.MergeState.Success;
            }
            else
            {
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "墨染",
                    errorMessage:           "无法对这张牌应用这个墨染");
                d.State = MergeDetails.MergeState.Cancel;
            }
        });

    public static MergeRule Congruent = new(
        name: "全等合成",
        errorMessage: null,
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;
            
            bool cond = lhs.GetEntry() == rhs.GetEntry() &&
                        lhs.GetJingJie() == rhs.GetJingJie() &&
                        lhs.GetJingJie() < rhs.GetEntry().HighestJingJie;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = new AssignMergeTarget(
                mergeType:              "全等合成",
                resultEntry:            rhs.GetEntry(),
                resultJingJie:          (rhs.GetJingJie() + 2).ClampUpper(rhs.GetEntry().HighestJingJie),
                resultWuXing:           rhs.GetWuXing());
            d.State = MergeDetails.MergeState.Success;
        });

    public static MergeRule SameName = new(
        name: "同名合成",
        errorMessage: null,
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetEntry() == rhs.GetEntry() &&
                        Mathf.Max(lhs.GetJingJie(), rhs.GetJingJie()) < lhs.GetEntry().HighestJingJie;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = new AssignMergeTarget(
                mergeType:              "同名合成",
                resultEntry:            rhs.GetEntry(),
                resultJingJie:          (Mathf.Max(lhs.GetJingJie(), rhs.GetJingJie()) + 1).ClampUpper(rhs.GetEntry().HighestJingJie),
                resultWuXing:           rhs.GetWuXing());
            d.State = MergeDetails.MergeState.Success;
        });

    public static MergeRule JingJieLimit1 = new(
        name: "境界限制",
        errorMessage: "无法合成原因\n玩家境界需要至少不低于两张卡牌中的一张的境界",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = rhs.GetJingJie() <= d.PlayerJingJie || lhs.GetJingJie() <= d.PlayerJingJie;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Cancel;
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "境界限制",
                    errorMessage:           "无法合成原因\n玩家境界需要至少不低于两张卡牌中的一张的境界");
                return;
            }

            d.State = MergeDetails.MergeState.Continue;
        });

    public static MergeRule JingJieReplace = new(
        name: "境界置换",
        errorMessage: null,
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetEntry() != rhs.GetEntry() &&
                        lhs.GetJingJie() != rhs.GetJingJie();
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            JingJie lowerJingJie = Mathf.Min(lhs.GetJingJie(), rhs.GetJingJie());
            RunSkill lowerJingJieSkill = lhs.GetJingJie() < rhs.GetJingJie() ? lhs : rhs;

            d.MergeTarget = new AssignMergeTarget(
                mergeType: "境界置换",
                resultEntry: lowerJingJieSkill.GetEntry(),
                resultJingJie: (lowerJingJie + 1).ClampUpper(lowerJingJieSkill.GetEntry().HighestJingJie),
                resultWuXing: lowerJingJieSkill.GetWuXing());
            d.State = MergeDetails.MergeState.Success;
        });

    public static MergeRule JingJieLimit2 = new(
        name: "境界限制",
        errorMessage: "无法合成原因\n玩家境界需要不低于两张卡牌的境界",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = rhs.GetJingJie() <= d.PlayerJingJie && lhs.GetJingJie() <= d.PlayerJingJie;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Cancel;
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "境界限制",
                    errorMessage:           "无法合成原因\n玩家境界需要不低于两张卡牌的境界");
                return;
            }

            d.State = MergeDetails.MergeState.Continue;
        });

    public static MergeRule SameWuXing = new(
        name: "同五行合成",
        errorMessage: null,
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetWuXing() == rhs.GetWuXing() &&
                        lhs.GetJingJie() == rhs.GetJingJie() &&
                        lhs.GetJingJie() < lhs.GetEntry().HighestJingJie;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = new DrawMergeTarget(
                mergeType: "同五行合成",
                resultJingJie: rhs.GetJingJie() + 1,
                resultWuXing: rhs.GetWuXing(),
                pred: skillEntry => skillEntry != lhs.GetEntry() && skillEntry != rhs.GetEntry());
            d.State = MergeDetails.MergeState.Success;
        });

    public static MergeRule XiangShengWuXing = new(
        name: "相生五行合成",
        errorMessage: null,
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = WuXing.XiangSheng(lhs.GetWuXing(), rhs.GetWuXing()) &&
                        lhs.GetJingJie() == rhs.GetJingJie() &&
                        lhs.GetJingJie() < lhs.GetEntry().HighestJingJie;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = new DrawMergeTarget(
                mergeType:              "相生五行合成",
                resultJingJie:          rhs.GetJingJie() + 1,
                resultWuXing:           WuXing.XiangShengNext(lhs.GetWuXing(), rhs.GetWuXing()),
                pred:                   null);
            d.State = MergeDetails.MergeState.Success;
        });

    public static MergeRule SameJingJie = new(
        name: "同境界合成",
        errorMessage: null,
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetJingJie() == rhs.GetJingJie() &&
                        lhs.GetJingJie() < lhs.GetEntry().HighestJingJie;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = new DrawMergeTarget(
                mergeType:              "同境界合成",
                resultJingJie:          rhs.GetJingJie() + 1,
                resultWuXing:           null,
                pred:                   skillEntry => !skillEntry.WuXing.IsBasic() ||
                                                      (skillEntry.WuXing != lhs.GetWuXing() &&
                                                       skillEntry.WuXing != rhs.GetWuXing()));
            d.State = MergeDetails.MergeState.Success;
        });

    public static MergeRule SameWuXingHuaShenReroll = new(
        name: "同五行化神置换",
        errorMessage: null,
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetJingJie() == rhs.GetJingJie() &&
                        lhs.GetJingJie() == JingJie.HuaShen &&
                        lhs.GetWuXing() == rhs.GetWuXing();
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = new DrawMergeTarget(
                mergeType:              "同五行化神置换",
                resultJingJie:          rhs.GetJingJie(),
                resultWuXing:           rhs.GetWuXing(),
                pred:                   skillEntry => skillEntry != lhs.GetEntry() && skillEntry != rhs.GetEntry());
            d.State = MergeDetails.MergeState.Success;
        });

    public static MergeRule XiangShengWuXingHuaShenReroll = new(
        name: "相生五行化神置换",
        errorMessage: null,
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetJingJie() == rhs.GetJingJie() &&
                        lhs.GetJingJie() == JingJie.HuaShen &&
                        WuXing.XiangSheng(lhs.GetWuXing(), rhs.GetWuXing());
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = new DrawMergeTarget(
                mergeType:              "相生五行化神置换",
                resultJingJie:          rhs.GetJingJie(),
                resultWuXing:           WuXing.XiangShengNext(lhs.GetWuXing(), rhs.GetWuXing()),
                pred:                   null);
            d.State = MergeDetails.MergeState.Success;
        });

    public static MergeRule HuaShenReroll = new(
        name: "化神置换",
        errorMessage: null,
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetJingJie() == rhs.GetJingJie() &&
                        lhs.GetJingJie() == JingJie.HuaShen;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = new DrawMergeTarget(
                mergeType:              "化神置换",
                resultJingJie:          rhs.GetJingJie(),
                resultWuXing:           null,
                pred:                   skillEntry => !skillEntry.WuXing.IsBasic() ||
                                                      (skillEntry.WuXing != lhs.GetWuXing() &&
                                                       skillEntry.WuXing != rhs.GetWuXing()));
            d.State = MergeDetails.MergeState.Success;
        });

    public static MergeRule Fallback = new(
        name: "无法合成",
        errorMessage: null,
        processMerge: d =>
        {
            d.MergeTarget = new InvalidMergeTarget(
                mergeType:              "无法合成",
                errorMessage:           "无法合成原因\n未知公式");
            d.State = MergeDetails.MergeState.Cancel;
        });

    public static MergeRule Trivial = new(
        name: "过渡",
        errorMessage: null,
        processMerge: d =>
        {
            d.State = MergeDetails.MergeState.Continue;
        });
    
    public static MergeRule[] DefaultMergeRules = new[] {
        BothMutator,
        Mutate,
        Congruent,
        SameName,
        JingJieLimit1,
        JingJieReplace,
        JingJieLimit2,
        SameWuXing,
        XiangShengWuXing,
        SameJingJie,
        SameWuXingHuaShenReroll,
        XiangShengWuXingHuaShenReroll,
        HuaShenReroll,
        Fallback,
    };
}
