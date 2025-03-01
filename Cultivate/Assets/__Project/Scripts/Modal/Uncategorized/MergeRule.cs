
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

            d.MergeTarget = new(
                mergeType:              "全等合成",
                valid:                  true,
                errorMessage:           null,
                resultEntry:            rhs.GetEntry(),
                resultJingJie:          (rhs.GetJingJie() + 2).ClampUpper(rhs.GetEntry().HighestJingJie),
                resultWuXing:           rhs.GetWuXing(),
                pred:                   null);
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

            d.MergeTarget = new(
                mergeType:              "同名合成",
                valid:                  true,
                errorMessage:           null,
                resultEntry:            rhs.GetEntry(),
                resultJingJie:          (Mathf.Max(lhs.GetJingJie(), rhs.GetJingJie()) + 1).ClampUpper(rhs.GetEntry().HighestJingJie),
                resultWuXing:           rhs.GetWuXing(),
                pred:                   null);
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
                d.MergeTarget = new(
                    mergeType:              "境界限制",
                    valid:                  false,
                    errorMessage:           "无法合成原因\n玩家境界需要至少不低于两张卡牌中的一张的境界",
                    resultEntry:            null,
                    resultJingJie:          null,
                    resultWuXing:           null,
                    pred:                   null);
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

            d.MergeTarget = new(
                mergeType: "境界置换",
                valid: true,
                errorMessage: null,
                resultEntry: lowerJingJieSkill.GetEntry(),
                resultJingJie: (lowerJingJie + 1).ClampUpper(lowerJingJieSkill.GetEntry().HighestJingJie),
                resultWuXing: lowerJingJieSkill.GetWuXing(),
                pred: null);
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
                d.MergeTarget = new(
                    mergeType:              "境界限制",
                    valid:                  false,
                    errorMessage:           "无法合成原因\n玩家境界需要不低于两张卡牌的境界",
                    resultEntry:            null,
                    resultJingJie:          null,
                    resultWuXing:           null,
                    pred:                   null);
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

            d.MergeTarget = new(
                mergeType: "同五行合成",
                valid: true,
                errorMessage: null,
                resultEntry: null,
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

            d.MergeTarget = new(
                mergeType:              "相生五行合成",
                valid:                  true,
                errorMessage:           null,
                resultEntry:            null,
                resultJingJie:          rhs.GetJingJie() + 1,
                resultWuXing:           WuXing.XiangShengNext(lhs.GetWuXing(), rhs.GetWuXing()).Value,
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

            d.MergeTarget = new(
                mergeType:              "同境界合成",
                valid:                  true,
                errorMessage:           null,
                resultEntry:            null,
                resultJingJie:          rhs.GetJingJie() + 1,
                resultWuXing:           null,
                pred:                   skillEntry => !skillEntry.WuXing.HasValue ||
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

            d.MergeTarget = new(
                mergeType:              "同五行化神置换",
                valid:                  true,
                errorMessage:           null,
                resultEntry:            null,
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

            d.MergeTarget = new(
                mergeType:              "相生五行化神置换",
                valid:                  true,
                errorMessage:           null,
                resultEntry:            null,
                resultJingJie:          rhs.GetJingJie(),
                resultWuXing:           WuXing.XiangShengNext(lhs.GetWuXing(), rhs.GetWuXing()).Value,
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

            d.MergeTarget = new(
                mergeType:              "化神置换",
                valid:                  true,
                errorMessage:           null,
                resultEntry:            null,
                resultJingJie:          rhs.GetJingJie(),
                resultWuXing:           null,
                pred:                   skillEntry => !skillEntry.WuXing.HasValue ||
                                                      (skillEntry.WuXing != lhs.GetWuXing() &&
                                                       skillEntry.WuXing != rhs.GetWuXing()));
            d.State = MergeDetails.MergeState.Success;
        });

    public static MergeRule Fallback = new(
        name: "无法合成",
        errorMessage: null,
        processMerge: d =>
        {
            d.MergeTarget = new(
                mergeType:              "无法合成",
                valid:                  false,
                errorMessage:           "无法合成原因\n未知公式",
                resultEntry:            null,
                resultJingJie:          null,
                resultWuXing:           null,
                pred:                   null);
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
