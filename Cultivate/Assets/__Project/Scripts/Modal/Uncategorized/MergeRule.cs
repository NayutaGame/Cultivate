
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class MergeRule
{
    private string _name;
    private Action<MergeDetails> _processMerge;
    private int _order;

    private MergeRule(string name, Action<MergeDetails> processMerge, int order = 0)
    {
        _name = name;
        _processMerge = processMerge;
        _order = order;
    }
    
    public int Order => _order;
    public void ProcessMerge(MergeDetails d) => _processMerge(d);

    public static readonly MergeRule BothMutator = new(
        name: "两张墨染",
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

    public static readonly MergeRule Mutate = new(
        name: "墨染",
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
                    resultEntry:            skill.GetEntry(),
                    resultJingJie:          skill.GetJingJie(),
                    resultWuXing:           skill.GetWuXing(),
                    oldMutators:            skill.GetMutators(),
                    newMutator:             mutator.GetEntry());
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

    public static readonly MergeRule SameNameHasFanXu = new(
        name: "同名含有返虚",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;
            
            bool cond = lhs.GetEntry() == rhs.GetEntry() &&
                        (lhs.GetJingJie() == JingJie.FanXu || rhs.GetJingJie() == JingJie.FanXu);
            if (cond)
            {
                d.State = MergeDetails.MergeState.Cancel;
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "同名含有返虚",
                    errorMessage:           "无法合成原因\n返虚已经是最高等级了，无法参与合成");
                return;
            }

            d.State = MergeDetails.MergeState.Continue;
        });

    public static readonly MergeRule SameNameBothHuaShen = new(
        name: "同名双化神",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;
            
            bool cond = lhs.GetEntry() == rhs.GetEntry() &&
                        lhs.GetJingJie() == JingJie.HuaShen &&
                        rhs.GetJingJie() == JingJie.HuaShen;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = new AssignMergeTarget(
                mergeType:              "同名双化神",
                resultEntry:            rhs.GetEntry(),
                resultJingJie:          JingJie.FanXu,
                resultWuXing:           rhs.GetWuXing(),
                lhsMutators:            lhs.GetMutators(),
                rhsMutators:            rhs.GetMutators());
            d.State = MergeDetails.MergeState.Success;
        });

    public static readonly MergeRule SameNameOneHuaShen = new(
        name: "同名单化神",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;
            
            bool cond = lhs.GetEntry() == rhs.GetEntry() &&
                        (lhs.GetJingJie() == JingJie.HuaShen ^ rhs.GetJingJie() == JingJie.HuaShen);
            if (cond)
            {
                d.State = MergeDetails.MergeState.Cancel;
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "同名单化神",
                    errorMessage:           "无法合成原因\n只有一张化神牌不足以突破");
                return;
            }

            d.State = MergeDetails.MergeState.Continue;
        });

    public static readonly MergeRule SameNameSameJingJieLEYuanYing = new(
        name: "同名同境界",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;
            
            bool cond = lhs.GetEntry() == rhs.GetEntry() &&
                        lhs.GetJingJie() == rhs.GetJingJie() &&
                        lhs.GetJingJie() <= JingJie.YuanYing;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = new AssignMergeTarget(
                mergeType:              "同名同境界",
                resultEntry:            rhs.GetEntry(),
                resultJingJie:          (rhs.GetJingJie() + 2).ClampUpper(JingJie.HuaShen),
                resultWuXing:           rhs.GetWuXing(),
                lhsMutators:            lhs.GetMutators(),
                rhsMutators:            rhs.GetMutators());
            d.State = MergeDetails.MergeState.Success;
        });

    public static readonly MergeRule SameNameDiffJingJieLEYuanYing = new(
        name: "同名不同境界",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetEntry() == rhs.GetEntry() &&
                        Mathf.Max(lhs.GetJingJie(), rhs.GetJingJie()) <= JingJie.YuanYing;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = new AssignMergeTarget(
                mergeType:              "同名不同境界",
                resultEntry:            rhs.GetEntry(),
                resultJingJie:          (Mathf.Max(lhs.GetJingJie(), rhs.GetJingJie()) + 1).ClampUpper(JingJie.HuaShen),
                resultWuXing:           rhs.GetWuXing(),
                lhsMutators:            lhs.GetMutators(),
                rhsMutators:            rhs.GetMutators());
            d.State = MergeDetails.MergeState.Success;
        });

    public static readonly MergeRule JingJieLimitGEEither = new(
        name: "境界限制",
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

    public static readonly MergeRule DiffJingJieHasFanXu = new(
        name: "不同境界含有返虚",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetEntry() != rhs.GetEntry() &&
                        lhs.GetJingJie() != rhs.GetJingJie() &&
                        (lhs.GetJingJie() == JingJie.FanXu || rhs.GetJingJie() == JingJie.FanXu);
            if (cond)
            {
                d.State = MergeDetails.MergeState.Cancel;
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "含有返虚",
                    errorMessage:           "无法合成原因\n返虚和非返虚卡牌无法合成");
                return;
            }

            d.State = MergeDetails.MergeState.Continue;
        });

    public static readonly MergeRule DiffJingJie = new(
        name: "境界置换",
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
                mergeType:              "境界置换",
                resultEntry:            lowerJingJieSkill.GetEntry(),
                resultJingJie:          (lowerJingJie + 1).ClampUpper(lowerJingJieSkill.GetEntry().HighestJingJie).ClampUpper(JingJie.HuaShen),
                resultWuXing:           lowerJingJieSkill.GetWuXing(),
                lhsMutators:            lowerJingJieSkill.GetMutators(),
                rhsMutators:            null);
            d.State = MergeDetails.MergeState.Success;
        });

    public static readonly MergeRule JingJieLimitGEBoth = new(
        name: "境界限制",
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
    
    public static readonly MergeRule SameWuXingBothFanXu = new(
        name: "同五行返虚返虚",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetEntry() != rhs.GetEntry() &&
                        lhs.GetWuXing() == rhs.GetWuXing() &&
                        lhs.GetJingJie() == JingJie.FanXu &&
                        rhs.GetJingJie() == JingJie.FanXu;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = DrawMergeTarget.From(
                mergeType: "同五行返虚重抽",
                resultJingJie: JingJie.FanXu,
                resultWuXing: rhs.GetWuXing(),
                pred: skillEntry => skillEntry != lhs.GetEntry() && skillEntry != rhs.GetEntry(),
                resultJingJieIsBaseJingJie: d.ResultJingJieIsBaseJingJie);
            d.State = MergeDetails.MergeState.Success;
        });
    
    public static readonly MergeRule SameWuXingBothHuaShen = new(
        name: "同五行化神化神",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetEntry() != rhs.GetEntry() &&
                        lhs.GetWuXing() == rhs.GetWuXing() &&
                        lhs.GetJingJie() == JingJie.HuaShen &&
                        rhs.GetJingJie() == JingJie.HuaShen;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = DrawMergeTarget.From(
                mergeType: "同五行化神重抽",
                resultJingJie: JingJie.HuaShen,
                resultWuXing: rhs.GetWuXing(),
                pred: skillEntry => skillEntry != lhs.GetEntry() && skillEntry != rhs.GetEntry(),
                resultJingJieIsBaseJingJie: d.ResultJingJieIsBaseJingJie);
            d.State = MergeDetails.MergeState.Success;
        });
    
    public static readonly MergeRule SameWuXingBothLEYuanYing = new(
        name: "同五行合成",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetEntry() != rhs.GetEntry() &&
                        lhs.GetWuXing() == rhs.GetWuXing() &&
                        lhs.GetJingJie() == rhs.GetJingJie() &&
                        lhs.GetJingJie() <= JingJie.YuanYing;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = DrawMergeTarget.From(
                mergeType: "同五行合成",
                resultJingJie: (rhs.GetJingJie() + 1).ClampUpper(JingJie.HuaShen),
                resultWuXing: rhs.GetWuXing(),
                pred: skillEntry => skillEntry != lhs.GetEntry() && skillEntry != rhs.GetEntry(),
                resultJingJieIsBaseJingJie: d.ResultJingJieIsBaseJingJie);
            d.State = MergeDetails.MergeState.Success;
        });

    public static readonly MergeRule XiangShengWuXingBothFanXu = new(
        name: "相生五行返虚返虚",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = WuXing.XiangSheng(lhs.GetWuXing(), rhs.GetWuXing()) &&
                        lhs.GetJingJie() == JingJie.FanXu &&
                        rhs.GetJingJie() == JingJie.FanXu;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = DrawMergeTarget.From(
                mergeType: "相生五行返虚重抽",
                resultJingJie: JingJie.FanXu,
                resultWuXing: WuXing.XiangShengNext(lhs.GetWuXing(), rhs.GetWuXing()),
                pred: null,
                resultJingJieIsBaseJingJie: d.ResultJingJieIsBaseJingJie);
            d.State = MergeDetails.MergeState.Success;
        });

    public static readonly MergeRule XiangShengWuXingBothHuaShen = new(
        name: "相生五行化神化神",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = WuXing.XiangSheng(lhs.GetWuXing(), rhs.GetWuXing()) &&
                        lhs.GetJingJie() == JingJie.HuaShen &&
                        rhs.GetJingJie() == JingJie.HuaShen;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = DrawMergeTarget.From(
                mergeType: "相生五行化神重抽",
                resultJingJie: JingJie.HuaShen,
                resultWuXing: WuXing.XiangShengNext(lhs.GetWuXing(), rhs.GetWuXing()),
                pred: null,
                resultJingJieIsBaseJingJie: d.ResultJingJieIsBaseJingJie);
            d.State = MergeDetails.MergeState.Success;
        });

    public static readonly MergeRule XiangShengWuXingBothLEYuanYing = new(
        name: "相生五行合成",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = WuXing.XiangSheng(lhs.GetWuXing(), rhs.GetWuXing()) &&
                        lhs.GetJingJie() == rhs.GetJingJie() &&
                        lhs.GetJingJie() <= JingJie.YuanYing;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = DrawMergeTarget.From(
                mergeType: "相生五行合成",
                resultJingJie: (rhs.GetJingJie() + 1).ClampUpper(JingJie.HuaShen),
                resultWuXing: WuXing.XiangShengNext(lhs.GetWuXing(), rhs.GetWuXing()),
                pred: null,
                resultJingJieIsBaseJingJie: d.ResultJingJieIsBaseJingJie);
            d.State = MergeDetails.MergeState.Success;
        });

    public static readonly MergeRule BothFanXu = new(
        name: "返虚返虚",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetJingJie() == JingJie.FanXu &&
                        rhs.GetJingJie() == JingJie.FanXu;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = DrawMergeTarget.From(
                mergeType: "返虚重抽",
                resultJingJie: JingJie.FanXu,
                resultWuXing: null,
                pred: skillEntry => !skillEntry.WuXing.IsBasic() ||
                                    (skillEntry.WuXing != lhs.GetWuXing() &&
                                     skillEntry.WuXing != rhs.GetWuXing()),
                resultJingJieIsBaseJingJie: d.ResultJingJieIsBaseJingJie);
            d.State = MergeDetails.MergeState.Success;
        });

    public static readonly MergeRule BothHuaShen = new(
        name: "化神化神",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetJingJie() == JingJie.HuaShen &&
                        rhs.GetJingJie() == JingJie.HuaShen;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = DrawMergeTarget.From(
                mergeType: "化神重抽",
                resultJingJie: JingJie.HuaShen,
                resultWuXing: null,
                pred: skillEntry => !skillEntry.WuXing.IsBasic() ||
                                    (skillEntry.WuXing != lhs.GetWuXing() &&
                                     skillEntry.WuXing != rhs.GetWuXing()),
                resultJingJieIsBaseJingJie: d.ResultJingJieIsBaseJingJie);
            d.State = MergeDetails.MergeState.Success;
        });

    public static readonly MergeRule BothLEYuanYing = new(
        name: "合成",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;

            bool cond = lhs.GetJingJie() == rhs.GetJingJie() &&
                        rhs.GetJingJie() <= JingJie.YuanYing;
            if (!cond)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            d.MergeTarget = DrawMergeTarget.From(
                mergeType: "合成",
                resultJingJie: rhs.GetJingJie() + 1,
                resultWuXing: null,
                pred: skillEntry => !skillEntry.WuXing.IsBasic() ||
                                    (skillEntry.WuXing != lhs.GetWuXing() &&
                                     skillEntry.WuXing != rhs.GetWuXing()),
                resultJingJieIsBaseJingJie: d.ResultJingJieIsBaseJingJie);
            d.State = MergeDetails.MergeState.Success;
        });

    public static readonly MergeRule Fallback = new(
        name: "无法合成",
        processMerge: d =>
        {
            d.MergeTarget = new InvalidMergeTarget(
                mergeType:              "无法合成",
                errorMessage:           "无法合成原因\n未知公式");
            d.State = MergeDetails.MergeState.Cancel;
        });

    public static readonly MergeRule Trivial = new(
        name: "过渡",
        processMerge: d =>
        {
            d.State = MergeDetails.MergeState.Continue;
        });
    
    public static readonly MergeRule[] DefaultMergeRules = new[] {
        BothMutator,
        Mutate,
        SameNameHasFanXu,
        SameNameBothHuaShen,
        SameNameOneHuaShen,
        SameNameSameJingJieLEYuanYing,
        SameNameDiffJingJieLEYuanYing,
        JingJieLimitGEEither,
        DiffJingJieHasFanXu,
        DiffJingJie,
        JingJieLimitGEBoth,
        SameWuXingBothFanXu,
        SameWuXingBothHuaShen,
        SameWuXingBothLEYuanYing,
        XiangShengWuXingBothFanXu,
        XiangShengWuXingBothHuaShen,
        XiangShengWuXingBothLEYuanYing,
        BothFanXu,
        BothHuaShen,
        BothLEYuanYing,
        Fallback,
    };

    public static readonly MergeRule SameNameHasHuaShenLockFanXu = new(
        name: "同名包含化神，返虚锁住",
        processMerge: d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;
            
            bool cond = lhs.GetEntry() == rhs.GetEntry() &&
                        (lhs.GetJingJie() == JingJie.HuaShen || rhs.GetJingJie() == JingJie.HuaShen);
            if (cond)
            {
                d.State = MergeDetails.MergeState.Cancel;
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "同名含有化神",
                    errorMessage:           "无法合成原因\n化神已经是最高等级了，无法参与合成");
                return;
            }

            d.State = MergeDetails.MergeState.Continue;
        });
    
    public static readonly MergeRule[] DefaultMergeRulesLockFanXu = new[] {
        BothMutator,
        Mutate,
        SameNameHasFanXu,
        SameNameHasHuaShenLockFanXu,
        SameNameSameJingJieLEYuanYing,
        SameNameDiffJingJieLEYuanYing,
        JingJieLimitGEEither,
        DiffJingJieHasFanXu,
        DiffJingJie,
        JingJieLimitGEBoth,
        SameWuXingBothFanXu,
        SameWuXingBothHuaShen,
        SameWuXingBothLEYuanYing,
        XiangShengWuXingBothFanXu,
        XiangShengWuXingBothHuaShen,
        XiangShengWuXingBothLEYuanYing,
        BothFanXu,
        BothHuaShen,
        BothLEYuanYing,
        Fallback,
    };
    
    /**
     * Two Mutator                                      fail
     * One Mutator                                      mutate
     * 
     * Same Name FanXu-FanXu                            fail
     * Same Name FanXu-HuaShen                          fail
     * Same Name FanXu-LE YuanYing                      fail
     * Same Name HuaShen-HuaShen                        fanxu
     * Same Name HuaShen-LE YuanYing                    fail
     * Same Name Same JingJie(Both Le YuanYing)         jingJie + 2, clamp at HuaShen
     * Same Name Different JingJie(Both Le YuanYing)    jingJie + 1, clamp at HuaShen
     *
     * Player JingJie Limit, GE either                  otherwise interrupt
     *
     * Diff Name Diff JingJie FanXu-HuaShen             fail
     * Diff Name Diff JingJie FanXu-LE YuanYing         fail
     * Diff Name Diff JingJie HuaShen-LE YuanYing       JingJie replace
     * Diff Name Diff JingJie LE YuanYing-LE YuanYing   JingJie replace
     *
     * Player JingJie Limit, GE both                    otherwise interrupt
     *
     * Same WuXing Both FanXu                           FanXu WuXing Reroll
     * Same WuXing Both HuaShen                         HuaShen WuXing Reroll
     * Same WuXing Both LE-YuanYing                     WuXing Upgrade
     * 
     * XiangSheng WuXing Both FanXu                     FanXu XiangSheng Reroll
     * XiangSheng WuXing Both HuaShen                   HuaShen XiangSheng Reroll
     * XiangSheng WuXing Both LE YuanYing               XiangSheng Upgrade
     * 
     * Diff WuXing Both FanXu                           FanXu Reroll
     * Diff WuXing Both HuaShen                         HuaShen Reroll
     * Diff WuXing Both LE YuanYing                     Upgrade
     *
     * Fallback
     */

    public static readonly MergeRule GuYuanMergeRule = new(
        name: "固元",
        order: -100,
        processMerge: d =>
        {
            int value = Fib.ToValue(4 + d.Src.Dj);
            d.AddSideEffect(() =>
            {
                RunManager.Instance.Environment.GainHealthProcedure(value);
            });
            d.State = MergeDetails.MergeState.Continue;
        });
    
    public static readonly MergeRule NoMerge = new(
        name: "无法合成",
        order: -50,
        processMerge: d =>
        {
            d.MergeTarget = new InvalidMergeTarget(
                mergeType: "无法合成",
                errorMessage: "特殊卡牌不可参与合成");
            d.State = MergeDetails.MergeState.Cancel;
        });

    public static readonly MergeRule DreamCard = new(
        name: "梦中卡牌",
        order: -50,
        processMerge: d =>
        {
            d.MergeTarget = new InvalidMergeTarget(
                mergeType: "梦中卡牌",
                errorMessage: "梦中卡牌不可参与合成");
            d.State = MergeDetails.MergeState.Cancel;
        });
    
    public static readonly MergeRule MingShiMergeRule = new(
        name:                       "命石",
        order:                      -2,
        processMerge:               d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;
            RunSkill src = d.Src;
            RunSkill tgt = d.Tgt;
            
            // Mutator
            // Both MingShi
            // FanXu
            // HuaShen
            // LEYuanYing

            bool isMutator = tgt.GetEntry().IsMutator;
            if (isMutator)
            {
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "命石",
                    errorMessage:           "命石无法影响墨染");
                d.State = MergeDetails.MergeState.Cancel;
                return;
            }

            bool bothMingShi = src.GetEntry() == Encyclopedia.SkillCategory.FromName("命石") &&
                               tgt.GetEntry() == Encyclopedia.SkillCategory.FromName("命石");
            if (bothMingShi)
            {
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "命石",
                    errorMessage:           "命石与命石无法合成");
                d.State = MergeDetails.MergeState.Cancel;
                return;
            }

            bool fanXu = tgt.GetJingJie() == JingJie.FanXu;
            if (fanXu)
            {
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "命石",
                    errorMessage:           "命石无法影响返虚卡牌");
                d.State = MergeDetails.MergeState.Cancel;
                return;
            }

            bool huaShen = tgt.GetJingJie() == JingJie.HuaShen;
            if (huaShen)
            {
                d.MergeTarget = new AssignMergeTarget(
                    mergeType:              "命石",
                    resultEntry:            tgt.GetEntry(),
                    resultJingJie:          JingJie.FanXu,
                    resultWuXing:           tgt.GetWuXing(),
                    lhsMutators:            tgt.GetMutators(),
                    rhsMutators:            null);
                d.State = MergeDetails.MergeState.Success;
                return;
            }
            
            // leYuanYing
            d.MergeTarget = new AssignMergeTarget(
                mergeType:              "命石",
                resultEntry:            tgt.GetEntry(),
                resultJingJie:          JingJie.HuaShen,
                resultWuXing:           tgt.GetWuXing(),
                lhsMutators:            tgt.GetMutators(),
                rhsMutators:            null);
            d.State = MergeDetails.MergeState.Success;
        });

    public static readonly MergeRule BuTianDanMergeRule = new(
        name:                       "补天丹",
        order:                      -1,
        processMerge:               d =>
        {
            RunSkill lhs = d.Lhs;
            RunSkill rhs = d.Rhs;
            RunSkill src = d.Src;
            RunSkill tgt = d.Tgt;
            
            // isMutator
            // Both BuTianDan
            // FanXu - FanXu
            // FanXu - LEHuaShen
            // HuaShen - GeHuaShen
            // HuaShen - LEYuanYing

            bool isMutator = tgt.GetEntry().IsMutator;
            if (isMutator)
            {
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "补天丹",
                    errorMessage:           "补天丹无法影响墨染");
                d.State = MergeDetails.MergeState.Cancel;
                return;
            }

            bool bothBuTianDan = src.GetEntry() == Encyclopedia.SkillCategory.FromName("补天丹") &&
                                 tgt.GetEntry() == Encyclopedia.SkillCategory.FromName("补天丹");
            if (bothBuTianDan)
            {
                d.State = MergeDetails.MergeState.Continue;
                return;
            }

            bool fanXuFanXu = src.GetJingJie() == JingJie.FanXu &&
                              tgt.GetJingJie() == JingJie.FanXu;
            if (fanXuFanXu)
            {
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "补天丹",
                    errorMessage:           "返虚卡牌已经无法提升境界了");
                d.State = MergeDetails.MergeState.Cancel;
                return;
            }

            bool fanXuLEHuaShen = src.GetJingJie() == JingJie.FanXu &&
                                  tgt.GetJingJie() <= JingJie.HuaShen;
            if (fanXuLEHuaShen)
            {
                d.MergeTarget = new AssignMergeTarget(
                    mergeType:              "补天丹",
                    resultEntry:            tgt.GetEntry(),
                    resultJingJie:          JingJie.FanXu,
                    resultWuXing:           tgt.GetWuXing(),
                    lhsMutators:            tgt.GetMutators(),
                    rhsMutators:            null);
                d.State = MergeDetails.MergeState.Success;
            }

            bool huaShenGEHuaShen = src.GetJingJie() == JingJie.HuaShen &&
                                    tgt.GetJingJie() >= JingJie.HuaShen;
            if (huaShenGEHuaShen)
            {
                d.MergeTarget = new InvalidMergeTarget(
                    mergeType:              "补天丹",
                    errorMessage:           "补天丹只可作用于更低境界的卡牌");
                d.State = MergeDetails.MergeState.Cancel;
                return;
            }
            
            // huaShen - LEYuanYing

            d.MergeTarget = new AssignMergeTarget(
                mergeType:              "补天丹",
                resultEntry:            tgt.GetEntry(),
                resultJingJie:          JingJie.HuaShen,
                resultWuXing:           tgt.GetWuXing(),
                lhsMutators:            tgt.GetMutators(),
                rhsMutators:            null);
            d.State = MergeDetails.MergeState.Success;
        });
    
    public static readonly MergeRule ZhanDuanMergeRule = new(
        name:                       "斩断",
        order:                      -100,
        processMerge:               d =>
        {
            d.AddSideEffect(() =>
            {
                RunManager.Instance.Environment.DepopulateFromSkillMountainProcedure(d.Tgt.GetEntry());
            });
            d.State = MergeDetails.MergeState.Continue;
        });
    
    public static readonly MergeRule TianJiMergeRule = new(
        name:                       "天机",
        order:                      -100,
        processMerge:               d =>
        {
            d.ResultJingJieIsBaseJingJie = true;
            d.State = MergeDetails.MergeState.Continue;
        });
}
