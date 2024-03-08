
using System.Collections.Generic;
using UnityEngine;
using CLLibrary;
using Range = CLLibrary.Range;

public class SkillCategory : Category<SkillEntry>
{
    public SkillCategory()
    {
        AddRange(new List<SkillEntry>()
        {
            new(name:                       "不存在的技能",
                wuXing:                     null,
                jingJieRange:               JingJie.LianQiOnly,
                description:                "不存在的技能",
                withinPool:                 false),

            new(name:                       "聚气术",
                wuXing:                     null,
                jingJieRange:               JingJie.LianQiOnly,
                skillTypeComposite:         null,
                description:                new SkillDescription((j, dj, indicator) =>
                    "灵气+1"),
                withinPool:                 false,
                execute: async (entity, skill, recursive) =>
                {
                    await entity.GainBuffProcedure("灵气");
                    return null;
                }),

            new(name:                       "信风",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.LianQi2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"护甲+{2 + dj}\n" +
                    $"初次：锋锐+{3 + 2 * dj}".ApplyCond(indicator)),
                withinPool:                 true,
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(2 + skill.Dj);
                    bool cond = skill.IsFirstTime || await caster.IsFocused();
                    if (cond)
                        await caster.GainBuffProcedure("锋锐", 3 + 2 * skill.Dj);
                    
                    return cond.ToIndicator();
                }),

            new(name:                       "乘风",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.LianQi2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{5 + dj}攻\n" +
                    $"若有锋锐：{3 + dj}攻".ApplyCond(indicator)),
                skillTypeComposite:         SkillType.Attack,
                execute:                    async (caster, skill, recursive) =>
                {
                    bool cond = caster.GetStackOfBuff("锋锐") > 0 || await caster.IsFocused();
                    int add = cond ? 3 + skill.Dj : 0;
                    await caster.AttackProcedure(5 + skill.Dj + add, wuXing: skill.Entry.WuXing);

                    return cond.ToIndicator();
                }),

            new(name:                       "金刃",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.LianQi2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{3 + dj}攻\n" +
                    $"施加{3 + dj}减甲"),
                skillTypeComposite:         SkillType.Attack,
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.RemoveArmorProcedure(3 + skill.Dj);
                    return null;
                }),

            new(name:                       "寻猎",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.LianQi2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{2 + dj}攻\n" +
                    $"击伤：施加{5 + 2 * dj}减甲".ApplyCond(indicator)),
                skillTypeComposite:         SkillType.Attack,
                execute:                    async (caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(2 + skill.Dj,
                        damaged: async d =>
                        {
                            await caster.RemoveArmorProcedure(5 + 2 * skill.Dj);
                            cond = true;
                        },
                        wuXing: skill.Entry.WuXing);
                    return cond.ToIndicator();
                }),

            new(name:                       "掠影",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"奇偶：" +
                    $"{5 + 2 * dj}攻".ApplyOdd(indicator) +
                    $"/" +
                    $"护甲+{5 + 2 * dj}".ApplyEven(indicator)),
                skillTypeComposite:         SkillType.Attack,
                execute:                    async (caster, skill, recursive) =>
                {
                    int value = 5 + 2 * skill.Dj;
                    bool odd = skill.IsOdd || await caster.IsFocused();
                    if (odd)
                        await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);
                    bool even = skill.IsEven || await caster.IsFocused();
                    if (even)
                        await caster.GainArmorProcedure(value);
                    return Style.IndicatorFromOddEven(odd, even);
                }),

            new(name:                       "盘旋",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"护甲+{4 + 2 * dj}\n施加{4 + 2 * dj}减甲"),
                skillTypeComposite:         null,
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(4 + 2 * skill.Dj);
                    await caster.RemoveArmorProcedure(4 + 2 * skill.Dj);
                    return null;
                }),

            new(name:                       "灵动",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{6 + 2 * dj}攻\n" +
                    $"敌方有减甲：多1次".ApplyStyle(indicator, "cond")),
                execute:                    async (caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().Armor < 0 || await caster.IsFocused();
                    int times = cond ? 2 : 1;
                    await caster.AttackProcedure(6 + 2 * skill.Dj, times: times, wuXing: skill.Entry.WuXing);
                    return cond.ToIndicator();
                }),

            new(name:                       "飞絮",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         null,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"奇偶：" +
                    $"施加{8 + 2 * dj}减甲".ApplyOdd(indicator) +
                    $"/" +
                    $"锋锐+{1 + dj}".ApplyEven(indicator)),
                execute:                    async (caster, skill, recursive) =>
                {
                    bool odd = skill.IsOdd || await caster.IsFocused();
                    if (odd)
                        await caster.RemoveArmorProcedure(8 + 2 * skill.Dj);
                    bool even = skill.IsEven || await caster.IsFocused(); 
                    if (even)
                        await caster.GainBuffProcedure("锋锐", 1 + skill.Dj);
                    return Style.IndicatorFromOddEven(odd, even);
                }),

            new(name:                       "诸行无常",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                manaCostEvaluator:          3,
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n造成伤害时：\n施加伤害值的减甲\n最多{3 + 2 * dj}"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("诸行无常", 3 + 2 * skill.Dj);
                    return null;
                }),

            new(name:                       "讯风",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{3 + dj}攻\n" +
                    $"击伤：锋锐+{3 + dj}".ApplyCond(indicator)),
                execute:                    async (caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(3 + skill.Dj,
                        damaged: async d =>
                        {
                            await caster.GainBuffProcedure("锋锐", 3 + skill.Dj);
                            cond = true;
                        },
                        wuXing: skill.Entry.WuXing);
                    return cond.ToIndicator();
                }),

            new(name:                       "刺穴",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"灵气+{6 + 2 * dj}\n遭受2滞气"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 6 + 2 * skill.Dj);
                    await caster.GainBuffProcedure("滞气", 2);
                    return null;
                }),

            new(name:                       "两仪",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.JinDan2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n获得护甲时/施加减甲时：额外+{3 + dj}"),
                skillTypeComposite:         SkillType.XiaoHao,
                execute:                    async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("两仪", 3 + skill.Dj);
                    return null;
                }),

            new(name:                       "敛息",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                manaCostEvaluator:          2,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"下{1 + dj}次造成伤害转减甲"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("敛息", 1 + skill.Dj);
                    return null;
                }),

            new(name:                       "凝水",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                channelTimeEvaluator:       new ChannelTimeEvaluator((j, dj, jiaShi) => 2 - dj),
                skillTypeComposite:         SkillType.LingQi | SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n击伤时：灵气+1"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("凝水");
                    return null;
                }),

            new(name:                       "袖里乾坤",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                channelTimeEvaluator:       1,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"暴击+{1 + dj}\n消耗1" +
                    $"柔韧".ApplyStyle(indicator, "0") +
                    $"/" +
                    $"锋锐".ApplyStyle(indicator, "1") +
                    $"：暴击+1"),
                execute:                    async (caster, skill, recursive) =>
                {
                    int stack = 1 + skill.Dj;

                    bool cond0 = await caster.TryConsumeProcedure("柔韧") || await caster.IsFocused();
                    if (cond0)
                        stack++;

                    bool cond1 = await caster.TryConsumeProcedure("锋锐") || await caster.IsFocused();
                    if (cond1)
                        stack++;

                    await caster.GainBuffProcedure("暴击", stack);
                    return Style.IndicatorFromBools(cond0, cond1);
                }),

            new(name:                       "流云",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                manaCostEvaluator:          new ManaCostEvaluator((j, dj, jiaShi) => 2 - 2 * dj),
                skillTypeComposite:         SkillType.Attack | SkillType.ErDong,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"2攻x3\n" +
                    $"击伤：二动".ApplyCond(indicator)),
                execute:                    async (caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(2,
                        times: 3,
                        wuXing: skill.Entry.WuXing,
                        damaged: async d =>
                        {
                            caster.Swift = true;
                            cond = true;
                        });
                    return cond.ToIndicator();
                }),

            new(name:                       "无妄",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{3 + 3 * dj}攻x2\n" +
                    $"敌方有减甲：暴击".ApplyCond(indicator)),
                execute:                    async (caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().Armor < 0 || await caster.IsFocused();
                    await caster.AttackProcedure(3 + 3 * skill.Dj, crit: cond, wuXing: skill.Entry.WuXing);
                    cond = cond || caster.Opponent().Armor < 0 || await caster.IsFocused();
                    await caster.AttackProcedure(3 + 3 * skill.Dj, crit: cond, wuXing: skill.Entry.WuXing);
                    return cond.ToIndicator();
                }),

            new(name:                       "天地同寿",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"双方遭受{15 + 5 * dj}减甲"),
                execute:                    async (caster, skill, recursive) =>
                {
                    int value = 15 + 5 * skill.Dj;
                    await caster.LoseArmorProcedure(value);
                    await caster.RemoveArmorProcedure(value);
                    return null;
                }),

            new(name:                       "山风",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗所有护甲，每{6 - dj}，锋锐+1\n金流转"),
                execute:                    async (caster, skill, recursive) =>
                {
                    int stack = caster.Armor / (skill.Dj - 1);
                    await caster.LoseArmorProcedure((skill.Dj - 1) * stack);
                    await caster.GainBuffProcedure("锋锐", stack);

                    await caster.CycleProcedure(WuXing.Jin);
                    return null;
                }),

            new(name:                       "追命",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"每1柔韧，施加2减甲"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.RemoveArmorProcedure(2 * caster.GetStackOfBuff("柔韧"));
                    return null;
                }),

            new(name:                       "千里神行符",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao | SkillType.ErDong,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"奇偶：" +
                    $"消耗".ApplyOdd(indicator) +
                    $"/" +
                    $"二动".ApplyEven(indicator) +
                    $"\n灵气+4"),
                execute:                    async (caster, skill, recursive) =>
                {
                    bool odd = skill.IsOdd || await caster.IsFocused();
                    if (odd)
                        await skill.ExhaustProcedure();
                    bool even = skill.IsEven || await caster.IsFocused();
                    if (even)
                        caster.Swift = true;

                    await caster.GainBuffProcedure("灵气", 4);
                    return Style.IndicatorFromOddEven(odd, even);
                }),

            new(name:                       "齐物论",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n奇偶同时激活两个效果"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("齐物论");
                    return null;
                }),

            new(name:                       "人间无戈",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n20锋锐觉醒：死亡不会导致Stage结算"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    return null;
                }),

            new(name:                       "一闪罗刹",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"1攻\n奇偶：" +
                    $"暴击+2".ApplyOdd(indicator) +
                    $"/" +
                    $"暴击释放".ApplyEven(indicator)),
                execute:                    async (caster, skill, recursive) =>
                {
                    bool odd = skill.IsOdd || await caster.IsFocused();
                    if (odd)
                        await caster.GainBuffProcedure("暴击", 2);
                    bool even = skill.IsEven || await caster.IsFocused();
                    if (even)
                    {
                        int critStack = caster.GetStackOfBuff("暴击");
                        await caster.TryConsumeProcedure("暴击", critStack);
                        await caster.AttackProcedure(1, skill.Entry.WuXing, damaged: async d => d.Value *= 1 + critStack);
                    }
                    else
                        await caster.AttackProcedure(1, skill.Entry.WuXing);

                    return Style.IndicatorFromOddEven(odd, even);
                }),

            new(name:                       "恋花",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{4 + 2 * dj}攻 吸血"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(4 + 2 * skill.Dj, lifeSteal: true, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(name:                       "冰弹",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.LianQi2HuaShen,
                manaCostEvaluator:          2,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{3 + 2 * dj}攻\n格挡+1"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.GainBuffProcedure("格挡");
                    return null;
                }),

            new(name:                       "满招损",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{5 + dj}攻\n" +
                    $"对方有灵气：{3 + dj}攻".ApplyCond(indicator)),
                execute:                    async (caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().GetStackOfBuff("灵气") > 0 || await caster.IsFocused();
                    int add = cond ? 3 + skill.Dj : 0;
                    await caster.AttackProcedure(5 + skill.Dj + add, wuXing: skill.Entry.WuXing);
                    return cond.ToIndicator();
                }),

            new(name:                       "清泉",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"灵气+{2 + dj}"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 2 + skill.Dj);
                    return null;
                }),

            new(name:                       "归意",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{10 + 2 * dj}攻\n" +
                    $"终结：吸血".ApplyCond(indicator)),
                execute:                    async (caster, skill, recursive) =>
                {
                    bool cond = skill.IsEnd || await caster.IsFocused();
                    await caster.AttackProcedure(10 + 2 * skill.Dj, lifeSteal: cond,
                        wuXing: skill.Entry.WuXing);
                    return cond.ToIndicator();
                }),

            new(name:                       "吐纳",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"灵气+{3 + dj}\n生命上限+{8 + 4 * dj}"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 3 + skill.Dj);
                    // await Procedure
                    caster.MaxHp += 8 + 4 * skill.Dj;
                    return null;
                }),

            new(name:                       "冰雨",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{10 + 2 * dj}攻\n击伤：格挡+1"),
                execute:                    async (caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(10 + 2 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: async d =>
                        {
                            cond = true;
                            await caster.GainBuffProcedure("格挡");
                        });
                    return cond.ToIndicator();
                }),

            new(name:                       "勤能补拙",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"护甲+{10 + 4 * dj}\n初次：遭受1跳回合"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(10 + 4 * skill.Dj);
                    bool cond = !skill.IsFirstTime || await caster.IsFocused();
                    if (!cond)
                        await caster.GainBuffProcedure("跳回合");
                    return null;
                }),

            new(name:                       "秋水",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.JinDan2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{12 + 2 * dj}攻\n" +
                    $"消耗1锋锐：吸血".ApplyStyle(indicator, "0") +
                    $"\n" +
                    $"消耗1灵气：翻倍".ApplyStyle(indicator, "1")),
                execute:                    async (caster, skill, recursive) =>
                {
                    bool cond0 = await caster.TryConsumeProcedure("锋锐") || await caster.IsFocused();
                    bool cond1 = await caster.TryConsumeProcedure("灵气") || await caster.IsFocused();
                    
                    int d = cond1 ? 2 : 1;
                    await caster.AttackProcedure((12 + 2 * skill.Dj) * d, lifeSteal: cond0, wuXing: skill.Entry.WuXing);
                    return Style.IndicatorFromBools(cond0, cond1);
                }),

            new(name:                       "玄冰刺",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.JinDan2HuaShen,
                manaCostEvaluator:          4,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{16 + 8 * dj}攻\n每造成{8 - dj}点伤害，格挡+1\n水流转"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(16 + 8 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: d => caster.GainBuffProcedure("格挡", d.Value / (8 - skill.Dj)));
                    await caster.CycleProcedure(WuXing.Shui);
                    return null;
                }),

            new(name:                       "腾跃",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.JinDan2HuaShen,
                manaCostEvaluator:          4,
                skillTypeComposite:         SkillType.Attack | SkillType.ErDong,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{6 + 3 * dj}攻\n二动\n第1 ~ {1 + dj}次：遭受1跳卡牌"),
                execute:                    async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(6 + 3 * skill.Dj, wuXing: skill.Entry.WuXing);
                    caster.Swift = true;
                    if (skill.RunUsedTimes <= skill.Dj)
                        await caster.GainBuffProcedure("跳卡牌");
                    return null;
                }),

            new(name:                       "观棋烂柯",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                manaCostEvaluator:          new ManaCostEvaluator((j, dj, jiaShi) => 1 - dj),
                description:                new SkillDescription((j, dj, indicator) =>
                    $"施加1跳回合"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GiveBuffProcedure("跳回合");
                    return null;
                }),

            new(name:                       "激流",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                manaCostEvaluator:          new ManaCostEvaluator((j, dj, jiaShi) => 1 - dj),
                description:                new SkillDescription((j, dj, indicator) =>
                    $"生命+{5 + 5 * dj}\n下一次使用牌时二动"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.HealProcedure(5 + 5 * skill.Dj);
                    await caster.GainBuffProcedure("二动");
                    return null;
                }),

            new(name:                       "气吞山河",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"将灵气补至本局最大值+{1 + dj}"),
                execute: async (caster, skill, recursive) =>
                {
                    int space = caster.HighestManaRecord - caster.GetMana() + 1 + skill.Dj;
                    await caster.GainBuffProcedure("灵气", space);
                    return null;
                }),

            new(name:                       "幻月狂乱",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n永久吸血\n回合内未攻击：遭受1跳回合"),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("幻月狂乱");
                    return null;
                }),

            new(name:                       "一梦如是",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack | SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"1攻\n" +
                    $"击伤：消耗，生命+[累计治疗]".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(1, skill.Entry.WuXing, damaged:
                        async d =>
                        {
                            await skill.ExhaustProcedure();
                            await caster.HealProcedure(caster.HealedRecord);
                            cond = true;
                        });
                    return cond.ToIndicator();
                }),

            new(name:                       "奔腾",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.ErDong,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"二动\n消耗2灵气：三动"),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = await caster.TryConsumeProcedure("灵气", 2) || await caster.IsFocused();
                    caster.Swift = true;
                    if (cond)
                        caster.TriSwift = true;
                    return cond.ToIndicator();
                }),

            new(name:                       "摩诃钵特摩",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.LingQi | SkillType.ErDong,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"灵气+4，格挡+4\n20格挡觉醒：八动，回合结束死亡"),
                execute: async (caster, skill, recursive) =>
                {
                    return null;
                }),

            new(name:                       "若竹",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"灵气+{1 + dj}\n穿透+1"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    await caster.GainBuffProcedure("穿透", 1);
                    return null;
                }),

            new(name:                       "突刺",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{6 + 2 * dj}攻 穿透"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(6 + 2 * skill.Dj, pierce: true, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(name:                       "花舞",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"力量+{1 + (dj / 2)}\n{2 + 2 * dj}攻"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("力量", 1 + skill.Dj / 2);
                    await caster.AttackProcedure(2 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(name:                       "初桃",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"灵气+{1 + dj}\n生命+{3 + dj}"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    await caster.HealProcedure(3 + skill.Dj);
                    return null;
                }),

            new(name:                       "潜龙在渊",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"生命+{6 + 4 * dj}\n初次：闪避+1"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.HealProcedure(6 + 4 * skill.Dj);
                    bool cond = skill.IsFirstTime || await caster.IsFocused();
                    if (cond)
                        await caster.GainBuffProcedure("闪避");
                    return null;
                }),

            new(name:                       "早春",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                manaCostEvaluator:          1,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"力量+{1 + (dj / 2)}\n护甲+{6 + dj}\n初次：翻倍"),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = skill.IsFirstTime || await caster.IsFocused();
                    int mul = cond ? 2 : 1;
                    await caster.GainBuffProcedure("力量", (1 + (skill.Dj / 2)) * mul);
                    await caster.GainArmorProcedure((6 + skill.Dj) * mul);
                    return null;
                }),

            new(name:                       "身骑白马",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"第{3 + dj}+次使用：{(6 + 2 * dj) * (6 + 2 * dj)}攻 穿透"),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = !(skill.StageUsedTimes < 2 + skill.Dj);
                    if (cond)
                        await caster.AttackProcedure((6 + 2 * skill.Dj) * (6 + 2 * skill.Dj), pierce: true,
                            wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(name:                       "回马枪",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"下次受攻击时：{12 + 4 * dj}攻"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("回马枪", 12 + 4 * skill.Dj);
                    return null;
                }),

            new(name:                       "千年笋",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{15 + 3 * dj}攻\n" +
                    $"消耗1格挡：穿透".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = await caster.TryConsumeProcedure("格挡") || await caster.IsFocused();
                    await caster.AttackProcedure(15 + 3 * skill.Dj, wuXing: skill.Entry.WuXing,
                        pierce: cond);
                    return cond.ToIndicator();
                }),

            new(name:                       "见龙在田",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                manaCostEvaluator:          2,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"闪避+{1 + dj / 2}\n" +
                    $"无闪避：闪避+{1 + (dj + 1) / 2}".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = caster.GetStackOfBuff("闪避") == 0 || await caster.IsFocused();
                    int add = cond ? 1 + (skill.Dj + 1) / 2 : 0;
                    await caster.GainBuffProcedure("闪避", 1 + skill.Dj / 2 + add);
                    return cond.ToIndicator();
                }),

            new(name:                       "一虚一实",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                manaCostEvaluator:          2,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"8攻\n受到{3 + dj}倍力量影响\n未击伤：治疗等量数值"),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = false;
                    int add = caster.GetStackOfBuff("力量") * (2 + skill.Dj);
                    int value = 8 + add;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
                        undamaged: async d =>
                        {
                            cond = true;
                            await caster.HealProcedure(value);
                        });
                    return cond.ToIndicator();
                }),

            new(name:                       "瑞雪丰年",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"每1格挡，生命+2"),
                execute: async (caster, skill, recursive) =>
                {
                    int stack = caster.GetStackOfBuff("格挡");
                    await caster.HealProcedure(2 * stack);
                    return null;
                }),

            new(name:                       "盛开",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                channelTimeEvaluator:       new ChannelTimeEvaluator((j, dj, jiaShi) => 2 - dj),
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n受治疗时：力量+1"),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("盛开");
                    return null;
                }),

            new(name:                       "百花缭乱",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗所有灵气，每{5 - dj}，力量+1\n木流转"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.TransferProcedure(5 - skill.Dj, "灵气", 1, "力量", true);
                    await caster.CycleProcedure(WuXing.Mu);
                    return null;
                }),

            new(name:                       "飞龙在天",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                channelTimeEvaluator:       new ChannelTimeEvaluator((j, dj, jiaShi) => 2 - dj),
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n每轮：闪避补至1"),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("飞龙在天");
                    return null;
                }),

            new(name:                       "二重",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                channelTimeEvaluator:       new ChannelTimeEvaluator((j, dj, jiaShi) => 1 - dj),
                description:                new SkillDescription((j, dj, indicator) =>
                    $"下{1 + dj}张牌使用两次"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("二重", 1 + skill.Dj);
                    return null;
                }),

            new(name:                       "心斋",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                channelTimeEvaluator:       new ChannelTimeEvaluator((j, dj, jiaShi) => 2 - dj),
                skillTypeComposite:         SkillType.LingQi | SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n所有耗蓝-1"),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("心斋");
                    return null;
                }),

            new(name:                       "亢龙有悔",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack | SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"未消耗2灵气：消耗".ApplyCond(indicator) +
                    $"\n1攻x2 闪避+2 力量+2"),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = await caster.TryConsumeProcedure("灵气", 2) || await caster.IsFocused();
                    if (!cond)
                        await skill.ExhaustProcedure();
                    
                    await caster.GainBuffProcedure("闪避", 2);
                    await caster.GainBuffProcedure("力量", 2);
                    await caster.AttackProcedure(1, times: 2, wuXing: skill.Entry.WuXing);
                    return (!cond).ToIndicator();
                }),

            new(name:                       "回响",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.HuaShenOnly,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"使用第一张牌"),
                execute: async (caster, skill, recursive) =>
                {
                    if (!recursive)
                        return null;
                    await caster._skills[0].Execute(caster, false);
                    return null;
                }),

            new(name:                       "鹤回翔",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n反转出牌顺序"),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    if (caster.Forward)
                        await caster.GainBuffProcedure("鹤回翔");
                    else
                        await caster.TryRemoveBuff("鹤回翔");
                    return null;
                }),

            new(name:                       "通透世界",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n20力量觉醒：攻击具有穿透"),
                execute: async (caster, skill, recursive) =>
                {
                    return null;
                }),

            new(name:                       "刷新球",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.HuaShenOnly,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗所有灵气，每6，多重+1"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.TransferProcedure(6, "灵气", 1, "多重", true);
                    return null;
                }),

            new(name:                       "云袖",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{2 + dj}攻x2\n护甲+{3 + dj}"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(2 + skill.Dj, wuXing: skill.Entry.WuXing, 2);
                    await caster.GainArmorProcedure(3 + skill.Dj);
                    return null;
                }),

            new(name:                       "化焰",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.LianQi2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{4 + 2 * dj}攻\n灼烧+{1 + dj / 2}"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(4 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.GainBuffProcedure("灼烧", 1 + skill.Dj / 2);
                    return null;
                }),

            new(name:                       "吐焰",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗{2 + dj}生命\n{8 + 3 * dj}攻"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.DamageSelfProcedure(2 + skill.Dj);
                    await caster.AttackProcedure(8 + 3 * skill.Dj, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(name:                       "燃命",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗{3 + dj}生命\n{2 + 3 * dj}攻\n灵气+3"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.DamageSelfProcedure(3 + skill.Dj);
                    await caster.AttackProcedure(2 + 3 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.GainBuffProcedure("灵气", 3);
                    return null;
                }),

            new(name:                       "凤来朝",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"2攻x{3 + dj}"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(2, times: 3 + skill.Dj, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(name:                       "聚火",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n灼烧+{2 + dj}"),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("灼烧", 2 + skill.Dj);
                    return null;
                }),

            new(name:                       "一切皆苦",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                manaCostEvaluator:          new ManaCostEvaluator((j, dj, jiaShi) => 3 - dj),
                skillTypeComposite:         SkillType.XiaoHao | SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n" +
                    $"唯一灵气牌（无法使用集中）：回合开始时：灵气+1".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    bool cond = skill.NoOtherLingQi;
                    if (cond)
                        await caster.GainBuffProcedure("一切皆苦");
                    return cond.ToIndicator();
                }),

            new(name:                       "常夏",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{4 + dj}攻\n每相邻1张火，多{4 + dj}攻"),
                execute: async (caster, skill, recursive) =>
                {
                    int mul = 1;
                    mul += skill.Prev(true).Entry.WuXing == WuXing.Huo ? 1 : 0;
                    mul += skill.Next(true).Entry.WuXing == WuXing.Huo ? 1 : 0;
                    await caster.AttackProcedure((4 + skill.Dj) * mul, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(name:                       "天衣无缝",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.JinDan2HuaShen,
                manaCostEvaluator:          4,
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n" +
                    $"若无攻击牌（无法使用集中）：每回合：{6 + 2 * dj}攻".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    bool cond = skill.NoOtherAttack;
                    if (cond)
                        await caster.GainBuffProcedure("天衣无缝", 6 + 2 * skill.Dj);
                    return cond.ToIndicator();
                }),

            new(name:                       "化劲",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.JinDan2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗5生命\n灼烧+{2 + dj}\n" +
                    $"消耗1力量：翻倍".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.DamageSelfProcedure(5);
                    bool cond = await caster.TryConsumeProcedure("力量") || await caster.IsFocused();
                    int d = cond ? 2 : 1;
                    await caster.GainBuffProcedure("灼烧", (2 + skill.Dj) * d);
                    return cond.ToIndicator();
                }),

            new(name:                       "淬体",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.JinDan2HuaShen,
                manaCostEvaluator:          new ManaCostEvaluator((j, dj, jiaShi) => 4 - dj),
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n受伤时：灼烧+1"),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("淬体");
                    return null;
                }),

            new(name:                       "轰天",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n消耗{20 + 5 * dj}生命\n{30 + 10 * dj}攻"),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.DamageSelfProcedure(20 + 5 * skill.Dj);
                    await caster.AttackProcedure(30 + 10 * skill.Dj);
                    return null;
                }),

            new(name:                       "燃灯留烬",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                manaCostEvaluator:          1,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"护甲+{6 + 2 * dj}\n每1被消耗卡：多{6 + 2 * dj}"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure((caster.ExhaustedCount + 1) * (6 + 2 * skill.Dj));
                    return null;
                }),

            new(name:                       "少女空舞",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"灵气+{2 + dj}，灼烧+{2 + dj}，每5灼烧：净化1"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 2 + skill.Dj);
                    await caster.GainBuffProcedure("灼烧", 2 + skill.Dj);
                    int dispel = caster.GetStackOfBuff("灼烧") / 5;
                    await caster.DispelProcedure(dispel);
                    return null;
                }),

            new(name:                       "天女散花",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"1攻 每获得过1闪避，多攻击1次"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(1, times: caster.GainedEvadeRecord + 1, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(name:                       "凤凰涅槃",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n20灼烧激活：每轮：生命回满"),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("待激活的凤凰涅槃");
                    return null;
                }),

            new(name:                       "净天地",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"下1张非攻击卡不消耗灵气，使用之后消耗"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("净天地");
                    return null;
                }),

            new(name:                       "落石",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                manaCostEvaluator:          2,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{12 + 4 * dj}攻"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(12 + 4 * skill.Dj, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(name:                       "流沙",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{3 + dj}攻\n灵气+{1 + dj}"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + skill.Dj);
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    return null;
                }),

            new(name:                       "土墙",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"灵气+{1 + dj}\n护甲+{3 + dj}"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    await caster.GainArmorProcedure(3 + skill.Dj);
                    return null;
                }),

            new(name:                       "地龙",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{7 + 2 * dj}攻\n击伤：护甲+{7 + 2 * dj}"),
                execute: async (caster, skill, recursive) =>
                {
                    int value = 7 + 2 * skill.Dj;
                    bool cond = false;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
                        damaged: async d =>
                        {
                            cond = true;
                            await caster.GainArmorProcedure(value);
                        });
                    return cond.ToIndicator();
                }),

            new(name:                       "铁骨",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n柔韧+{1 + (1 + dj) / 2}"),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("柔韧", 1 + (1 + skill.Dj) / 2);
                    return null;
                }),

            new(name:                       "点星",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{8 + 2 * dj}攻\n" +
                    $"相邻牌都非攻击：翻倍".ApplyStyle(indicator, "0") +
                    $"\n" +
                    $"消耗1灵气：翻倍".ApplyStyle(indicator, "1")),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond0 = skill.NoAttackAdjacents || await caster.IsFocused();
                    bool cond1 = await caster.TryConsumeProcedure("灵气") || await caster.IsFocused();
                    int bitShift = 0;
                    bitShift += cond0 ? 1 : 0;
                    bitShift += cond1 ? 1 : 0;
                    await caster.AttackProcedure((8 + 2 * skill.Dj) << bitShift);
                    return Style.IndicatorFromBools(cond0, cond1);
                }),

            new(name:                       "一莲托生",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                channelTimeEvaluator:       new ChannelTimeEvaluator((j, dj, jiaShi) => 2 - dj),
                skillTypeComposite:         SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"灵气+2\n" +
                    $"生命小于一半：翻倍".ApplyStyle(indicator, "0") +
                    $"\n" +
                    $"架势：翻倍".ApplyStyle(indicator, "1")),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond0 = (caster.MaxHp / 2 >= caster.Hp) || await caster.IsFocused();
                    bool cond1 = await caster.ToggleJiaShiProcedure() || await caster.IsFocused();
                    int bitShift = (cond0 ? 1 : 0) + (cond1 ? 1 : 0);
                    await caster.GainBuffProcedure("灵气", 2 << bitShift);
                    return Style.IndicatorFromBools(cond0, cond1);
                }),

            new(name:                       "巩固",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"灵气+{3 + dj}\n每2灵气，护甲+1"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 3 + skill.Dj);
                    await caster.GainArmorProcedure(caster.GetMana() / 2);
                    return null;
                }),

            new(name:                       "软剑",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                manaCostEvaluator:          1,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{9 + 4 * dj}攻\n击伤：施加1缠绕"),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(9 + 4 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: async d =>
                        {
                            cond = true;
                            await caster.GiveBuffProcedure("缠绕");
                        });
                    return cond.ToIndicator();
                }),

            new(name:                       "一力降十会",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                manaCostEvaluator:          3,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{6 + dj}攻\n" +
                    $"唯一攻击牌（无法使用集中）：{6 + dj}倍".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = skill.NoOtherAttack;
                    int d = cond ? (6 + skill.Dj) : 1;
                    await caster.AttackProcedure((6 + skill.Dj) * d, wuXing: skill.Entry.WuXing);
                    return cond.ToIndicator();
                }),

            new(name:                       "金钟罩",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                channelTimeEvaluator:       new ChannelTimeEvaluator((j, dj, jiaShi) => jiaShi ? 0 : 1),
                description:                new SkillDescription((j, dj, indicator) =>
                    $"吟唱1\n" +
                    $"架势：无需吟唱" +
                    $"\n护甲+{20 + 6 * dj}\n" +
                    $"无护甲：翻倍".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = (caster.Armor <= 0) || await caster.IsFocused();
                    int bitShift = cond ? 1 : 0;
                    await caster.GainArmorProcedure((20 + 6 * skill.Dj) << bitShift);
                    return cond.ToIndicator();
                }),

            new(name:                       "腾云",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.LingQi | SkillType.ErDong,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"灵气+{4 + dj}\n" +
                    $"架势：二动".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 4 + skill.Dj);
                    
                    bool cond = await caster.ToggleJiaShiProcedure();
                    caster.Swift |= cond;
                    return cond.ToIndicator();
                }),

            new(name:                       "收刀",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"下回合护甲+{8 + 4 * dj}\n" +
                    $"架势：翻倍".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = await caster.ToggleJiaShiProcedure();
                    int bitShift = cond ? 1 : 0;
                    await caster.GainBuffProcedure("延迟护甲", (8 + 4 * skill.Dj) << bitShift);
                    return cond.ToIndicator();
                }),

            new(name:                       "摩利支天咒",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                channelTimeEvaluator:       new ChannelTimeEvaluator((j, dj, jiaShi) => 1 + dj),
                description:                new SkillDescription((j, dj, indicator) =>
                    $"吟唱{1 + dj}\n柔韧+{(2 + dj) * (2 + dj)}"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("柔韧", (2 + skill.Dj) * (2 + skill.Dj));
                    return null;
                }),

            new(name:                       "抱元守一",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"柔韧+{3 + dj}\n遭受{4 + dj}内伤"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("柔韧", 3 + skill.Dj);
                    await caster.GainBuffProcedure("内伤", 4 + skill.Dj);
                    return null;
                }),

            new(name:                       "骑象",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                channelTimeEvaluator:       new ChannelTimeEvaluator((j, dj, jiaShi) => jiaShi ? 0 : 2),
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"吟唱2\n" +
                    $"架势：无需吟唱" +
                    $"\n{22 + 8 * dj}攻\n" +
                    $"击伤：护甲+击伤值".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(22 + 8 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: async d =>
                        {
                            cond = true;
                            caster.GainArmorProcedure(d.Value);
                        });
                    return cond.ToIndicator();
                }),

            new(name:                       "金刚",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                manaCostEvaluator:          new ManaCostEvaluator((j, dj, jiaShi) => 3 - 2 * dj),
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"1攻 每有一点护甲多1攻"),
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(1 + Mathf.Max(0, caster.Armor), wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(name:                       "铁布衫",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"1次，受伤时：护甲+[受伤值]，" +
                    $"架势：2次".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = await caster.ToggleJiaShiProcedure();
                    int stack = cond ? 2 : 1;
                    await caster.GainBuffProcedure("铁布衫", stack);
                    return cond.ToIndicator();
                }),

            new(name:                       "拔刀",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"{10 + 5 * dj}攻\n" +
                    $"架势：翻倍".ApplyCond(indicator)),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond = await caster.ToggleJiaShiProcedure();
                    int bitShift = cond ? 1 : 0;
                    await caster.AttackProcedure((10 + 5 * skill.Dj) << bitShift, wuXing: skill.Entry.WuXing);
                    return cond.ToIndicator();
                }),

            new(name:                       "天人合一",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                manaCostEvaluator:          new ManaCostEvaluator((j, dj, jiaShi) => 5 - 2 * dj),
                skillTypeComposite:         SkillType.XiaoHao,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"消耗\n激活所有架势"),
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("天人合一");
                    return null;
                }),

            new(name:                       "窑土",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"每1灼烧，护甲+2"),
                execute: async (caster, skill, recursive) =>
                {
                    int stack = caster.GetStackOfBuff("灼烧");
                    await caster.GainArmorProcedure(2 * stack);
                    return null;
                }),

            new(name:                       "那由他",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.HuaShenOnly,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"20柔韧觉醒：没有耗蓝阶段，Step阶段无法受影响，所有Buff层数不会再变化"),
                execute: async (caster, skill, recursive) =>
                {
                    return null;
                }),

            new(name:                       "一诺五岳",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"1攻 " +
                    $"对方生命高".ApplyStyle(indicator, "0") +
                    $"|" +
                    $"攻击牌多".ApplyStyle(indicator, "1") +
                    $"|" +
                    $"护甲高".ApplyStyle(indicator, "2") +
                    $"|" +
                    $"灵气高".ApplyStyle(indicator, "3") +
                    $"|" +
                    $"滞气少".ApplyStyle(indicator, "4") +
                    $"|" +
                    $"缠绕少".ApplyStyle(indicator, "5") +
                    $"|" +
                    $"软弱少".ApplyStyle(indicator, "6") +
                    $"|" +
                    $"内伤少".ApplyStyle(indicator, "7") +
                    $"|" +
                    $"腐朽少".ApplyStyle(indicator, "8") +
                    $"|" +
                    $"架势".ApplyStyle(indicator, "9") +
                    $"|" +
                    $"终结".ApplyStyle(indicator, "10") +
                    $"|" +
                    $"初次".ApplyStyle(indicator, "11") +
                    $"：翻倍"),
                execute: async (caster, skill, recursive) =>
                {
                    bool cond0 = (caster.Hp < caster.Opponent().Hp) || await caster.IsFocused();
                    bool cond1 = (caster.AttackCount < caster.Opponent().AttackCount) || await caster.IsFocused();
                    bool cond2 = (caster.Armor < caster.Opponent().Armor) || await caster.IsFocused();
                    bool cond3 = (caster.GetMana() < caster.Opponent().GetMana()) || await caster.IsFocused();
                    bool cond4 = (caster.GetStackOfBuff("滞气") > caster.Opponent().GetStackOfBuff("滞气")) || await caster.IsFocused();
                    bool cond5 = (caster.GetStackOfBuff("缠绕") > caster.Opponent().GetStackOfBuff("缠绕")) || await caster.IsFocused();
                    bool cond6 = (caster.GetStackOfBuff("软弱") > caster.Opponent().GetStackOfBuff("软弱")) || await caster.IsFocused();
                    bool cond7 = (caster.GetStackOfBuff("内伤") > caster.Opponent().GetStackOfBuff("内伤")) || await caster.IsFocused();
                    bool cond8 = (caster.GetStackOfBuff("腐朽") > caster.Opponent().GetStackOfBuff("腐朽")) || await caster.IsFocused();
                    bool cond9 = await caster.ToggleJiaShiProcedure();
                    bool cond10 = skill.IsEnd || await caster.IsFocused();
                    bool cond11 = skill.IsFirstTime || await caster.IsFocused();

                    int bitShift = (cond0 ? 1 : 0) +
                                   (cond1 ? 1 : 0) +
                                   (cond2 ? 1 : 0) +
                                   (cond3 ? 1 : 0) +
                                   (cond4 ? 1 : 0) +
                                   (cond5 ? 1 : 0) +
                                   (cond6 ? 1 : 0) +
                                   (cond7 ? 1 : 0) +
                                   (cond8 ? 1 : 0) +
                                   (cond9 ? 1 : 0) +
                                   (cond10 ? 1 : 0) +
                                   (cond11 ? 1 : 0);
                    await caster.AttackProcedure(1 << bitShift, wuXing: skill.Entry.WuXing);
                    return Style.IndicatorFromBools(cond0, cond1, cond2, cond3, cond4, cond5, cond6, cond7, cond8,
                        cond9, cond10, cond11);
                }),

            #region 事件牌

            new(name:                       "一念",
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.ErDong,
                description:                new SkillDescription((j, dj, indicator) =>
                    ($"消耗{8 - dj}生命\n") + (j <= JingJie.ZhuJi ? "二动" : "三动")),
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.LoseHealthProcedure(8 - skill.Dj);
                    if (skill.GetJingJie() <= JingJie.ZhuJi)
                        caster.Swift = true;
                    else
                        caster.TriSwift = true;
                    return null;
                }),

            new(name:                       "无量劫",
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                channelTimeEvaluator:       3,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"吟唱3\n治疗{18 + dj * 6}"),
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.HealProcedure(18 + skill.Dj * 6);
                    return null;
                }),

            new(name:                       "百草集",
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                manaCostEvaluator:          3,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"如果存在锋锐，格挡，力量，闪避，灼烧，层数+{1 + dj}"),
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    BuffEntry[] buffEntries = new BuffEntry[] { "锋锐", "格挡", "力量", "闪避", "灼烧", };

                    foreach (BuffEntry buffEntry in buffEntries)
                    {
                        Buff b = caster.FindBuff(buffEntry);
                        if (b != null)
                            await caster.GainBuffProcedure(b.GetEntry(), 1 + skill.Dj);
                    }

                    return null;
                }),

            new(name:                       "遗憾",
                wuXing:                     null,
                jingJieRange:               JingJie.JinDan2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"对手失去{3 + dj}灵气"),
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.RemoveBuffProcedure("灵气", 3 + skill.Dj, false);
                    return null;
                }),

            new(name:                       "爱恋",
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"获得{2 + dj}集中"),
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("集中", 2 + skill.Dj, false);
                    return null;
                }),

            new(name:                       "射金乌",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                manaCostEvaluator:          3,
                skillTypeComposite:         SkillType.Attack,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"5攻x{4 + 2 * dj}"),
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(5, times: 4 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(name:                       "春雨",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                manaCostEvaluator:          2,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"双方生命+{20 + 5 * dj}"),
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.HealProcedure(20 + 5 * skill.Dj);
                    await caster.Opponent().HealProcedure(20 + 5 * skill.Dj);
                    return null;
                }),

            new(name:                       "枯木",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.JinDan2HuaShen,
                description:                new SkillDescription((j, dj, indicator) =>
                    $"双方遭受{5 + dj}腐朽"),
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("腐朽", 5 + skill.Dj);
                    await caster.GiveBuffProcedure("腐朽", 5 + skill.Dj);
                    return null;
                }),

            #endregion

            #region 机关牌

            // 筑基
            
            new(name:                       "醒神香", // 香
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJiOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.LingQi,
                description:                "灵气+4",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 4);
                    return null;
                }),
            
            new(name:                       "飞镖", // 刃
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJiOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                description:                "12攻",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(12);
                    return null;
                }),
            
            new(name:                       "铁匣", // 匣
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJiOnly,
                skillTypeComposite:         SkillType.SunHao,
                description:                "护甲+12",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(12);
                    return null;
                }),
            
            new(name:                       "滑索", // 轮
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJiOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.ErDong | SkillType.XiaoHao,
                description:                "三动 消耗",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    caster.TriSwift = true;
                    await skill.ExhaustProcedure();
                    return null;
                }),

            // 元婴
            
            new(name:                       "还魂香", // 香香
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.LingQi,
                description:                "灵气+8",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 8);
                    return null;
                }),
            
            new(name:                       "净魂刀", // 香刃
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.LingQi | SkillType.LingQi,
                description:                "10攻\n击伤：灵气+1，对手灵气-1",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10,
                        damaged: async d =>
                        {
                            await caster.GainBuffProcedure("灵气");
                            await caster.RemoveBuffProcedure("灵气");
                        });
                    return null;
                }),
            
            new(name:                       "防护罩", // 香匣
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao,
                description:                "护甲+8\n每有1灵气，护甲+4",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    int add = caster.GetStackOfBuff("灵气");
                    await caster.GainArmorProcedure(8 + add);
                    return null;
                }),
            
            new(name:                       "能量饮料", // 香轮
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.LingQi,
                description:                "下1次灵气减少时，加回",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气回收");
                    return null;
                }),
            
            new(name:                       "炎铳", // 刃刃
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                description:                "25攻",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(25);
                    return null;
                }),
            
            new(name:                       "机关人偶", // 刃匣
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                description:                "护甲+12\n10攻",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(12);
                    await caster.AttackProcedure(10);
                    return null;
                }),
            
            new(name:                       "铁陀螺", // 刃轮
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                description:                "2攻x6",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(2, times: 6);
                    return null;
                }),
            
            new(name:                       "防壁", // 匣匣
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao,
                description:                "护甲+20\n柔韧+2",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(20);
                    await caster.GainBuffProcedure("柔韧", 2);
                    return null;
                }),
            
            new(name:                       "不倒翁", // 匣轮
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao,
                description:                "下2次护甲减少时，加回",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("护甲回收", 2);
                    return null;
                }),
            
            new(name:                       "助推器", // 轮轮
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.ErDong,
                description:                "二动 二重",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    caster.Swift = true;
                    await caster.GainBuffProcedure("二重");
                    return null;
                }),

            // 返虚
            
            new(name:                       "反应堆", // 香香香
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                description:                "消耗\n遭受1不堪一击，永久二重+1",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("不堪一击");
                    await caster.GainBuffProcedure("永久二重");
                    return null;
                }),
            
            new(name:                       "烟花", // 香香刃
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao,
                description:                "消耗所有灵气，每1，力量+1",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    int stack = caster.GetStackOfBuff("灵气");
                    await caster.TryConsumeProcedure("灵气", stack);
                    await caster.GainBuffProcedure("力量", stack);
                    return null;
                }),
            
            new(name:                       "长明灯", // 香香匣
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                description:                "消耗\n获得灵气时：每1，生命+3",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("长明灯", 3);
                    return null;
                }),
            
            new(name:                       "大往生香", // 香香轮
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao | SkillType.LingQi,
                description:                "消耗\n永久免费+1",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("永久免费");
                    return null;
                }),
            
            new(name:                       "地府通讯器", // 缺少匣
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.LingQi,
                description:                "失去一半生命，每8，灵气+1",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    int gain = caster.Hp / 16;
                    await caster.LoseHealthProcedure(gain * 8);
                    await caster.GainBuffProcedure("灵气", gain);
                    return null;
                }),
            
            new(name:                       "无人机阵列", // 刃刃刃
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                description:                "消耗\n永久穿透+1",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("永久穿透");
                    return null;
                }),
            
            new(name:                       "弩炮", // 刃刃香
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                description:                "50攻 吸血",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(50, lifeSteal: true);
                    return null;
                }),
            
            new(name:                       "尖刺陷阱", // 刃刃匣
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao,
                description:                "下次受到攻击时，对对方施加等量减甲",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("尖刺陷阱");
                    return null;
                }),
            
            new(name:                       "暴雨梨花针", // 刃刃轮
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                description:                "1攻x10",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(1, times: 10);
                    return null;
                }),
            
            new(name:                       "炼丹炉", // 缺少轮
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                description:                "1攻x10",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("回合力量");
                    return null;
                }),
            
            new(name:                       "浮空艇", // 匣匣匣
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                description:                "消耗\n回合被跳过时，该回合无法受到伤害\n遭受12跳回合",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("浮空艇");
                    await caster.GainBuffProcedure("跳回合", 12);
                    return null;
                }),
            
            new(name:                       "动量中和器", // 匣匣香
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao,
                description:                "格挡+10",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("格挡", 10);
                    return null;
                }),
            
            new(name:                       "机关伞", // 匣匣刃
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao,
                description:                "灼烧+8",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灼烧", 8);
                    return null;
                }),
            
            new(name:                       "一轮马", // 匣匣轮
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao,
                description:                "闪避+6",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("闪避", 6);
                    return null;
                }),
            
            new(name:                       "外骨骼", // 缺少香
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                description:                "消耗\n攻击时，护甲+3",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("外骨骼", 3);
                    return null;
                }),
            
            new(name:                       "永动机", // 轮轮轮
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao | SkillType.LingQi,
                description:                "消耗\n力量+8 灵气+8\n8回合后死亡",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("力量", 8);
                    await caster.GainBuffProcedure("灵气", 8);
                    await caster.GainBuffProcedure("永动机", 8);
                    return null;
                }),
            
            new(name:                       "火箭靴", // 轮轮香
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                description:                "消耗\n使用灵气牌时，获得二动",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("火箭靴");
                    return null;
                }),
            
            new(name:                       "定龙桩", // 轮轮刃
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                description:                "消耗\n对方二动时，如果没有暴击，获得1",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("定龙桩");
                    return null;
                }),
            
            new(name:                       "飞行器", // 轮轮匣
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                description:                "消耗\n成功闪避时，如果对方没有跳回合，施加1",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("飞行器");
                    return null;
                }),
            
            new(name:                       "时光机", // 缺少刃
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                channelTimeEvaluator:       2,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                description:                "消耗\n使用一张牌前，升级",
                withinPool:                 false,
                execute: async (caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("时光机");
                    return null;
                }),

            #endregion
        });
    }

    public void Init()
    {
        List.Do(entry => entry.GenerateAnnotations());
    }

    public override SkillEntry DefaultEntry() => this["不存在的技能"];
}
