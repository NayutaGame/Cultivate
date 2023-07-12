using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class FormationCategory : Category<FormationEntry>
{
    public FormationCategory()
    {
        List = new()
        {
            new FormationEntry("一业常置阵", new[]
            {
                new SubFormationEntry(JingJie.HuaShen, "有且只有1种五行，不少于9张", "战斗开始时，使用前两位的卡", (entity, args) =>
                {
                    int requirement = 9;
                    int countGErequirement = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                    int countEQ0 = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] == 0);
                    return countGErequirement == 1 && countEQ0 == 4;
                }),
                new SubFormationEntry(JingJie.YuanYing, "有且只有1种五行，不少于7张", "战斗开始时，使用第一位的卡", (entity, args) =>
                {
                    int requirement = 7;
                    int countGErequirement = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                    int countEQ0 = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] == 0);
                    return countGErequirement == 1 && countEQ0 == 4;
                }),
            }),

            new FormationEntry("二律无相阵", new[]
            {
                new SubFormationEntry(JingJie.HuaShen, "有两个五行，都不少于6张", "主动消耗非灵气Buff时，返还2点", (entity, args) =>
                {
                    int requirement = 6;
                    int countGErequirement = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                    return countGErequirement == 2;
                }),
                new SubFormationEntry(JingJie.YuanYing, "有两个五行，都不少于5张", "主动消耗非灵气Buff时，返还1点", (entity, args) =>
                {
                    int requirement = 5;
                    int countGErequirement = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                    return countGErequirement == 2;
                }),
            }),

            new FormationEntry("三才流转阵", new[]
            {
                new SubFormationEntry(JingJie.HuaShen, "有三个五行，都不少于4张", "X计数+1", (entity, args) =>
                {
                    int requirement = 4;
                    int countGErequirement = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                    return countGErequirement == 3;
                }),
                new SubFormationEntry(JingJie.YuanYing, "有三个五行，都不少于3张", "X计数+2", (entity, args) =>
                {
                    int requirement = 3;
                    int countGErequirement = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                    return countGErequirement == 3;
                }),
            }),

            new FormationEntry("四元禁法阵", new[]
            {
                new SubFormationEntry(JingJie.HuaShen, "有四个五行，都不少于3张", "双方所有阵法失效", (entity, args) =>
                {
                    int requirement = 3;
                    int countGErequirement = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                    return countGErequirement == 4;
                }),
            }),

            new FormationEntry("颠倒五行阵", new[]
            {
                new SubFormationEntry(JingJie.HuaShen, "有五种不同五行，都不少于2张", "拥有对方的所有阵法，战斗开始时，空置位将复制对方同位置的卡，所有条件算作已激活", (entity, args) =>
                {
                    int requirement = 2;
                    int countGErequirement = WuXing.Traversal.Count(wuXing => args.WuXingCounts[wuXing] >= requirement);
                    return countGErequirement == 5;
                }),
            }),

            new FormationEntry("六爻化劫阵", new[]
            {
                new SubFormationEntry(JingJie.HuaShen, "无二动牌，角色境界不低于化神", "第二回合开始时，双方重置生命上限，回100%血", (entity, args) =>
                {
                    return args.SwiftCount == 0 && entity.GetJingJie() >= JingJie.HuaShen;
                }),
                new SubFormationEntry(JingJie.YuanYing, "无二动牌，角色境界不低于元婴", "第二回合开始时，双方重置生命上限，回30%血", (entity, args) =>
                {
                    return args.SwiftCount == 0 && entity.GetJingJie() >= JingJie.YuanYing;
                }),
            }),

            new FormationEntry("七曜移星阵", new[]
            {
                new SubFormationEntry(JingJie.HuaShen, "有至少8张带有二动的牌", "战斗开始时，对方遭受3跳回合", (entity, args) =>
                {
                    return args.SwiftCount >= 8;
                }),
                new SubFormationEntry(JingJie.YuanYing, "有至少6张带有二动的牌", "战斗开始时，对方遭受2跳回合", (entity, args) =>
                {
                    return args.SwiftCount >= 6;
                }),
                new SubFormationEntry(JingJie.JinDan, "有至少4张带有二动的牌", "战斗开始时，对方遭受1跳回合", (entity, args) =>
                {
                    return args.SwiftCount >= 4;
                }),
            }),

            new FormationEntry("八卦奇门阵", new[]
            {
                new SubFormationEntry(JingJie.HuaShen, "无消耗牌，角色境界不低于化神", "对方使用消耗牌后，自己也使用2次", (entity, args) =>
                {
                    return args.ConsumeCount == 0 && entity.GetJingJie() >= JingJie.HuaShen;
                }),
                new SubFormationEntry(JingJie.YuanYing, "无消耗牌，角色境界不低于元婴", "对方使用消耗牌后，自己也使用1次", (entity, args) =>
                {
                    return args.ConsumeCount == 0 && entity.GetJingJie() >= JingJie.YuanYing;
                }),
            }),

            new FormationEntry("九宫迷踪阵", new[]
            {
                new SubFormationEntry(JingJie.HuaShen, "不少于9张非攻击牌", "护甲损耗变成0%，战斗开始护甲+30", (entity, args) =>
                {
                    return args.NonAttackCount >= 9;
                }),
                new SubFormationEntry(JingJie.YuanYing, "不少于7张非攻击牌", "护甲损耗变成0%", (entity, args) =>
                {
                    return args.NonAttackCount >= 7;
                }),
                new SubFormationEntry(JingJie.JinDan, "不少于5张非攻击牌", "护甲损耗变成25%", (entity, args) =>
                {
                    return args.NonAttackCount >= 5;
                }),
            }),

            new FormationEntry("千界聚灵阵", new[]
            {
                new SubFormationEntry(JingJie.HuaShen, "费用消耗超过20", "战斗开始时，灵气+7", (entity, args) =>
                {
                    return args.TotalCostCount >= 20;
                }),
                new SubFormationEntry(JingJie.YuanYing, "费用消耗超过16", "战斗开始时，灵气+5", (entity, args) =>
                {
                    return args.TotalCostCount >= 16;
                }),
                new SubFormationEntry(JingJie.JinDan, "费用消耗超过12", "战斗开始时，灵气+3", (entity, args) =>
                {
                    return args.TotalCostCount >= 12;
                }),
            }),

            new FormationEntry("万剑归宗阵", new[]
            {
                new SubFormationEntry(JingJie.HuaShen, "连续7张攻击牌", "战斗开始时，力量+5", (entity, args) =>
                {
                    return args.ConsecutiveAttackCount >= 7;
                }),
                new SubFormationEntry(JingJie.YuanYing, "连续6张攻击牌", "战斗开始时，力量+4", (entity, args) =>
                {
                    return args.ConsecutiveAttackCount >= 6;
                }),
                new SubFormationEntry(JingJie.JinDan, "连续5张攻击牌", "战斗开始时，力量+3", (entity, args) =>
                {
                    return args.ConsecutiveAttackCount >= 5;
                }),
            }),
        };
    }
}
