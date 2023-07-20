using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class FormationCategory : Category<FormationGroupEntry>
{
    public FormationCategory()
    {
        AddRange(new List<FormationGroupEntry>()
        {
            new("一业常置阵", 1, formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有且只有1种五行，不少于9张", "战斗开始时，使用前两位的卡",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 9;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        int countEQ0 = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] == 0);
                        return countGErequirement == 1 && countEQ0 == 4;
                    },
                    gain: async (formation, owner) =>
                    {
                        await owner._waiGongList[0].ExecuteWithoutTween(owner);
                        await owner._waiGongList[1].ExecuteWithoutTween(owner);
                    }),
                new FormationEntry(JingJie.YuanYing, "有且只有1种五行，不少于7张", "战斗开始时，使用第一位的卡",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 7;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        int countEQ0 = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] == 0);
                        return countGErequirement == 1 && countEQ0 == 4;
                    },
                    gain: async (formation, owner) =>
                    {
                        await owner._waiGongList[0].ExecuteWithoutTween(owner);
                    }),
            }),

            new("二律无相阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有两个五行，都不少于6张", "主动消耗非灵气Buff时，返还2点",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 6;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        return countGErequirement == 2;
                    }),
                new FormationEntry(JingJie.YuanYing, "有两个五行，都不少于5张", "主动消耗非灵气Buff时，返还1点",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 5;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        return countGErequirement == 2;
                    }),
            }),

            new("三才流转阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有三个五行，都不少于4张", "X计数+1",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 4;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        return countGErequirement == 3;
                    }),
                new FormationEntry(JingJie.YuanYing, "有三个五行，都不少于3张", "X计数+2",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 3;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        return countGErequirement == 3;
                    }),
            }),

            new("四元禁法阵", -3, formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有四个五行，都不少于3张", "双方所有阵法失效",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 3;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        return countGErequirement == 4;
                    },
                    anyFormationAdd: async (formation, owner, d) =>
                    {
                        if (d._formation.GetName() != "四元禁法阵")
                            d.Cancel = true;
                        return d;
                    }),
            }),

            new("颠倒五行阵", -2, formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有五种不同五行，都不少于2张", "拥有对方的所有阵法，战斗开始时，空置位将复制对方同位置的卡，所有条件算作已激活",
                    canActivate: (entity, args) =>
                    {
                        int requirement = 2;
                        int countGErequirement =
                            WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                        return countGErequirement == 5;
                    }),
            }),

            new("六爻化劫阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "无二动牌，角色境界不低于化神", "第二回合开始时，双方重置生命上限，回100%血",
                    canActivate: (entity, args) =>
                    {
                        return args.SwiftCount == 0 && entity.GetJingJie() >= JingJie.HuaShen;
                    }),
                new FormationEntry(JingJie.YuanYing, "无二动牌，角色境界不低于元婴", "第二回合开始时，双方重置生命上限，回30%血",
                    canActivate: (entity, args) =>
                    {
                        return args.SwiftCount == 0 && entity.GetJingJie() >= JingJie.YuanYing;
                    }),
            }),

            new("七曜移星阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "有至少8张带有二动的牌", "战斗开始时，对方遭受3跳回合",
                    canActivate: (entity, args) =>
                    {
                        return args.SwiftCount >= 8;
                    }),
                new FormationEntry(JingJie.YuanYing, "有至少6张带有二动的牌", "战斗开始时，对方遭受2跳回合",
                    canActivate: (entity, args) =>
                    {
                        return args.SwiftCount >= 6;
                    }),
                new FormationEntry(JingJie.JinDan, "有至少4张带有二动的牌", "战斗开始时，对方遭受1跳回合",
                    canActivate: (entity, args) =>
                    {
                        return args.SwiftCount >= 4;
                    }),
            }),

            new("八卦奇门阵", -1, formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "无消耗牌，角色境界不低于化神", "对方使用消耗牌后，自己也使用2次",
                    canActivate: (entity, args) =>
                    {
                        return args.ConsumeCount == 0 && entity.GetJingJie() >= JingJie.HuaShen;
                    }),
                new FormationEntry(JingJie.YuanYing, "无消耗牌，角色境界不低于元婴", "对方使用消耗牌后，自己也使用1次",
                    canActivate: (entity, args) =>
                    {
                        return args.ConsumeCount == 0 && entity.GetJingJie() >= JingJie.YuanYing;
                    }),
            }),

            new("九宫迷踪阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "不少于9张非攻击牌", "护甲损耗变成0%，战斗开始护甲+30",
                    canActivate: (entity, args) =>
                    {
                        return args.NonAttackCount >= 9;
                    }),
                new FormationEntry(JingJie.YuanYing, "不少于7张非攻击牌", "护甲损耗变成0%",
                    canActivate: (entity, args) =>
                    {
                        return args.NonAttackCount >= 7;
                    }),
                new FormationEntry(JingJie.JinDan, "不少于5张非攻击牌", "护甲损耗变成25%",
                    canActivate: (entity, args) =>
                    {
                        return args.NonAttackCount >= 5;
                    }),
            }),

            new("千界聚灵阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "费用消耗超过20", "战斗开始时，灵气+7",
                    canActivate: (entity, args) =>
                    {
                        return args.TotalCostCount >= 20;
                    }),
                new FormationEntry(JingJie.YuanYing, "费用消耗超过16", "战斗开始时，灵气+5",
                    canActivate: (entity, args) =>
                    {
                        return args.TotalCostCount >= 16;
                    }),
                new FormationEntry(JingJie.JinDan, "费用消耗超过12", "战斗开始时，灵气+3",
                    canActivate: (entity, args) =>
                    {
                        return args.TotalCostCount >= 12;
                    }),
            }),

            new("万剑归宗阵", formationEntries: new[]
            {
                new FormationEntry(JingJie.HuaShen, "连续7张攻击牌", "战斗开始时，力量+5",
                    canActivate: (entity, args) =>
                    {
                        return args.ConsecutiveAttackCount >= 7;
                    }),
                new FormationEntry(JingJie.YuanYing, "连续6张攻击牌", "战斗开始时，力量+4",
                    canActivate: (entity, args) =>
                    {
                        return args.ConsecutiveAttackCount >= 6;
                    }),
                new FormationEntry(JingJie.JinDan, "连续5张攻击牌", "战斗开始时，力量+3",
                    canActivate: (entity, args) =>
                    {
                        return args.ConsecutiveAttackCount >= 5;
                    }),
            }),
        });
    }
}
