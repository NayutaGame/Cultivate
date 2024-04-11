
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
            // O JSMHT E
            new(id:                         "0000",
                name:                       "不存在的技能",
                wuXing:                     null,
                jingJieRange:               JingJie.LianQiOnly,
                castDescription:            (j, dj, costResult, castResult) => "不存在的技能",
                withinPool:                 false),

            new(id:                         "0001",
                name:                       "聚气术",
                wuXing:                     null,
                jingJieRange:               JingJie.LianQiOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    "灵气+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气");
                    return null;
                }),

            new(id:                         "0100",
                name:                       "摇曳",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{4 + 4 * ((dj + 1) / 2)}" +
                    $"\n锋锐+{2 + dj / 2}" +
                    (j >= JingJie.JinDan ? "\n金流转" : ""),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(4 + 4 * ((skill.Dj + 1) / 2));
                    await caster.GainBuffProcedure("锋锐", 2 + skill.Dj / 2);
                    if (skill.GetJingJie() >= JingJie.JinDan)
                        await caster.CycleProcedure(WuXing.Jin);
                    return null;
                }),

            new(id:                         "0102",
                name:                       "金刃",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{3 + dj}攻" +
                    $"\n施加{3 + dj}减甲",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.RemoveArmorProcedure(3 + skill.Dj);
                    return null;
                }),

            new(id:                         "0103",
                name:                       "寻猎",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{2 + dj}攻" +
                    $"\n击伤：施加{5 + 2 * dj}减甲".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(2 + skill.Dj,
                        damaged: async d =>
                        {
                            await caster.RemoveArmorProcedure(5 + 2 * skill.Dj);
                            cond = true;
                        },
                        wuXing: skill.Entry.WuXing);
                    return cond.ToCastResult();
                }),

            new(id:                         "0110",
                name:                       "刺穴",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+10" +
                    $"\n遭受{6 - dj}滞气",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 10);
                    await caster.GainBuffProcedure("滞气", 6 - skill.Dj);
                    return null;
                }),

            new(id:                         "0516",
                name:                       "金鳞",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromDj(dj => 3 - 2 * dj),
                costDescription:            CostDescription.ManaFromDj(dj => 3 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻 每有一点护甲多1攻",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(1 + Mathf.Max(0, caster.Armor), wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0115",
                name:                       "流云",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.ErDong,
                cost:                       CostResult.ManaFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ManaFromDj(dj => 2 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"2攻x3\n" +
                    $"击伤：二动".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(2,
                        times: 3,
                        wuXing: skill.Entry.WuXing,
                        damaged: async d =>
                        {
                            caster.SetActionPoint(2);
                            cond = true;
                        });
                    return cond.ToCastResult();
                }),

            new(id:                         "0117",
                name:                       "天地同寿",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.JinDan2HuaShen,
                cost:                       CostResult.ArmorFromValue(10),
                costDescription:            CostDescription.ArmorFromValue(10),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加{10 + 5 * dj}减甲",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.RemoveArmorProcedure(10 + 5 * skill.Dj);
                    return null;
                }),

            new(id:                         "0108",
                name:                       "诸行无常",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromValue(5),
                costDescription:            CostDescription.ChannelFromValue(5),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n造成伤害时：\n施加伤害值的减甲\n最多{5 + 4 * dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("诸行无常", 5 + 4 * skill.Dj);
                    return null;
                }),

            new(id:                         "0109",
                name:                       "凛冽",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{3 + dj}攻" +
                    $"\n击伤：锋锐+{3 + dj}".ApplyCond(castResult) +
                    (j >= JingJie.JinDan ? "\n金流转" : ""),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(3 + skill.Dj,
                        damaged: async d =>
                        {
                            await caster.GainBuffProcedure("锋锐", 3 + skill.Dj);
                            cond = true;
                        },
                        wuXing: skill.Entry.WuXing);

                    if (skill.GetJingJie() >= JingJie.JinDan)
                        await caster.CycleProcedure(WuXing.Jin);
                    return cond.ToCastResult();
                }),

            new(id:                         "0114",
                name:                       "袖里乾坤",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"暴击+{1 + dj}\n" +
                    $"消耗1{"柔韧".ApplyStyle(castResult, "0")}/{"锋锐".ApplyStyle(castResult, "1")}：暴击+1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int stack = 1 + skill.Dj;

                    bool cond0 = await caster.TryConsumeProcedure("柔韧") || await caster.IsFocused();
                    if (cond0)
                        stack++;

                    bool cond1 = await caster.TryConsumeProcedure("锋锐") || await caster.IsFocused();
                    if (cond1)
                        stack++;

                    await caster.GainBuffProcedure("暴击", stack);
                    return Style.CastResultFromBools(cond0, cond1);
                }),

            new(id:                         "0121",
                name:                       "齐物论",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"奇偶同时激活两个效果",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("齐物论");
                    return null;
                }),

            new(id:                         "0116",
                name:                       "无妄",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{3 + 3 * dj}攻x2\n" +
                    $"敌方有减甲：暴击".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().Armor < 0 || await caster.IsFocused();
                    await caster.AttackProcedure(3 + 3 * skill.Dj, crit: cond, wuXing: skill.Entry.WuXing);
                    cond = cond || caster.Opponent().Armor < 0 || await caster.IsFocused();
                    await caster.AttackProcedure(3 + 3 * skill.Dj, crit: cond, wuXing: skill.Entry.WuXing);
                    return cond.ToCastResult();
                }),

            new(id:                         "0124",
                name:                       "化神金",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.HuaShenOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"无效",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    return null;
                }),

            new(id:                         "0122",
                name:                       "人间无戈",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"20锋锐觉醒：死亡不会导致Stage结算",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("待激活的人间无戈");
                    return null;
                }),

            new(id:                         "0123",
                name:                       "暴雨梨花针",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻\n" +
                    $"奇偶：{"暴击+2".ApplyOdd(castResult)}/{"暴击释放".ApplyEven(castResult)}",
                cast:                       async (env, caster, skill, recursive) =>
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

                    return Style.CastResultFromOddEven(odd, even);
                }),

            new(id:                         "0205",
                name:                       "吐纳",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{2 + dj}\n" +
                    $"生命上限+{4 + 4 * dj}\n" +
                    (j >= JingJie.HuaShen ? "治疗可以穿上限" : ""),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 2 + skill.Dj);
                    if (skill.GetJingJie() >= JingJie.HuaShen)
                        await caster.GainBuffProcedure("吐纳");
                    // await Procedure
                    caster.MaxHp += 4 + 4 * skill.Dj;
                    return null;
                }),
            
            new(id:                         "0200",
                name:                       "恋花",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{4 + 2 * dj}攻 吸血\n" +
                    (j >= JingJie.JinDan ? $"消耗1{"锋锐".ApplyStyle(castResult, "0")}/{"格挡".ApplyStyle(castResult, "1")}：多6攻" : ""),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    if (skill.GetJingJie() >= JingJie.JinDan)
                    {
                        int value = 4 + 2 * skill.Dj;
                    
                        bool cond0 = await caster.TryConsumeProcedure("锋锐") || await caster.IsFocused();
                        if (cond0)
                            value += 6;

                        bool cond1 = await caster.TryConsumeProcedure("格挡") || await caster.IsFocused();
                        if (cond1)
                            value += 6;
                    
                        await caster.AttackProcedure(value, lifeSteal: true, wuXing: skill.Entry.WuXing);
                        return Style.CastResultFromBools(cond0, cond1);
                    }
                    else
                    {
                        int value = 4 + 2 * skill.Dj;
                    
                        await caster.AttackProcedure(value, lifeSteal: true, wuXing: skill.Entry.WuXing);
                        return null;
                    }
                }),
            
            new(id:                         "0201",
                name:                       "冰肌",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{2 + 6 * dj}攻" +
                    (j >= JingJie.JinDan ?
                        $"\n每造成7点伤害，格挡+1\n水流转" :
                        $"\n击伤：格挡+1".ApplyCond(castResult)),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    if (skill.GetJingJie() < JingJie.JinDan)
                    {
                        bool cond = false;
                        await caster.AttackProcedure(2 + 6 * skill.Dj,
                            wuXing: skill.Entry.WuXing,
                            damaged: async d =>
                            {
                                await caster.GainBuffProcedure("格挡");
                                cond = true;
                            });
                        return cond.ToCastResult();
                    }
                    else
                    {
                        await caster.AttackProcedure(2 + 6 * skill.Dj, wuXing: skill.Entry.WuXing,
                            damaged: d => caster.GainBuffProcedure("格挡", d.Value / 7));
                        await caster.CycleProcedure(WuXing.Shui);
                        return null;
                    }
                }),
            
            new(id:                         "0218",
                name:                       "不动明王诀",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"治疗20" +
                    $"\n遭受5缠绕",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.HealProcedure(20);
                    // await caster.GainBuffProcedure("缠绕", 5);
                    return null;
                }),
            
            new(id:                         "0310",
                name:                       "止水",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{8 + 3 * dj}攻" +
                    $"\n未击伤：治疗等量数值",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    int value = 8 + 3 * skill.Dj;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
                        undamaged: async d =>
                        {
                            cond = true;
                            await caster.HealProcedure(value);
                        });
                    return cond.ToCastResult();
                }),
            
            new(id:                         "0219",
                name:                       "一闪",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻 二动",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: skill.Entry.WuXing);
                    caster.SetActionPoint(2);
                    return null;
                }),
            
            new(id:                         "0311",
                name:                       "瑞雪",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"格挡+{1 + dj}\n" +
                    $"每1格挡，生命+2",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("格挡", 1 + skill.Dj);
                    int stack = caster.GetStackOfBuff("格挡");
                    await caster.HealProcedure(2 * stack);
                    return null;
                }),

            new(id:                         "0406",
                name:                       "抱朴",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao | SkillType.LingQi,
                cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"每回合：灵气+1\n" +
                    $"唯一灵气牌：多1层".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    bool cond = skill.NoOtherLingQi;
                    await caster.GainBuffProcedure("抱朴", cond ? 2 : 1);
                    return cond.ToCastResult();
                }),
            
            new(id:                         "0210",
                name:                       "腾浪",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.ErDong,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{6 + 3 * dj}攻\n" +
                    $"前P次使用时：遭受1跳卡牌\n" +
                    (j >= JingJie.YuanYing ? "二动" : ""),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(6 + 3 * skill.Dj, wuXing: skill.Entry.WuXing);
                    if (skill.StageCastedCount <= skill.SlotIndex)
                        await caster.GainBuffProcedure("跳卡牌");
                    if (skill.GetJingJie() >= JingJie.YuanYing)
                        caster.SetActionPoint(2);
                    return null;
                }),

            new(id:                         "0213",
                name:                       "气吞山河",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"将灵气补至本局最大值+{1 + dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int space = caster.HighestManaRecord - caster.GetMana() + 1 + skill.Dj;
                    await caster.GainBuffProcedure("灵气", space);
                    return null;
                }),

            new(id:                         "0214",
                name:                       "幻月狂乱",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗 永久吸血\n" +
                    $"回合内未攻击：遭受1跳回合",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("幻月狂乱");
                    return null;
                }),

            new(id:                         "0112",
                name:                       "敛息",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{15 + 5 * dj}攻\n" +
                    $"击伤：取消伤害，施加减甲".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(15 + 5 * skill.Dj, wuXing: WuXing.Shui,
                        damaged: async d =>
                        {
                            await caster.RemoveArmorProcedure(d.Value);
                            d.Cancel = true;
                            cond = true;
                        });
                    return cond.ToCastResult();
                }),
            
            new(id:                         "0216",
                name:                       "奔腾",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.ErDong,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"三动",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    caster.SetActionPoint(3);
                    return null;
                }),
            
            new(id:                         "0217",
                name:                       "摩诃钵特摩",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao | SkillType.ErDong,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"20格挡觉醒：八动，回合结束死亡",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("待激活的摩诃钵特摩");
                    return null;
                }),

            new(id:                         "0215",
                name:                       "一梦如是",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack | SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻\n" +
                    $"击伤：消耗，生命+[累计治疗]".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(1, skill.Entry.WuXing, damaged:
                        async d =>
                        {
                            await skill.ExhaustProcedure();
                            await caster.HealProcedure(caster.HealedRecord);
                            cond = true;
                        });
                    return cond.ToCastResult();
                }),
            
            new(id:                         "0300",
                name:                       "若竹",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj} 生命+{1 + 3 * dj}\n" +
                    $"穿透+1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    await caster.HealProcedure(1 + 3 * skill.Dj);
                    await caster.GainBuffProcedure("穿透", 1);
                    return null;
                }),
            
            new(id:                         "0301",
                name:                       "小松",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{6 + 2 * dj}攻 穿透" +
                    (j >= JingJie.JinDan
                        ? $"\n消耗1{"格挡".ApplyStyle(castResult, "0")}/{"力量".ApplyStyle(castResult, "1")}：多6攻"
                        : ""),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    if (skill.GetJingJie() >= JingJie.JinDan)
                    {
                        int value = 6 + 2 * skill.Dj;
                    
                        bool cond0 = await caster.TryConsumeProcedure("格挡") || await caster.IsFocused();
                        if (cond0)
                            value += 6;

                        bool cond1 = await caster.TryConsumeProcedure("力量") || await caster.IsFocused();
                        if (cond1)
                            value += 6;
                    
                        await caster.AttackProcedure(value, pierce: true, wuXing: skill.Entry.WuXing);
                        return Style.CastResultFromBools(cond0, cond1);
                    }
                    else
                    {
                        await caster.AttackProcedure(6 + 2 * skill.Dj, pierce: true, wuXing: skill.Entry.WuXing);
                        return null;
                    }
                }),

            new(id:                         "0302",
                name:                       "缭乱",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"力量+{1 + (dj / 2)}\n" +
                    $"{2 + 2 * dj}攻",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("力量", 1 + skill.Dj / 2);
                    await caster.AttackProcedure(2 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0323",
                name:                       "回春",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"双方护甲+{10 + 3 * dj}" +
                    $"\n双方生命+{10 + 3 * dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int value = 10 + 3 * skill.Dj;
                    await caster.GainArmorProcedure(value);
                    await caster.GiveArmorProcedure(value);
                    await caster.HealProcedure(value);
                    await caster.HealOppoProcedure(value);
                    return null;
                }),

            new(id:                         "0324",
                name:                       "断筋",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n施加2缠绕",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: skill.Entry.WuXing);
                    // await caster.GiveBuffProcedure("缠绕", 2);
                    return null;
                }),

            new(id:                         "0306",
                name:                       "潜龙在渊",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"第{3 + dj}+次使用：{(6 + 2 * dj) * (6 + 2 * dj)}攻\n".ApplyCond(castResult) +
                    $"闪避+1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = !(skill.StageCastedCount < 2 + skill.Dj);
                    if (cond)
                        await caster.AttackProcedure((6 + 2 * skill.Dj) * (6 + 2 * skill.Dj), wuXing: skill.Entry.WuXing);

                    await caster.GainBuffProcedure("闪避", 1);
                    
                    return cond.ToCastResult();
                }),

            new(id:                         "0313",
                name:                       "落英",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗所有灵气，每{6 - dj}，力量+1\n" +
                    $"木流转",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.TransferProcedure(6 - skill.Dj, "灵气", 1, "力量", true);
                    await caster.CycleProcedure(WuXing.Mu);
                    return null;
                }),

            new(id:                         "0312",
                name:                       "盛开",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"受治疗时：力量+1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("盛开");
                    return null;
                }),

            new(id:                         "0309",
                name:                       "见龙在田",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ChannelFromCastedCount(cc => 4 - cc),
                costDescription:            CostDescription.ChannelFromValue(4),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻 闪避+2\n" +
                    $"成长：吟唱-1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: WuXing.Mu);
                    await caster.GainBuffProcedure("闪避", 2);
                    return null;
                }),

            new(id:                         "0319",
                name:                       "鹤回翔",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"反转出牌顺序\n" +
                    (j >= JingJie.HuaShen ? $"二重+1" : ""),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    if (caster.Forward)
                        await caster.GainBuffProcedure("鹤回翔");
                    else
                        await caster.TryRemoveBuff("鹤回翔");

                    if (skill.GetJingJie() >= JingJie.HuaShen)
                        await caster.GainBuffProcedure("二重");
                    return null;
                }),

            new(id:                         "0316",
                name:                       "心斋",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.LingQi | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"所有耗蓝-1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("心斋");
                    return null;
                }),
            
            new(id:                         "0322",
                name:                       "飞龙在天",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻 闪避+1\n" +
                    $"初次：重置走步计数".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: WuXing.Mu);
                    await caster.GainBuffProcedure("闪避");

                    bool cond = skill.IsFirstTime;
                    if (cond)
                    {
                        caster._p = 0;
                        if (!caster._skills[0].Exhausted)
                            await caster.GainBuffProcedure("跳走步");
                    }
                    
                    return cond.ToCastResult();
                }),

            new(id:                         "0321",
                name:                       "刷新",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.HuaShenOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗所有灵气，每5，多重+1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.TransferProcedure(5, "灵气", 1, "多重", true);
                    return null;
                }),

            new(id:                         "0320",
                name:                       "通透世界",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"20力量觉醒：攻击具有穿透",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("待激活的通透世界");
                    return null;
                }),

            new(id:                         "0317",
                name:                       "亢龙有悔",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack | SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"未消耗4灵气：消耗".ApplyCond(castResult) +
                    $"\n1攻x2 闪避+2 力量+2",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = await caster.TryConsumeProcedure("灵气", 4) || await caster.IsFocused();
                    if (!cond)
                        await skill.ExhaustProcedure();
                    
                    await caster.GainBuffProcedure("闪避", 2);
                    await caster.GainBuffProcedure("力量", 2);
                    await caster.AttackProcedure(1, times: 2, wuXing: skill.Entry.WuXing);
                    return (!cond).ToCastResult();
                }),
            
            new(id:                         "0419",
                name:                       "舍生",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.HealthFromValue(3),
                costDescription:            CostDescription.HealthFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+4" +
                    $"\n金丹：消耗1/5生命，每10，灵气+1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    return null;
                }),

            new(id:                         "0411",
                name:                       "轰天",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"12攻" +
                    $"\n遭受3内伤",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(12, skill.Entry.WuXing);
                    await caster.GainBuffProcedure("内伤", 3);
                    return null;
                }),
            
            new(id:                         "0400",
                name:                       "云袖",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{2 + dj}攻x2\n" +
                    $"护甲+{3 + dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(2 + skill.Dj, wuXing: skill.Entry.WuXing, 2);
                    await caster.GainArmorProcedure(3 + skill.Dj);
                    return null;
                }),

            new(id:                         "0413",
                name:                       "正念",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灼烧+{1 + dj}\n" +
                    $"每5灼烧：净化1\n" +
                    $"火流转",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灼烧", 1 + skill.Dj);
                    int dispel = caster.GetStackOfBuff("灼烧") / 5;
                    await caster.DispelProcedure(dispel);
                    await caster.CycleProcedure(WuXing.Huo);
                    return null;
                }),

            new(id:                         "0420",
                name:                       "三味",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加3内伤",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GiveBuffProcedure("内伤", 3);
                    return null;
                }),

            new(id:                         "0404",
                name:                       "九射",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ChannelFromDj(dj => 3 + dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 3 + dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{5 + dj}攻x{3 + 2 * dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(5 + skill.Dj, times: 3 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0417",
                name:                       "登宝塔",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{8 + 2 * dj}\n" +
                    $"使用3次时：消耗",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(8 + 2 * skill.Dj);
                    if (skill.StageCastedCount >= 3)
                        await skill.ExhaustProcedure();
                    return null;
                }),

            new(id:                         "0408",
                name:                       "天衣无缝",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao | SkillType.Attack,
                cost:                       CostResult.ChannelFromValue(5),
                costDescription:            CostDescription.ChannelFromValue(5),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"若无攻击牌：每回合：{6 + 2 * dj}攻".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    bool cond = skill.NoOtherAttack;
                    if (cond)
                        await caster.GainBuffProcedure("天衣无缝", 6 + 2 * skill.Dj);
                    return cond.ToCastResult();
                }),

            new(id:                         "0421",
                name:                       "剑王行",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"若消耗过牌：二动".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: skill.Entry.WuXing);
                    bool cond = caster.ExhaustedCount > 0;
                    if (cond)
                        caster.SetActionPoint(2);
                    return cond.ToCastResult();
                }),

            new(id:                         "0418",
                name:                       "观众生",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{6 + 2 * dj}" +
                    $"\n每1被消耗卡：多{6 + 2 * dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure((caster.ExhaustedCount + 1) * (6 + 2 * skill.Dj));
                    return null;
                }),

            new(id:                         "0410",
                name:                       "淬体",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 5 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"受伤时：灼烧+1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("淬体");
                    return null;
                }),

            new(id:                         "0409",
                name:                       "化劲",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.HealthFromValue(5),
                costDescription:            CostDescription.HealthFromValue(5),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灼烧+{2 + dj}" +
                    $"\n施加3脆弱" +
                    $"\n火流转",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灼烧", 2 + skill.Dj);
                    await caster.GiveBuffProcedure("脆弱", 3);
                    await caster.CycleProcedure(WuXing.Huo);
                    return null;
                }),

            new(id:                         "0416",
                name:                       "净天地",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao | SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"下1张非攻击卡不消耗灵气，使用之后消耗",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("净天地");
                    return null;
                }),

            new(id:                         "0415",
                name:                       "凤凰涅槃",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"20灼烧激活：每轮：生命回满",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("待激活的凤凰涅槃");
                    return null;
                }),

            new(id:                         "0414",
                name:                       "天女散花",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻 每获得过1闪避，多攻击1次",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(1, times: caster.GainedEvadeRecord + 1, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0510",
                name:                       "马步",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                cost:                       CostResult.ChannelFromJiaShi(jiaShi => jiaShi ? 0 : 1),
                costDescription:            CostDescription.ChannelFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"架势：免除吟唱\n" +
                    $"护甲+{8 + 6 * dj}\n" +
                    (j >= JingJie.JinDan ? $"无护甲：翻倍".ApplyCond(castResult) : ""),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    if (skill.GetJingJie() >= JingJie.YuanYing)
                    {
                        bool cond = (caster.Armor <= 0) || await caster.IsFocused();
                        int bitShift = cond ? 1 : 0;
                        await caster.GainArmorProcedure((8 + 6 * skill.Dj) << bitShift);
                        return cond.ToCastResult();
                    }
                    else
                    {
                        await caster.GainArmorProcedure(8 + 6 * skill.Dj);
                        return null;
                    }
                }),

            new(id:                         "0523",
                name:                       "寸劲",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{8 + 4 * dj}攻" +
                    $"\n遭受{2 + 2 * dj}软弱",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(8 + 4 * skill.Dj, wuXing: WuXing.Tu);
                    await caster.GainBuffProcedure("软弱", 2 + 2 * skill.Dj);
                    return null;
                }),
            
            new(id:                         "0500",
                name:                       "落石",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{12 + 4 * dj}攻" +
                    (j >= JingJie.JinDan ? $"\n消耗所有灵气，每1：多{3 + dj}" : ""),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int value = 12 + 4 * skill.Dj;
                    if (skill.GetJingJie() >= JingJie.JinDan)
                    {
                        int mana = caster.GetMana();
                        await caster.LoseBuffProcedure("灵气", mana);
                        value += mana * (3 + skill.Dj);
                    }
                    
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0511",
                name:                       "鹤翼",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.LingQi | SkillType.ErDong,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{4 + dj}" +
                    $"\n架势：二动".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 4 + skill.Dj);
                    
                    bool cond = await caster.ToggleJiaShiProcedure();
                    caster.SetActionPoint(2);
                    return cond.ToCastResult();
                }),

            new(id:                         "0513",
                name:                       "玉骨",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"柔韧+{2 + dj}" +
                    $"\n每1柔韧，施加2减甲",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("柔韧", 2 + skill.Dj);
                    await caster.RemoveArmorProcedure(2 * caster.GetStackOfBuff("柔韧"));
                    return null;
                }),

            new(id:                         "0509",
                name:                       "一力降十会",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{8 + 2 * dj}攻" +
                    $"\n唯一攻击牌：跳走步".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(8 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    bool cond = skill.NoOtherAttack;
                    if (cond)
                        await caster.GainBuffProcedure("跳走步");
                    return cond.ToCastResult();
                }),

            new(id:                         "0525",
                name:                       "逆脉",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.HealthFromValue(10),
                costDescription:            CostDescription.HealthFromValue(10),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+20",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(20);
                    return null;
                }),

            new(id:                         "0111",
                name:                       "两仪",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n获得护甲时/施加减甲时：额外+{3 + dj}",
                cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("两仪", 3 + skill.Dj);
                    return null;
                }),

            new(id:                         "0526",
                name:                       "点穴",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n施加2滞气",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: skill.Entry.WuXing);
                    await caster.GiveBuffProcedure("滞气", 2);
                    return null;
                }),

            new(id:                         "0524",
                name:                       "龟息",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"柔韧+{3 + dj}\n" +
                    $"遭受{6 + 3 * dj}内伤",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("柔韧", 3 + skill.Dj);
                    await caster.GainBuffProcedure("内伤", 6 + 3 * skill.Dj);
                    return null;
                }),

            new(id:                         "0519",
                name:                       "天人合一",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ManaFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ManaFromDj(dj => 5 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"激活所有架势",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("天人合一");
                    return null;
                }),

            new(id:                         "0515",
                name:                       "龙象",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ChannelFromJiaShi(jiaShi => jiaShi ? 0 : 2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"架势：免除吟唱\n" +
                    $"{22 + 8 * dj}攻\n" +
                    $"击伤：护甲+击伤值".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(22 + 8 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: async d =>
                        {
                            cond = true;
                            await caster.GainArmorProcedure(d.Value);
                        });
                    return cond.ToCastResult();
                }),

            new(id:                         "0527",
                name:                       "化神土",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.HuaShenOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"无效",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    return null;
                }),

            new(id:                         "0521",
                name:                       "那由他",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"20柔韧觉醒：没有耗蓝阶段，Step阶段无法受影响，所有Buff层数不会再变化",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("待激活的那由他");
                    return null;
                }),

            new(id:                         "0522",
                name:                       "迦楼罗",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻 " +
                    $"对方生命高".ApplyStyle(castResult, "0") +
                    $"|" +
                    $"攻击牌多".ApplyStyle(castResult, "1") +
                    $"|" +
                    $"护甲高".ApplyStyle(castResult, "2") +
                    $"|" +
                    $"灵气高".ApplyStyle(castResult, "3") +
                    $"|" +
                    $"滞气少".ApplyStyle(castResult, "4") +
                    $"|" +
                    $"缠绕少".ApplyStyle(castResult, "5") +
                    $"|" +
                    $"软弱少".ApplyStyle(castResult, "6") +
                    $"|" +
                    $"内伤少".ApplyStyle(castResult, "7") +
                    $"|" +
                    $"腐朽少".ApplyStyle(castResult, "8") +
                    $"|" +
                    $"架势".ApplyStyle(castResult, "9") +
                    $"|" +
                    $"终结".ApplyStyle(castResult, "10") +
                    $"|" +
                    $"初次".ApplyStyle(castResult, "11") +
                    $"：翻倍",
                cast:                       async (env, caster, skill, recursive) =>
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
                    return Style.CastResultFromBools(cond0, cond1, cond2, cond3, cond4, cond5, cond6, cond7, cond8,
                        cond9, cond10, cond11);
                }),
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            











            





            
            
            
            
            
            
            new(id:                         "0101",
                name:                       "乘风",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{5 + dj}攻\n" +
                    $"若有锋锐：{3 + dj}攻".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = caster.GetStackOfBuff("锋锐") > 0 || await caster.IsFocused();
                    int add = cond ? 3 + skill.Dj : 0;
                    await caster.AttackProcedure(5 + skill.Dj + add, wuXing: skill.Entry.WuXing);

                    return cond.ToCastResult();
                }),

            new(id:                         "0104",
                name:                       "掠影",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"奇偶：" +
                    $"{5 + 2 * dj}攻".ApplyOdd(castResult) +
                    $"/" +
                    $"护甲+{5 + 2 * dj}".ApplyEven(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int value = 5 + 2 * skill.Dj;
                    bool odd = skill.IsOdd || await caster.IsFocused();
                    if (odd)
                        await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);
                    bool even = skill.IsEven || await caster.IsFocused();
                    if (even)
                        await caster.GainArmorProcedure(value);
                    return Style.CastResultFromOddEven(odd, even);
                }),

            new(id:                         "0105",
                name:                       "盘旋",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         null,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{4 + 2 * dj}\n施加{4 + 2 * dj}减甲",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(4 + 2 * skill.Dj);
                    await caster.RemoveArmorProcedure(4 + 2 * skill.Dj);
                    return null;
                }),

            new(id:                         "0106",
                name:                       "灵动",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{6 + 2 * dj}攻\n" +
                    $"敌方有减甲：多1次".ApplyStyle(castResult, "cond"),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().Armor < 0 || await caster.IsFocused();
                    int times = cond ? 2 : 1;
                    await caster.AttackProcedure(6 + 2 * skill.Dj, times: times, wuXing: skill.Entry.WuXing);
                    return cond.ToCastResult();
                }),

            new(id:                         "0107",
                name:                       "飞絮",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         null,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"奇偶：" +
                    $"施加{8 + 2 * dj}减甲".ApplyOdd(castResult) +
                    $"/" +
                    $"锋锐+{1 + dj}".ApplyEven(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool odd = skill.IsOdd || await caster.IsFocused();
                    if (odd)
                        await caster.RemoveArmorProcedure(8 + 2 * skill.Dj);
                    bool even = skill.IsEven || await caster.IsFocused(); 
                    if (even)
                        await caster.GainBuffProcedure("锋锐", 1 + skill.Dj);
                    return Style.CastResultFromOddEven(odd, even);
                }),

            new(id:                         "0113",
                name:                       "凝水",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.LingQi | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"击伤时：灵气+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("凝水");
                    return null;
                }),

            new(id:                         "0119",
                name:                       "追命",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"每1柔韧，施加2减甲",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.RemoveArmorProcedure(2 * caster.GetStackOfBuff("柔韧"));
                    return null;
                }),

            new(id:                         "0120",
                name:                       "千里神行符",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao | SkillType.ErDong,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"奇偶：" +
                    $"消耗".ApplyOdd(castResult) +
                    $"/" +
                    $"二动".ApplyEven(castResult) +
                    $"\n灵气+4",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool odd = skill.IsOdd || await caster.IsFocused();
                    if (odd)
                        await skill.ExhaustProcedure();
                    bool even = skill.IsEven || await caster.IsFocused();
                    if (even)
                        caster.SetActionPoint(2);

                    await caster.GainBuffProcedure("灵气", 4);
                    return Style.CastResultFromOddEven(odd, even);
                }),

            new(id:                         "0118",
                name:                       "旧山风",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.HuaShenOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗所有护甲，每{6 - dj}，锋锐+1\n" +
                    $"金流转",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int stack = caster.Armor / (6 - skill.Dj);
                    await caster.LoseArmorProcedure((6 - skill.Dj) * stack);
                    await caster.GainBuffProcedure("锋锐", stack);

                    await caster.CycleProcedure(WuXing.Jin);
                    return null;
                }),
            
            new(id:                         "0202",
                name:                       "满招损",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{5 + dj}攻\n" +
                    $"对方有灵气：{3 + dj}攻".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().GetStackOfBuff("灵气") > 0 || await caster.IsFocused();
                    int add = cond ? 3 + skill.Dj : 0;
                    await caster.AttackProcedure(5 + skill.Dj + add, wuXing: skill.Entry.WuXing);
                    return cond.ToCastResult();
                }),

            new(id:                         "0203",
                name:                       "清泉",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{2 + dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 2 + skill.Dj);
                    return null;
                }),

            new(id:                         "0204",
                name:                       "归意",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{10 + 2 * dj}攻\n" +
                    $"终结：吸血".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = skill.IsEnd || await caster.IsFocused();
                    await caster.AttackProcedure(10 + 2 * skill.Dj, lifeSteal: cond,
                        wuXing: skill.Entry.WuXing);
                    return cond.ToCastResult();
                }),

            new(id:                         "0206",
                name:                       "冰雨",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{10 + 2 * dj}攻\n" +
                    $"击伤：格挡+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(10 + 2 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: async d =>
                        {
                            cond = true;
                            await caster.GainBuffProcedure("格挡");
                        });
                    return cond.ToCastResult();
                }),

            new(id:                         "0207",
                name:                       "勤能补拙",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{10 + 4 * dj}\n" +
                    $"初次：遭受1跳回合".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(10 + 4 * skill.Dj);
                    bool cond = !skill.IsFirstTime || await caster.IsFocused();
                    if (!cond)
                        await caster.GainBuffProcedure("跳回合");
                    return cond.ToCastResult();
                }),

            new(id:                         "0208",
                name:                       "秋水",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{12 + 2 * dj}攻\n" +
                    $"消耗1锋锐：吸血".ApplyStyle(castResult, "0") +
                    $"\n" +
                    $"消耗1灵气：翻倍".ApplyStyle(castResult, "1"),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond0 = await caster.TryConsumeProcedure("锋锐") || await caster.IsFocused();
                    bool cond1 = await caster.TryConsumeProcedure("灵气") || await caster.IsFocused();
                    
                    int d = cond1 ? 2 : 1;
                    await caster.AttackProcedure((12 + 2 * skill.Dj) * d, lifeSteal: cond0, wuXing: skill.Entry.WuXing);
                    return Style.CastResultFromBools(cond0, cond1);
                }),

            new(id:                         "0209",
                name:                       "玄冰刺",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(4),
                costDescription:            CostDescription.ManaFromValue(4),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{16 + 8 * dj}攻\n" +
                    $"每造成{8 - dj}点伤害，格挡+1\n" +
                    $"水流转",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(16 + 8 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: d => caster.GainBuffProcedure("格挡", d.Value / (8 - skill.Dj)));
                    await caster.CycleProcedure(WuXing.Shui);
                    return null;
                }),

            new(id:                         "0211",
                name:                       "观棋烂柯",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ManaFromDj(dj => 1 - dj),
                costDescription:            CostDescription.ManaFromDj(dj => 1 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加1跳回合",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GiveBuffProcedure("跳回合");
                    return null;
                }),

            new(id:                         "0212",
                name:                       "激流",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ManaFromDj(dj => 1 - dj),
                costDescription:            CostDescription.ManaFromDj(dj => 1 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"生命+{5 + 5 * dj}\n" +
                    $"下一次使用牌时二动",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.HealProcedure(5 + 5 * skill.Dj);
                    await caster.GainBuffProcedure("二动");
                    return null;
                }),
            
            new(id:                         "0314",
                name:                       "旧飞龙在天",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"每轮：闪避补至1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("飞龙在天");
                    return null;
                }),
            
            new(id:                         "0304",
                name:                       "旧潜龙在渊",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"生命+{6 + 4 * dj}\n" +
                    $"初次：闪避+1".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.HealProcedure(6 + 4 * skill.Dj);
                    bool cond = skill.IsFirstTime || await caster.IsFocused();
                    if (cond)
                        await caster.GainBuffProcedure("闪避");
                    return cond.ToCastResult();
                }),
            
            new(id:                         "0303",
                name:                       "初桃",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}\n" +
                    $"生命+{3 + dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    await caster.HealProcedure(3 + skill.Dj);
                    return null;
                }),

            new(id:                         "0305",
                name:                       "早春",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"力量+{1 + (dj / 2)}\n" +
                    $"护甲+{6 + dj}\n" +
                    $"初次：翻倍".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = skill.IsFirstTime || await caster.IsFocused();
                    int mul = cond ? 2 : 1;
                    await caster.GainBuffProcedure("力量", (1 + (skill.Dj / 2)) * mul);
                    await caster.GainArmorProcedure((6 + skill.Dj) * mul);
                    return cond.ToCastResult();
                }),

            new(id:                         "0307",
                name:                       "回马枪",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"下次受攻击时：{12 + 4 * dj}攻",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("回马枪", 12 + 4 * skill.Dj);
                    return null;
                }),

            new(id:                         "0308",
                name:                       "千年笋",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{15 + 3 * dj}攻\n" +
                    $"消耗1格挡：穿透".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = await caster.TryConsumeProcedure("格挡") || await caster.IsFocused();
                    await caster.AttackProcedure(15 + 3 * skill.Dj, wuXing: skill.Entry.WuXing,
                        pierce: cond);
                    return cond.ToCastResult();
                }),

            new(id:                         "0315",
                name:                       "二重",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ChannelFromDj(dj => 1 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 1 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"下{1 + dj}张牌使用两次",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("二重", 1 + skill.Dj);
                    return null;
                }),

            new(id:                         "0318",
                name:                       "回响",
                wuXing:                     WuXing.Mu,
                jingJieRange:               JingJie.HuaShenOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"使用第一张牌",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    if (!recursive)
                        return null;
                    await caster.CastProcedure(caster._skills[0], false);
                    return null;
                }),
            new(id:                         "0402",
                name:                       "吐焰",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.HealthFromDj(dj => 2 + dj),
                costDescription:            CostDescription.HealthFromDj(dj => 2 + dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{8 + 3 * dj}攻",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(8 + 3 * skill.Dj, wuXing: skill.Entry.WuXing);
                    return null;
                }),
            
            new(id:                         "0401",
                name:                       "化焰",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{4 + 2 * dj}攻\n" +
                    $"灼烧+{1 + dj / 2}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(4 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.GainBuffProcedure("灼烧", 1 + skill.Dj / 2);
                    return null;
                }),

            new(id:                         "0403",
                name:                       "燃命",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.LingQi,
                cost:                       CostResult.HealthFromDj(dj => 3 + dj),
                costDescription:            CostDescription.HealthFromDj(dj => 3 + dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+3\n" +
                    $"{2 + 3 * dj}攻",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 3);
                    await caster.AttackProcedure(2 + 3 * skill.Dj, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0405",
                name:                       "聚火",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"灼烧+{2 + dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("灼烧", 2 + skill.Dj);
                    return null;
                }),

            new(id:                         "0407",
                name:                       "常夏",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{4 + dj}攻\n" +
                    $"每相邻1张火，多{4 + dj}攻",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int mul = 1;
                    mul += skill.Prev(true).Entry.WuXing == WuXing.Huo ? 1 : 0;
                    mul += skill.Next(true).Entry.WuXing == WuXing.Huo ? 1 : 0;
                    await caster.AttackProcedure((4 + skill.Dj) * mul, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0412",
                name:                       "燃灯留烬",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{6 + 2 * dj}\n" +
                    $"每1被消耗卡：多{6 + 2 * dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure((caster.ExhaustedCount + 1) * (6 + 2 * skill.Dj));
                    return null;
                }),
            
            new(id:                         "0501",
                name:                       "流沙",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{3 + dj}攻\n" +
                    $"灵气+{1 + dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + skill.Dj);
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    return null;
                }),

            new(id:                         "0502",
                name:                       "土墙",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}\n" +
                    $"护甲+{3 + dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    await caster.GainArmorProcedure(3 + skill.Dj);
                    return null;
                }),

            new(id:                         "0503",
                name:                       "地龙",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{7 + 2 * dj}攻\n" +
                    $"击伤：护甲+{7 + 2 * dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int value = 7 + 2 * skill.Dj;
                    bool cond = false;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
                        damaged: async d =>
                        {
                            cond = true;
                            await caster.GainArmorProcedure(value);
                        });
                    return cond.ToCastResult();
                }),

            new(id:                         "0504",
                name:                       "铁骨",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"柔韧+{1 + (1 + dj) / 2}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("柔韧", 1 + (1 + skill.Dj) / 2);
                    return null;
                }),

            new(id:                         "0505",
                name:                       "点星",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{8 + 2 * dj}攻\n" +
                    $"相邻牌都非攻击：翻倍".ApplyStyle(castResult, "0") +
                    $"\n" +
                    $"消耗1灵气：翻倍".ApplyStyle(castResult, "1"),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond0 = skill.NoAttackAdjacents || await caster.IsFocused();
                    bool cond1 = await caster.TryConsumeProcedure("灵气") || await caster.IsFocused();
                    int bitShift = 0;
                    bitShift += cond0 ? 1 : 0;
                    bitShift += cond1 ? 1 : 0;
                    await caster.AttackProcedure((8 + 2 * skill.Dj) << bitShift);
                    return Style.CastResultFromBools(cond0, cond1);
                }),

            new(id:                         "0506",
                name:                       "一莲托生",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+2\n" +
                    $"生命小于一半：翻倍".ApplyStyle(castResult, "0") +
                    $"\n" +
                    $"架势：翻倍".ApplyStyle(castResult, "1"),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond0 = (caster.MaxHp / 2 >= caster.Hp) || await caster.IsFocused();
                    bool cond1 = await caster.ToggleJiaShiProcedure() || await caster.IsFocused();
                    int bitShift = (cond0 ? 1 : 0) + (cond1 ? 1 : 0);
                    await caster.GainBuffProcedure("灵气", 2 << bitShift);
                    return Style.CastResultFromBools(cond0, cond1);
                }),

            new(id:                         "0507",
                name:                       "巩固",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{3 + dj}\n" +
                    $"每2灵气，护甲+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 3 + skill.Dj);
                    await caster.GainArmorProcedure(caster.GetMana() / 2);
                    return null;
                }),

            new(id:                         "0508",
                name:                       "软剑",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{9 + 4 * dj}攻\n" +
                    $"击伤：施加1缠绕",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(9 + 4 * skill.Dj, wuXing: skill.Entry.WuXing,
                        damaged: async d =>
                        {
                            cond = true;
                            await caster.GiveBuffProcedure("缠绕");
                        });
                    return cond.ToCastResult();
                }),

            new(id:                         "0512",
                name:                       "收刀",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"下回合护甲+{8 + 4 * dj}\n" +
                    $"架势：翻倍".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = await caster.ToggleJiaShiProcedure();
                    int bitShift = cond ? 1 : 0;
                    await caster.GainBuffProcedure("延迟护甲", (8 + 4 * skill.Dj) << bitShift);
                    return cond.ToCastResult();
                }),

            new(id:                         "0514",
                name:                       "抱元守一",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"柔韧+{3 + dj}\n" +
                    $"遭受{4 + dj}内伤",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("柔韧", 3 + skill.Dj);
                    await caster.GainBuffProcedure("内伤", 4 + skill.Dj);
                    return null;
                }),

            new(id:                         "0517",
                name:                       "铁布衫",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1次，受伤时：护甲+[受伤值]，" +
                    $"架势：2次".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = await caster.ToggleJiaShiProcedure();
                    int stack = cond ? 2 : 1;
                    await caster.GainBuffProcedure("铁布衫", stack);
                    return cond.ToCastResult();
                }),

            new(id:                         "0518",
                name:                       "拔刀",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{10 + 5 * dj}攻\n" +
                    $"架势：翻倍".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = await caster.ToggleJiaShiProcedure();
                    int bitShift = cond ? 1 : 0;
                    await caster.AttackProcedure((10 + 5 * skill.Dj) << bitShift, wuXing: skill.Entry.WuXing);
                    return cond.ToCastResult();
                }),

            new(id:                         "0520",
                name:                       "窑土",
                wuXing:                     WuXing.Tu,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"每1灼烧，护甲+2",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int stack = caster.GetStackOfBuff("灼烧");
                    await caster.GainArmorProcedure(2 * stack);
                    return null;
                }),

            #region 事件牌

            new(id:                         "0600",
                name:                       "一念",
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.ErDong,
                cost:                       CostResult.HealthFromDj(dj => 8 - 2 * dj),
                costDescription:            CostDescription.HealthFromDj(dj => 8 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    j <= JingJie.ZhuJi ? "二动" : "三动",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = skill.GetJingJie() <= JingJie.ZhuJi;
                    caster.SetActionPoint(cond ? 2 : 3);
                    return null;
                }),

            new(id:                         "0601",
                name:                       "无量劫",
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                cost:                       CostResult.ChannelFromValue(3),
                costDescription:            CostDescription.ChannelFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"治疗{18 + dj * 6}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.HealProcedure(18 + skill.Dj * 6);
                    return null;
                }),

            new(id:                         "0602",
                name:                       "百草集",
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ChannelFromValue(3),
                costDescription:            CostDescription.ChannelFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"如果存在锋锐，格挡，力量，闪避，灼烧，层数+{1 + dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
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

            new(id:                         "0603",
                name:                       "遗憾",
                wuXing:                     null,
                jingJieRange:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"对手失去{3 + dj}灵气",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.RemoveBuffProcedure("灵气", 3 + skill.Dj, false);
                    return null;
                }),

            new(id:                         "0604",
                name:                       "爱恋",
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"获得{2 + dj}集中",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("集中", 2 + skill.Dj, false);
                    return null;
                }),

            new(id:                         "0605",
                name:                       "射金乌",
                wuXing:                     WuXing.Huo,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ChannelFromValue(3),
                costDescription:            CostDescription.ChannelFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"5攻x{4 + 2 * dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(5, times: 4 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0606",
                name:                       "春雨",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.ZhuJi2HuaShen,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"双方生命+{20 + 5 * dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.HealProcedure(20 + 5 * skill.Dj);
                    await caster.Opponent().HealProcedure(20 + 5 * skill.Dj);
                    return null;
                }),

            new(id:                         "0607",
                name:                       "枯木",
                wuXing:                     WuXing.Jin,
                jingJieRange:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"双方遭受{5 + dj}腐朽",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("腐朽", 5 + skill.Dj);
                    await caster.GiveBuffProcedure("腐朽", 5 + skill.Dj);
                    return null;
                }),

            new(id:                         "0608",
                name:                       "玄武吐息法",
                wuXing:                     WuXing.Shui,
                jingJieRange:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"玄武吐息法",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    return null;
                }),

            #endregion

            #region 机关牌

            // 筑基
            
            new(id:                         "0700", // 香
                name:                       "醒神香", // 香
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJiOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) => "灵气+4",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 4);
                    return null;
                }),
            
            new(id:                         "0701", // 刃
                name:                       "飞镖", // 刃
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJiOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) => "12攻",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(12);
                    return null;
                }),
            
            new(id:                         "0702", // 匣
                name:                       "铁匣", // 匣
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJiOnly,
                skillTypeComposite:         SkillType.SunHao,
                castDescription:            (j, dj, costResult, castResult) => "护甲+12",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(12);
                    return null;
                }),
            
            new(id:                         "0703", // 轮
                name:                       "滑索", // 轮
                wuXing:                     null,
                jingJieRange:               JingJie.ZhuJiOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.ErDong | SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) => "三动 消耗",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    caster.SetActionPoint(3);
                    await skill.ExhaustProcedure();
                    return null;
                }),

            // 元婴
            
            new(id:                         "0704", // 香香
                name:                       "还魂香", // 香香
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) => "灵气+8",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 8);
                    return null;
                }),
            
            new(id:                         "0705", // 香刃
                name:                       "净魂刀", // 香刃
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.LingQi | SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) => "10攻\n击伤：灵气+1，对手灵气-1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10,
                        damaged: async d =>
                        {
                            await caster.GainBuffProcedure("灵气");
                            await caster.RemoveBuffProcedure("灵气");
                        });
                    return null;
                }),
            
            new(id:                         "0706", // 香匣
                name:                       "防护罩", // 香匣
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao,
                castDescription:            (j, dj, costResult, castResult) => "护甲+8\n每有1灵气，护甲+4",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int add = caster.GetStackOfBuff("灵气");
                    await caster.GainArmorProcedure(8 + add);
                    return null;
                }),
            
            new(id:                         "0707", // 香轮
                name:                       "能量饮料", // 香轮
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) => "下1次灵气减少时，加回",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气回收");
                    return null;
                }),
            
            new(id:                         "0708", // 刃刃
                name:                       "炎铳", // 刃刃
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) => "25攻",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(25);
                    return null;
                }),
            
            new(id:                         "0709", // 刃匣
                name:                       "机关人偶", // 刃匣
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) => "护甲+12\n10攻",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(12);
                    await caster.AttackProcedure(10);
                    return null;
                }),
            
            new(id:                         "0710", // 刃轮
                name:                       "铁陀螺", // 刃轮
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) => "2攻x6",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(2, times: 6);
                    return null;
                }),
            
            new(id:                         "0711", // 匣匣
                name:                       "防壁", // 匣匣
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao,
                castDescription:            (j, dj, costResult, castResult) => "护甲+20\n柔韧+2",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(20);
                    await caster.GainBuffProcedure("柔韧", 2);
                    return null;
                }),
            
            new(id:                         "0712", // 匣轮
                name:                       "不倒翁", // 匣轮
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao,
                castDescription:            (j, dj, costResult, castResult) => "下2次护甲减少时，加回",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("护甲回收", 2);
                    return null;
                }),
            
            new(id:                         "0713", // 轮轮
                name:                       "助推器", // 轮轮
                wuXing:                     null,
                jingJieRange:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.ErDong,
                castDescription:            (j, dj, costResult, castResult) => "二动 二重",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    caster.SetActionPoint(2);
                    await caster.GainBuffProcedure("二重");
                    return null;
                }),

            // 返虚
            
            new(id:                         "0714", // 香香香
                name:                       "反应堆", // 香香香
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "消耗\n遭受1不堪一击，永久二重+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("不堪一击");
                    await caster.GainBuffProcedure("永久二重");
                    return null;
                }),
            
            new(id:                         "0715", // 香香刃
                name:                       "烟花", // 香香刃
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "消耗所有灵气，每1，力量+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int stack = caster.GetStackOfBuff("灵气");
                    await caster.TryConsumeProcedure("灵气", stack);
                    await caster.GainBuffProcedure("力量", stack);
                    return null;
                }),
            
            new(id:                         "0716", // 香香匣
                name:                       "长明灯", // 香香匣
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "消耗\n获得灵气时：每1，生命+3",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("长明灯", 3);
                    return null;
                }),
            
            new(id:                         "0717", // 香香轮
                name:                       "大往生香", // 香香轮
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao | SkillType.LingQi,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "消耗\n永久免费+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("永久免费");
                    return null;
                }),
            
            new(id:                         "0718", // 缺少匣
                name:                       "地府通讯器", // 缺少匣
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.LingQi,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "失去一半生命，每8，灵气+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int gain = caster.Hp / 16;
                    await caster.LoseHealthProcedure(gain * 8);
                    await caster.GainBuffProcedure("灵气", gain);
                    return null;
                }),
            
            new(id:                         "0719", // 刃刃刃
                name:                       "无人机阵列", // 刃刃刃
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "消耗\n永久穿透+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("永久穿透");
                    return null;
                }),
            
            new(id:                         "0720", // 刃刃香
                name:                       "弩炮", // 刃刃香
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "50攻 吸血",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(50, lifeSteal: true);
                    return null;
                }),
            
            new(id:                         "0721", // 刃刃匣
                name:                       "尖刺陷阱", // 刃刃匣
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "下次受到攻击时，对对方施加等量减甲",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("尖刺陷阱");
                    return null;
                }),
            
            new(id:                         "0722", // 刃刃轮
                name:                       "暴雨梨花针", // 刃刃轮
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "1攻x10",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(1, times: 10);
                    return null;
                }),
            
            new(id:                         "0723", // 缺少轮
                name:                       "炼丹炉", // 缺少轮
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "1攻x10",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("回合力量");
                    return null;
                }),
            
            new(id:                         "0724", // 匣匣匣
                name:                       "浮空艇", // 匣匣匣
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "消耗\n回合被跳过时，该回合无法受到伤害\n遭受12跳回合",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("浮空艇");
                    await caster.GainBuffProcedure("跳回合", 12);
                    return null;
                }),
            
            new(id:                         "0725", // 匣匣香
                name:                       "动量中和器", // 匣匣香
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "格挡+10",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("格挡", 10);
                    return null;
                }),
            
            new(id:                         "0726", // 匣匣刃
                name:                       "机关伞", // 匣匣刃
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "灼烧+8",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灼烧", 8);
                    return null;
                }),
            
            new(id:                         "0727", // 匣匣轮
                name:                       "一轮马", // 匣匣轮
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "闪避+6",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("闪避", 6);
                    return null;
                }),
            
            new(id:                         "0728", // 缺少香
                name:                       "外骨骼", // 缺少香
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "消耗\n攻击时，护甲+3",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("外骨骼", 3);
                    return null;
                }),
            
            new(id:                         "0729", // 轮轮轮
                name:                       "永动机", // 轮轮轮
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao | SkillType.LingQi,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "消耗\n力量+8 灵气+8\n8回合后死亡",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("力量", 8);
                    await caster.GainBuffProcedure("灵气", 8);
                    await caster.GainBuffProcedure("永动机", 8);
                    return null;
                }),
            
            new(id:                         "0730", // 轮轮香
                name:                       "火箭靴", // 轮轮香
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "消耗\n使用灵气牌时，获得二动",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("火箭靴");
                    return null;
                }),
            
            new(id:                         "0731", // 轮轮刃
                name:                       "定龙桩", // 轮轮刃
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "消耗\n对方二动时，如果没有暴击，获得1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("定龙桩");
                    return null;
                }),
            
            new(id:                         "0732", // 轮轮匣
                name:                       "飞行器", // 轮轮匣
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "消耗\n成功闪避时，如果对方没有跳回合，施加1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("飞行器");
                    return null;
                }),
            
            new(id:                         "0733", // 缺少刃
                name:                       "时光机", // 缺少刃
                wuXing:                     null,
                jingJieRange:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "消耗\n使用一张牌前：升级",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
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

    public override SkillEntry DefaultEntry() => this["0000"];
}
