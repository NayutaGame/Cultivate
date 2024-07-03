
using System.Collections.Generic;
using UnityEngine;
using CLLibrary;

public class SkillCategory : Category<SkillEntry>
{
    public SkillCategory()
    {
        AddRange(new List<SkillEntry>()
        {
            #region 特殊牌
            
            new(id:                         "0000",
                name:                       "不存在的技能",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQiOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    "不存在的技能，出现则代表卡池已空",
                withinPool:                 false),

            new(id:                         "0001",
                name:                       "聚气术",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQiOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    "灵气+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气");
                    return null;
                }),
            
            new(id:                         "0002",
                name:                       "冲撞",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQiOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻" ,
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10);
                    return null;
                }),

            #endregion

            #region 标准牌

            new(id:                         "0103",
                name:                       "寻猎",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"2攻" +
                    $"\n击伤：施加{5 + 5 * dj}减甲".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(2,
                        didDamage: async d =>
                        {
                            await caster.RemoveArmorProcedure(5 + 5 * skill.Dj);
                            cond = true;
                        },
                        wuXing: skill.Entry.WuXing);
                    return cond.ToCastResult();
                }),

            new(id:                         "0102",
                name:                       "金刃",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(4 + dj)}攻" +
                    $"\n施加{Fib.ToValue(3 + dj)}减甲",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(Fib.ToValue(4 + skill.Dj), wuXing: skill.Entry.WuXing);
                    await caster.RemoveArmorProcedure(Fib.ToValue(3 + skill.Dj));
                    return null;
                }),

            new(id:                         "0124",
                name:                       "起势",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"4攻" +
                    $"\n击伤：灵气+{1 + dj}".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(4,
                        didDamage: async d =>
                        {
                            await caster.GainBuffProcedure("灵气", 1 + skill.Dj, induced: true);
                            cond = true;
                        },
                        wuXing: skill.Entry.WuXing);
                    return cond.ToCastResult();
                }),

            new(id:                         "0516",
                name:                       "金鳞",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromDj(dj => 2 - ((dj + 1) / 2)),
                costDescription:            CostDescription.ManaFromDj(dj => 2 - ((dj + 1) / 2)),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{4 + 4 * (dj / 2)}" +
                    $"\n1攻 每1护甲，多1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(4 + 4 * (skill.Dj / 2), induced: false);
                    await caster.AttackProcedure(1 + Mathf.Max(0, caster.Armor), wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0527",
                name:                       "追击",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromDj(dj => 2 - ((dj + 1) / 2)),
                costDescription:            CostDescription.ManaFromDj(dj => 2 - ((dj + 1) / 2)),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{3 + dj}攻 每携带1金：多{3 + dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int count = caster.CountSuch(s => s.Entry.WuXing == WuXing.Jin);
                    await caster.AttackProcedure(count * (3 + skill.Dj), wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0117",
                name:                       "天地同寿",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cost:                       CostResult.ArmorFromDj(dj => 10 + 10 * dj),
                costDescription:            CostDescription.ArmorFromDj(dj => 10 + 10 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加{10 + 10 * dj}减甲" +
                    $"\n初次：翻倍".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool firstTime = skill.IsFirstTime;
                    int mul = firstTime ? 2 : 1;
                    await caster.RemoveArmorProcedure(mul * (10 + 10 * skill.Dj));
                    return firstTime.ToCastResult();
                }),

            new(id:                         "0109",
                name:                       "凛冽",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"锋锐+{1 + dj} 金流转" +
                    $"\n[锋锐]攻 吸血",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("锋锐", 1 + skill.Dj);
                    await caster.AttackProcedure(caster.GetStackOfBuff("锋锐"), lifeSteal: true, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0106",
                name:                       "灵动",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{6 + 4 * dj}攻\n" +
                    $"敌方有减甲：多1次".ApplyStyle(castResult, "cond"),
                withinPool:                 true,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = caster.Opponent().Armor < 0 || await caster.IsFocused();
                    int times = cond ? 2 : 1;
                    await caster.AttackProcedure(6 + 4 * skill.Dj, times: times, wuXing: skill.Entry.WuXing);
                    return cond.ToCastResult();
                }),
            
            new(id:                         "0126",
                name:                       "刺穴",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+8" +
                    $"\n遭受{4 - dj}滞气" +
                    $"\n二动",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 8);
                    await caster.GainBuffProcedure("滞气", 4 - skill.Dj);
                    caster.SetActionPoint(2);
                    return null;
                }),

            new(id:                         "0114",
                name:                       "袖里乾坤",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ManaFromDj(dj => dj + 1),
                costDescription:            CostDescription.ManaFromDj(dj => dj + 1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"暴击+{1 + dj}" +
                    $"\n护甲+12",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("暴击", 1 + skill.Dj);
                    await caster.GainArmorProcedure(12, induced: false);
                    return null;
                }),

            new(id:                         "0116",
                name:                       "无妄",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
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

            new(id:                         "0112",
                name:                       "敛息",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{20 + 10 * dj}攻\n" +
                    $"击伤：取消伤害，施加减甲".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(20 + 10 * skill.Dj, wuXing: WuXing.Jin,
                        willDamage: async d =>
                        {
                            await caster.RemoveArmorProcedure(d.Value);
                            d.Cancel = true;
                            cond = true;
                        });
                    return cond.ToCastResult();
                }),
            
            new(id:                         "0110",
                name:                       "弹指",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{3 + dj}" +
                    $"\n消耗1暴击：翻倍".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = await caster.TryConsumeProcedure("暴击");
                    int bitShift = cond ? 1 : 0;
                    await caster.GainBuffProcedure("灵气", (3 + skill.Dj) << bitShift);
                    return cond.ToCastResult();
                }),

            new(id:                         "0123",
                name:                       "暴雨梨花针",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"暴击+2 施加10减甲" +
                    $"\n终结：1攻 暴击释放".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("暴击", 2);
                    await caster.RemoveArmorProcedure(10);
                    
                    bool cond = skill.IsEnd;
                    if (cond)
                    {
                        int critStack = caster.GetStackOfBuff("暴击");
                        await caster.TryConsumeProcedure("暴击", critStack);
                        await caster.AttackProcedure(1, skill.Entry.WuXing, willDamage: async d => d.Value *= 1 + critStack);
                    }

                    return cond.ToCastResult();
                }),
            
            new(id:                         "0100",
                name:                       "山风",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+15" +
                    $"\n每15护甲，锋锐+1 金流转",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(15, induced: false);
                    int gain = caster.Armor / 15;
                    await caster.GainBuffProcedure("锋锐", gain);
                    await caster.CycleProcedure(WuXing.Jin);
                    return null;
                }),
            
            new(id:                         "0200",
                name:                       "恋花",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(3 + dj) * 2}攻 吸血",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int value = Fib.ToValue(3 + skill.Dj) * 2;
                    await caster.AttackProcedure(value, lifeSteal: true, wuXing: skill.Entry.WuXing);
                    return null;
                }),
            
            new(id:                         "0201",
                name:                       "流霰",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{2 + 4 * dj}攻" +
                    (j < JingJie.JinDan ?
                        $"\n击伤：格挡+1".ApplyCond(castResult) :
                        $"\n每造成{10 - dj}点伤害，格挡+1 水流转"),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int value = 2 + 4 * skill.Dj;
                    if (skill.GetJingJie() < JingJie.JinDan)
                    {
                        bool cond = false;
                        await caster.AttackProcedure(value,
                            wuXing: skill.Entry.WuXing,
                            didDamage: async d =>
                            {
                                await caster.GainBuffProcedure("格挡");
                                cond = true;
                            });
                        return cond.ToCastResult();
                    }
                    else
                    {
                        await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
                            didDamage: d => caster.GainBuffProcedure("格挡", d.Value / (10 - skill.Dj)));
                        await caster.CycleProcedure(WuXing.Shui);
                        return null;
                    }
                }),

            new(id:                         "0205",
                name:                       "吐纳",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{2 + dj}" +
                    $"\n生命上限+{4 + 4 * dj}" +
                    (j >= JingJie.HuaShen ? "\n治疗可以穿上限" : ""),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 2 + skill.Dj);
                    if (skill.GetJingJie() >= JingJie.HuaShen)
                        await caster.GainBuffProcedure("吐纳");
                    // await Procedure
                    caster.MaxHp += 4 + 4 * skill.Dj;
                    return null;
                }),
            
            new(id:                         "0310",
                name:                       "止水",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    (j < JingJie.HuaShen ? $"{8 + 4 * dj}攻" : $"28攻") +
                    $"\n未击伤：治疗等量数值".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    int value = skill.GetJingJie() < JingJie.HuaShen ?
                        (8 + 4 * skill.Dj) :
                        24;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
                        undamaged: async d =>
                        {
                            cond = true;
                            await caster.HealProcedure(value, induced: true);
                        });
                    return cond.ToCastResult();
                }),
            
            new(id:                         "0329",
                name:                       "空幻",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       async (env, entity, skill, recursive) => new ManaCostResult(4 - skill.Dj - entity.CountSuch(s => s.Entry.WuXing == WuXing.Shui)),
                costDescription:            CostDescription.ManaFromDj(dj => 4 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{10 + 4 * dj}攻 吸血\n每携带1水：消耗-1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10 + 4 * skill.Dj, lifeSteal: true, wuXing: skill.Entry.WuXing);
                    return null;
                }),
            
            new(id:                         "0220",
                name:                       "踏浪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                cost:                       CostResult.ManaFromDj(dj => 3 + dj),
                costDescription:            CostDescription.ManaFromDj(dj => 3 + dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{7 + 2 * dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 7 + 2 * skill.Dj);
                    return null;
                }),
            
            new(id:                         "0219",
                name:                       "一闪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.ErDong,
                cost:                       CostResult.ManaFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ManaFromDj(dj => 2 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{2 + 6 * dj}攻" +
                    $"\n二动",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(2 + 6 * skill.Dj, wuXing: skill.Entry.WuXing);
                    caster.SetActionPoint(2);
                    return null;
                }),
            
            new(id:                         "0311",
                name:                       "瑞雪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cost:                       CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"格挡+{1 + dj}" +
                    $"\n水流转" +
                    $"\n每1格挡，生命+2",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("格挡", 1 + skill.Dj);
                    await caster.CycleProcedure(WuXing.Shui);
                    int stack = caster.GetStackOfBuff("格挡");
                    await caster.HealProcedure(2 * stack, induced: false);
                    return null;
                }),
            
            new(id:                         "0210",
                name:                       "腾浪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.ErDong,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"6攻" +
                    $"\n前P次使用时：遭受1跳卡牌" +
                    (j < JingJie.HuaShen ? "" : "\n二动"),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(6, wuXing: skill.Entry.WuXing);
                    if (skill.StageCastedCount <= skill.SlotIndex)
                        await caster.GainBuffProcedure("跳卡牌");
                    if (skill.GetJingJie() >= JingJie.HuaShen)
                        caster.SetActionPoint(2);
                    return null;
                }),

            new(id:                         "0213",
                name:                       "气吞山河",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"将灵气补至本局最大值+{1 + dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int space = caster.HighestManaRecord - caster.GetMana() + 1 + skill.Dj;
                    await caster.GainBuffProcedure("灵气", space);
                    return null;
                }),
            
            new(id:                         "0218",
                name:                       "不动明王诀",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"治疗{30 + 10 * dj}" +
                    $"\n遭受5缠绕",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.HealProcedure(30 + 10 * skill.Dj, induced: false);
                    await caster.GainBuffProcedure("缠绕", 5);
                    return null;
                }),
            
            new(id:                         "0330",
                name:                       "吞天",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{10 + 5 * dj}攻 暴击 吸血",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10 + 5 * skill.Dj, crit: true, lifeSteal: true, wuXing: skill.Entry.WuXing);
                    return null;
                }),
            
            new(id:                         "0216",
                name:                       "奔腾",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.ErDong,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"三动",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    caster.SetActionPoint(3);
                    return null;
                }),

            new(id:                         "0215",
                name:                       "一梦如是",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻" +
                    $"\n首次击伤：生命+[累计治疗]".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = caster.GetStackOfBuff("一梦如是已触发") > 1;
                    await caster.AttackProcedure(1, skill.Entry.WuXing, didDamage:
                        async d =>
                        {
                            await caster.GainBuffProcedure("一梦如是已触发");
                            await caster.HealProcedure(caster.HealedRecord, induced: true);
                        });
                    return cond.ToCastResult();
                }),
            
            new(id:                         "0301",
                name:                       "小松",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(4 + dj)}攻 穿透" +
                    $"\n成长：多{Fib.ToValue(3 + dj)}攻",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int value = Fib.ToValue(4 + skill.Dj) + (Fib.ToValue(3 + skill.Dj)) * skill.StageCastedCount;
                    await caster.AttackProcedure(value, pierce: true, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0306",
                name:                       "潜龙在渊",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"闪避+1" +
                    $"\n成长{2 + dj}次后：{(4 + 2 * dj) * (4 + 2 * dj)}攻",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    if (skill.StageCastedCount >= 2 + skill.Dj)
                        await caster.AttackProcedure((4 + 2 * skill.Dj) * (4 + 2 * skill.Dj), wuXing: skill.Entry.WuXing);

                    await caster.GainBuffProcedure("闪避");
                    return null;
                }),

            new(id:                         "0325",
                name:                       "水滴石穿",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    (j < JingJie.HuaShen ? $"{5 + 2 * dj}攻" : $"17攻") +
                    $"\n终结：穿透".ApplyStyle(castResult, "0") +
                    $"\n击伤：穿透+1".ApplyStyle(castResult, "1"),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool isEnd = skill.IsEnd;
                    bool damaged = false;

                    int value = skill.GetJingJie() < JingJie.HuaShen ? (5 + 2 * skill.Dj) : 15;
                    
                    await caster.AttackProcedure(value, pierce: isEnd, wuXing: skill.Entry.WuXing,
                        didDamage: async d =>
                        {
                            damaged = true;
                            await caster.GainBuffProcedure("穿透", induced: true);
                        });
                    
                    return Style.CastResultFromBools(isEnd, damaged);
                }),
            
            new(id:                         "0332",
                name:                       "明神",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}" +
                    $"\n集中+1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    await caster.GainBuffProcedure("集中");
                    return null;
                }),

            new(id:                         "0323",
                name:                       "回春",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"双方护甲+{10 + 6 * dj}" +
                    $"\n双方生命+{10 + 6 * dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int value = 10 + 6 * skill.Dj;
                    await caster.GainArmorProcedure(value, induced: false);
                    await caster.GiveArmorProcedure(value, induced: false);
                    await caster.HealProcedure(value, induced: false);
                    await caster.HealOppoProcedure(value, induced: false);
                    return null;
                }),

            new(id:                         "0324",
                name:                       "断筋",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n施加{2 + dj}缠绕",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: skill.Entry.WuXing);
                    await caster.GiveBuffProcedure("缠绕", 2 + skill.Dj);
                    return null;
                }),
            
            new(id:                         "0300",
                name:                       "若竹",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}" +
                    $"\n穿透+1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    await caster.GainBuffProcedure("穿透");
                    return null;
                }),
            
            new(id:                         "0331",
                name:                       "霹雳",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{10 + 10 * dj}攻 遭受2缠绕",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10 + 10 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.GainBuffProcedure("缠绕", 2);
                    return null;
                }),

            new(id:                         "0313",
                name:                       "落英",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"力量+{1 + dj}" +
                    $"\n消耗每{5 - dj}灵气，多1" +
                    $"\n木流转",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("力量", 1 + skill.Dj);
                    await caster.TransferProcedure(5 - skill.Dj, "灵气", 1, "力量", true);
                    await caster.CycleProcedure(WuXing.Mu);
                    return null;
                }),

            new(id:                         "0309",
                name:                       "见龙在田",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       async (env, entity, skill, recursive) => new ChannelCostResult(3 - skill.Dj - skill.StageCastedCount),
                costDescription:            CostDescription.ChannelFromDj(dj => 3 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻 闪避+2" +
                    $"\n成长：吟唱-1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: WuXing.Mu);
                    await caster.GainBuffProcedure("闪避", 2);
                    return null;
                }),

            new(id:                         "0302",
                name:                       "缭乱",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"力量+{dj}" +
                    $"\n成长:多1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("力量", skill.Dj + skill.StageCastedCount);
                    return null;
                }),

            new(id:                         "0326",
                name:                       "谈笑风生",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cost:                       CostResult.ManaFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ManaFromDj(dj => 2 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"生命+{9 + 4 * dj}" +
                    $"\n消耗1暴击：翻倍".ApplyStyle(castResult, "0") +
                    $"\n消耗1吸血：造成伤害".ApplyStyle(castResult, "1"),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool consumeCrit = await caster.TryConsumeProcedure("暴击");
                    bool consumeLifesteal = caster.GetStackOfBuff("幻月狂乱") > 0 || await caster.TryConsumeProcedure("吸血");

                    int bitShift = consumeCrit ? 1 : 0;
                    int value = (9 + 4 * skill.Dj) << bitShift;
                    await caster.HealProcedure(value, induced: false);
                    if (consumeLifesteal)
                        await caster.IndirectProcedure(value, skill.Entry.WuXing);
                    return Style.CastResultFromBools(consumeCrit, consumeLifesteal);
                }),
    
            new(id:                         "0319",
                name:                       "鹤回翔",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ChannelFromDj(dj => 1 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 1 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"反转出牌顺序" +
                    (j <= JingJie.YuanYing ? $"" :
                        $"\n二重+1"),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    if (caster.Forward)
                        await caster.GainBuffProcedure("鹤回翔");
                    else
                        await caster.TryRemoveBuff("鹤回翔");

                    if (skill.GetJingJie() > JingJie.YuanYing)
                        await caster.GainBuffProcedure("二重");
                    return null;
                }),
            
            new(id:                         "0322",
                name:                       "飞龙在天",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ChannelFromDj(dj => dj - 1),
                costDescription:            CostDescription.ChannelFromDj(dj => dj - 1),
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

            new(id:                         "0327",
                name:                       "入木三分",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻" +
                    (j < JingJie.HuaShen ? $"" : $"\n穿透") +
                    $"\n对方每有2护甲，多1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int value = 1 + caster.Opponent().Armor / 2;
                    bool pierce = skill.GetJingJie() >= JingJie.HuaShen;

                    await caster.AttackProcedure(value, pierce: pierce, wuXing: skill.Entry.WuXing);
                    
                    return null;
                }),
            
            new(id:                         "0328",
                name:                       "钟声",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"升级下{2 + dj}张使用的牌",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("钟声", 2 + skill.Dj);
                    return null;
                }),

            new(id:                         "0321",
                name:                       "刷新",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"多重+1\n消耗每6灵气，多1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("多重");
                    await caster.TransferProcedure(6, "灵气", 1, "多重", true);
                    return null;
                }),

            new(id:                         "0317",
                name:                       "亢龙有悔",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"无法受到治疗" +
                    $"\n1攻x3 闪避+3 力量+3",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("禁止治疗");
                    await caster.GainBuffProcedure("闪避", 3);
                    await caster.GainBuffProcedure("力量", 3);
                    await caster.AttackProcedure(1, times: 3, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0411",
                name:                       "轰天",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.HealthFromDj(dj => Fib.ToValue(4 + dj)),
                costDescription:            CostDescription.HealthFromDj(dj => Fib.ToValue(4 + dj)),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(6 + dj)}攻",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(Fib.ToValue(6 + skill.Dj), skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0420",
                name:                       "三味",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加{3 + dj}内伤",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GiveBuffProcedure("内伤", 3 + skill.Dj);
                    return null;
                }),
            
            new(id:                         "0400",
                name:                       "云袖",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{2 + 2 * dj}攻x2\n" +
                    $"护甲+{2 + 2 * dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int value = 2 + 2 * skill.Dj;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing, 2);
                    await caster.GainArmorProcedure(value, induced: false);
                    return null;
                }),

            new(id:                         "0417",
                name:                       "正念",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 4 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 4 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n灼烧+{dj} 每1张已消耗牌，多1 火流转" +
                    $"\n每1灼烧，1净化",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("灼烧", skill.Dj + caster.ExhaustedCount);
                    await caster.DispelProcedure(caster.GetStackOfBuff("灼烧"));
                    return null;
                }),
            
            new(id:                         "0419",
                name:                       "舍生",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                cost:                       async (env, entity, skill, recursive) =>
                    new HealthCostResult(entity.IsLowHp ? 0 : 10),
                costDescription:            CostDescription.HealthFromValue(10),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{4 + dj}" +
                    $"\n残血时：无需消耗生命",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 4 + skill.Dj);
                    return null;
                }),

            new(id:                         "0404",
                name:                       "九射",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{7 + dj}攻x{3 + 2 * dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(7 + skill.Dj, times: 3 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0407",
                name:                       "常夏",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{3 + dj}攻 每携带1火，多1次",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int count = caster.CountSuch(s => s.Entry.WuXing == WuXing.Huo);
                    await caster.AttackProcedure(3 + skill.Dj, wuXing: skill.Entry.WuXing, times: count);
                    return null;
                }),

            new(id:                         "0413",
                name:                       "登宝塔",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n护甲+6 每1张已消耗牌，多6",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainArmorProcedure(6 * (1 + caster.ExhaustedCount), induced: false);
                    return null;
                }),

            new(id:                         "0421",
                name:                       "剑王行",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n若消耗过牌：二动".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: skill.Entry.WuXing);
                    bool cond = caster.ExhaustedCount > 0;
                    if (cond)
                        caster.SetActionPoint(2);
                    return cond.ToCastResult();
                }),

            new(id:                         "0422",
                name:                       "坐忘",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.LingQi | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 3 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 3 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n免费+1 每1张已消耗牌，多1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("免费", 1 + caster.ExhaustedCount);
                    return null;
                }),

            new(id:                         "0418",
                name:                       "观众生",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.LingQi | SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"下{1 + dj}张非攻击卡具有免费和消耗",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("观众生", 1 + skill.Dj);
                    return null;
                }),

            new(id:                         "0409",
                name:                       "化劲",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.HealthFromValue(5),
                costDescription:            CostDescription.HealthFromValue(5),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灼烧+{2 + dj} 火流转" +
                    $"\n施加3软弱",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灼烧", 2 + skill.Dj);
                    await caster.GiveBuffProcedure("软弱", 3);
                    await caster.CycleProcedure(WuXing.Huo);
                    return null;
                }),

            new(id:                         "0423",
                name:                       "彻悟",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{30 + 20 * dj}攻 成为残血",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(30 + 20 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.BecomeLowHealth();
                    return null;
                }),

            new(id:                         "0424",
                name:                       "晚霞",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                skillTypeComposite:         SkillType.XiaoHao | SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n灵气+8",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    if (!recursive)
                        return null;
                    
                    await skill.ExhaustProcedure();
                    foreach (StageSkill s in caster._skills)
                    {
                        if (!s.Exhausted)
                            continue;
                        await caster.CastProcedure(s, recursive: false);
                    }
                    return null;
                }),

            new(id:                         "0416",
                name:                       "净天地",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n使用一次所有消耗牌",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    if (!recursive)
                        return null;
                    
                    await skill.ExhaustProcedure();
                    foreach (StageSkill s in caster._skills)
                    {
                        if (!s.Exhausted)
                            continue;
                        await caster.CastProcedure(s, recursive: false);
                    }
                    return null;
                }),

            new(id:                         "0414",
                name:                       "天女散花",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
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
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       CostResult.ChannelFromJiaShi(jiaShi => jiaShi ? 0 : 1),
                costDescription:            CostDescription.ChannelFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"架势：免除吟唱" +
                    $"\n护甲+{Fib.ToValue(6 + dj)}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(Fib.ToValue(6 + skill.Dj), induced: false);
                    return null;
                }),

            new(id:                         "0523",
                name:                       "寸劲",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(5 + dj) * 2 - 2}攻" +
                    $"\n遭受{3 + 2 * dj}软弱",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(Fib.ToValue(5 + skill.Dj) * 2 - 2, wuXing: WuXing.Tu);
                    await caster.GainBuffProcedure("软弱", 3 + 2 * skill.Dj);
                    return null;
                }),
            
            new(id:                         "0500",
                name:                       "落石",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{17 + 3 * dj}攻" +
                    (j < JingJie.JinDan ? "" : $"\n消耗所有灵气，每1：多{dj}攻"),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int value = 17 + 3 * skill.Dj;
                    if (skill.GetJingJie() >= JingJie.JinDan)
                    {
                        int mana = caster.GetMana();
                        await caster.LoseBuffProcedure("灵气", mana);
                        value += mana * (3 + skill.Dj);
                    }
                    
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);
                    return null;
                }),
            
            new(id:                         "0501",
                name:                       "流沙",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{3 + 2 * dj}攻\n" +
                    $"灵气+{1 + dj / 2}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(3 + 2 * skill.Dj);
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj / 2);
                    return null;
                }),

            new(id:                         "0511",
                name:                       "鹤翼",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                cost:                       async (env, entity, skill, recursive) =>
                    new ChannelCostResult((skill.GetJingJie() >= JingJie.YuanYing && entity.GetStackOfBuff("灵气") == 0) ? 0 : 1),
                costDescription:            CostDescription.ChannelFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}" +
                    $"\n架势：翻倍".ApplyCond(castResult) +
                    (j < JingJie.YuanYing ? "" : $"\n无灵气：免除吟唱"),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = await caster.ToggleJiaShiProcedure();
                    int bitShift = cond ? 1 : 0;
                    await caster.GainBuffProcedure("灵气", (1 + skill.Dj) << bitShift);
                    return cond.ToCastResult();
                }),

            new(id:                         "0507",
                name:                       "巩固",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}\n" +
                    $"每1灵气，护甲+{1 + dj / 2}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    await caster.GainArmorProcedure(caster.GetMana() * (1 + skill.Dj / 2), induced: false);
                    return null;
                }),

            new(id:                         "0509",
                name:                       "一力降十会",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{(5 * dj * dj + 35 * dj + 50) / 2}攻" +
                    $"\n遭受4跳回合" +
                    $"\n唯一攻击牌：免除跳回合".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int value = (5 * skill.Dj * skill.Dj + 35 * skill.Dj + 50) / 2;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);

                    bool cond = skill.NoOtherAttack;
                    if (!cond)
                        await caster.GainBuffProcedure("跳回合", 4);
                    return cond.ToCastResult();
                }),

            new(id:                         "0528",
                name:                       "霸王",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{4 + 4 * dj}攻" +
                    $"\n初次：翻倍".ApplyStyle(castResult, "0") +
                    $"\n终结：翻倍".ApplyStyle(castResult, "1"),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond0 = skill.IsFirstTime;
                    bool cond1 = skill.IsEnd;
                    int value = (4 + 4 * skill.Dj) << ((cond0 ? 1 : 0) + (cond1 ? 1 : 0));
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);
                    return Style.CastResultFromBools(cond0, cond1);
                }),

            new(id:                         "0525",
                name:                       "逆脉",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{15 + 10 * dj}" +
                    $"\n非残血：翻倍" +
                    $"\n成为残血",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool notLowHp = !caster.IsLowHp;
                    int bitShift = notLowHp ? 1 : 0;
                    await caster.GainArmorProcedure((15 + 10 * skill.Dj) << bitShift, induced: false);

                    if (notLowHp)
                    {
                        await caster.BecomeLowHealth();
                    }
                    return null;
                }),

            new(id:                         "0526",
                name:                       "点穴",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n施加{2 + dj}滞气",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10, wuXing: skill.Entry.WuXing);
                    await caster.GiveBuffProcedure("滞气", 2 + skill.Dj);
                    return null;
                }),

            new(id:                         "0513",
                name:                       "玉骨",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cost:                       CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"柔韧+{2 + dj} 土流转" +
                    $"\n每4柔韧，暴击+1",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("柔韧", 2 + skill.Dj);
                    await caster.CycleProcedure(WuXing.Tu);
                    int stack = caster.GetStackOfBuff("柔韧");
                    int add = stack / 4;
                    await caster.GainBuffProcedure("暴击", add);
                    return null;
                }),

            new(id:                         "0529",
                name:                       "螣蛇",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻 护甲+1" +
                    $"\n每携带1张土：多{2 + dj}",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int count = caster.CountSuch(s => s.Entry.WuXing == WuXing.Tu);
                    int value = 1 + count * (2 + skill.Dj);
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);
                    await caster.GainArmorProcedure(value, induced: false);
                    return null;
                }),

            new(id:                         "0524",
                name:                       "龟息",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"柔韧+{3 + dj} 土流转" +
                    $"\n遭受{4 + dj}内伤",
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("柔韧", 3 + skill.Dj);
                    await caster.CycleProcedure(WuXing.Tu);
                    await caster.GainBuffProcedure("内伤", 4 + skill.Dj);
                    return null;
                }),

            new(id:                         "0515",
                name:                       "龙象",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ChannelFromJiaShi(jiaShi => jiaShi ? 0 : 1),
                costDescription:            CostDescription.ChannelFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"架势：免除吟唱" +
                    $"\n{16 + 9 * dj}攻" +
                    $"\n击伤：护甲+击伤值".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(16 + 9 * skill.Dj, wuXing: skill.Entry.WuXing,
                        didDamage: async d =>
                        {
                            cond = true;
                            await caster.GainArmorProcedure(d.Value, induced: true);
                        });
                    return cond.ToCastResult();
                }),

            new(id:                         "0522",
                name:                       "迦楼罗",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.HuaShenOnly,
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
            
            #endregion

            #region 之前的被动们

            new(id:                         "0108",
                name:                       "诸行无常",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 4 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 4 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n击伤时：施加{3 + 2 * dj}减甲" +
                    $"\n每携带1金，层数+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    int n = caster.CountSuch(stageSkill => stageSkill.Entry.WuXing == WuXing.Jin);
                    await caster.GainBuffProcedure("诸行无常", n + 3 + 2 * skill.Dj);
                    return null;
                }),

            new(id:                         "0121",
                name:                       "齐物论",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n奇偶同时激活两个效果",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("齐物论");
                    return null;
                }),

            new(id:                         "0122",
                name:                       "人间无戈",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"20锋锐觉醒：死亡不会导致Stage结算",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("待激活的人间无戈");
                    return null;
                }),

            new(id:                         "0406",
                name:                       "抱朴",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao | SkillType.LingQi,
                cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"每回合：灵气+1\n" +
                    $"唯一灵气牌：多1层".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    bool cond = skill.NoOtherLingQi;
                    await caster.GainBuffProcedure("抱朴", cond ? 2 : 1);
                    return cond.ToCastResult();
                }),

            new(id:                         "0214",
                name:                       "幻月狂乱",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗 所有攻击具有吸血\n" +
                    $"回合内未攻击：遭受1跳回合",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("幻月狂乱");
                    return null;
                }),
            
            new(id:                         "0217",
                name:                       "摩诃钵特摩",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao | SkillType.ErDong,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"20格挡觉醒：连续行动八次，回合结束死亡",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("待激活的摩诃钵特摩");
                    return null;
                }),

            new(id:                         "0312",
                name:                       "盛开",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n受治疗时：力量+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("盛开");
                    return null;
                }),

            new(id:                         "0316",
                name:                       "心斋",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.LingQi | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n所有耗蓝-1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("心斋");
                    return null;
                }),

            new(id:                         "0320",
                name:                       "通透世界",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"20力量觉醒：攻击具有穿透",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("待激活的通透世界");
                    return null;
                }),

            new(id:                         "0408",
                name:                       "天衣无缝",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao | SkillType.Attack,
                cost:                       CostResult.ChannelFromValue(5),
                costDescription:            CostDescription.ChannelFromValue(5),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n每回合：{6 + 2 * dj}攻" +
                    $"\n卡组无其他攻击牌：多3层".ApplyCond(castResult) +
                    $"\n无法使用其他方式攻击",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    bool cond = skill.NoOtherAttack;
                    int stack = 6 + 2 * skill.Dj;
                    if (cond)
                        stack += 3;
                    await caster.GainBuffProcedure("天衣无缝", stack);
                    return cond.ToCastResult();
                }),

            new(id:                         "0410",
                name:                       "淬体",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 5 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"受伤时：灼烧+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("淬体");
                    return null;
                }),

            new(id:                         "0415",
                name:                       "凤凰涅槃",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n20灼烧激活：每轮：生命回满",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("待激活的凤凰涅槃");
                    return null;
                }),

            new(id:                         "0111",
                name:                       "两仪",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n获得护甲时/施加减甲时：额外+{3 + dj}",
                cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("两仪", 3 + skill.Dj);
                    return null;
                }),

            new(id:                         "0519",
                name:                       "天人合一",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.XiaoHao,
                cost:                       CostResult.ManaFromDj(dj => 5 - 2 * dj),
                costDescription:            CostDescription.ManaFromDj(dj => 5 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗\n" +
                    $"激活所有架势",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("天人合一");
                    return null;
                }),

            new(id:                         "0521",
                name:                       "那由他",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.XiaoHao,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"20柔韧觉醒：没有耗蓝阶段，Step阶段无法受影响，所有Buff层数不会再变化",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("待激活的那由他");
                    return null;
                }),

            #endregion
            
            #region 待选池子
            
            new(id:                         "0101",
                name:                       "乘风",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
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
                jingJieBound:               JingJie.ZhuJi2HuaShen,
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
                        await caster.GainArmorProcedure(value, induced: false);
                    return Style.CastResultFromOddEven(odd, even);
                }),

            new(id:                         "0105",
                name:                       "盘旋",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         null,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{4 + 2 * dj}\n施加{4 + 2 * dj}减甲",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(4 + 2 * skill.Dj, induced: false);
                    await caster.RemoveArmorProcedure(4 + 2 * skill.Dj);
                    return null;
                }),

            new(id:                         "0115",
                name:                       "流云",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromDj(dj => 2 - ((dj + 1) / 2)),
                costDescription:            CostDescription.ManaFromDj(dj => 2 - ((dj + 1) / 2)),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{7 + 3 * (dj / 2)}攻\n" +
                    $"击伤：下回合，{7 + 3 * (dj / 2)}攻".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    int val = 7 + 3 * (skill.Dj / 2);
                    await caster.AttackProcedure(val,
                        wuXing: skill.Entry.WuXing,
                        didDamage: async d =>
                        {
                            await caster.GainBuffProcedure("延迟攻", val);
                            cond = true;
                        });
                    return cond.ToCastResult();
                }),

            new(id:                         "0107",
                name:                       "飞絮",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
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
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.LingQi | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗" +
                    $"\n击伤时：灵气+1",
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
                jingJieBound:               JingJie.YuanYing2HuaShen,
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
                jingJieBound:               JingJie.HuaShenOnly,
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
                jingJieBound:               JingJie.HuaShenOnly,
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
                jingJieBound:               JingJie.LianQi2HuaShen,
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
                jingJieBound:               JingJie.LianQi2HuaShen,
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
                jingJieBound:               JingJie.ZhuJi2HuaShen,
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
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{10 + 2 * dj}攻\n" +
                    $"击伤：格挡+1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(10 + 2 * skill.Dj, wuXing: skill.Entry.WuXing,
                        didDamage: async d =>
                        {
                            cond = true;
                            await caster.GainBuffProcedure("格挡");
                        });
                    return cond.ToCastResult();
                }),

            new(id:                         "0207",
                name:                       "勤能补拙",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{10 + 4 * dj}\n" +
                    $"初次：遭受1跳回合".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(10 + 4 * skill.Dj, induced: false);
                    bool cond = !skill.IsFirstTime || await caster.IsFocused();
                    if (!cond)
                        await caster.GainBuffProcedure("跳回合");
                    return cond.ToCastResult();
                }),

            new(id:                         "0208",
                name:                       "秋水",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
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
                jingJieBound:               JingJie.JinDan2HuaShen,
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
                        didDamage: d => caster.GainBuffProcedure("格挡", d.Value / (8 - skill.Dj)));
                    await caster.CycleProcedure(WuXing.Shui);
                    return null;
                }),

            new(id:                         "0211",
                name:                       "观棋烂柯",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
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
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ManaFromDj(dj => 1 - dj),
                costDescription:            CostDescription.ManaFromDj(dj => 1 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"生命+{5 + 5 * dj}\n" +
                    $"下一次使用牌时二动",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.HealProcedure(5 + 5 * skill.Dj, induced: false);
                    await caster.GainBuffProcedure("二动");
                    return null;
                }),
            
            new(id:                         "0314",
                name:                       "旧飞龙在天",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
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
                    await caster.GainBuffProcedure("轮闪避");
                    return null;
                }),
            
            new(id:                         "0304",
                name:                       "旧潜龙在渊",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"生命+{6 + 4 * dj}\n" +
                    $"初次：闪避+1".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.HealProcedure(6 + 4 * skill.Dj, induced: false);
                    bool cond = skill.IsFirstTime || await caster.IsFocused();
                    if (cond)
                        await caster.GainBuffProcedure("闪避");
                    return cond.ToCastResult();
                }),
            
            new(id:                         "0303",
                name:                       "初桃",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}\n" +
                    $"生命+{3 + dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    await caster.HealProcedure(3 + skill.Dj, induced: false);
                    return null;
                }),

            new(id:                         "0305",
                name:                       "早春",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
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
                    await caster.GainArmorProcedure((6 + skill.Dj) * mul, induced: false);
                    return cond.ToCastResult();
                }),

            new(id:                         "0307",
                name:                       "回马枪",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
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
                jingJieBound:               JingJie.JinDan2HuaShen,
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
                jingJieBound:               JingJie.YuanYing2HuaShen,
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
                jingJieBound:               JingJie.HuaShenOnly,
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
                jingJieBound:               JingJie.LianQi2HuaShen,
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
                jingJieBound:               JingJie.LianQi2HuaShen,
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
                jingJieBound:               JingJie.LianQi2HuaShen,
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
                jingJieBound:               JingJie.ZhuJi2HuaShen,
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

            new(id:                         "0412",
                name:                       "燃灯留烬",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{6 + 2 * dj}\n" +
                    $"每1被消耗卡：多{6 + 2 * dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure((caster.ExhaustedCount + 1) * (6 + 2 * skill.Dj), induced: false);
                    return null;
                }),

            new(id:                         "0502",
                name:                       "土墙",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}\n" +
                    $"护甲+{3 + dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    await caster.GainArmorProcedure(3 + skill.Dj, induced: false);
                    return null;
                }),

            new(id:                         "0503",
                name:                       "地龙",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
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
                        didDamage: async d =>
                        {
                            cond = true;
                            await caster.GainArmorProcedure(value, induced: true);
                        });
                    return cond.ToCastResult();
                }),

            new(id:                         "0504",
                name:                       "铁骨",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
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
                jingJieBound:               JingJie.JinDan2HuaShen,
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
                jingJieBound:               JingJie.JinDan2HuaShen,
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

            new(id:                         "0508",
                name:                       "软剑",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
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
                        didDamage: async d =>
                        {
                            cond = true;
                            await caster.GiveBuffProcedure("缠绕");
                        });
                    return cond.ToCastResult();
                }),

            new(id:                         "0512",
                name:                       "收刀",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
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
                jingJieBound:               JingJie.YuanYing2HuaShen,
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
                jingJieBound:               JingJie.YuanYing2HuaShen,
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
                jingJieBound:               JingJie.YuanYing2HuaShen,
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
                jingJieBound:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"每1灼烧，护甲+2",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int stack = caster.GetStackOfBuff("灼烧");
                    await caster.GainArmorProcedure(2 * stack, induced: false);
                    return null;
                }),

            #endregion

            #region 事件牌

            new(id:                         "0600",
                name:                       "一念",
                wuXing:                     null,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
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
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                cost:                       CostResult.ChannelFromValue(3),
                costDescription:            CostDescription.ChannelFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"治疗{18 + dj * 6}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.HealProcedure(18 + skill.Dj * 6, induced: false);
                    return null;
                }),

            new(id:                         "0602",
                name:                       "百草集",
                wuXing:                     null,
                jingJieBound:               JingJie.YuanYing2HuaShen,
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
                jingJieBound:               JingJie.JinDan2HuaShen,
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
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"获得{2 + dj}集中",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainBuffProcedure("集中", 2 + skill.Dj, false);
                    return null;
                }),

            new(id:                         "0605",
                name:                       "射落金乌",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"5攻x4",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(5, times: 4, wuXing: skill.Entry.WuXing);
                    return null;
                }),

            new(id:                         "0606",
                name:                       "春雨",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"双方生命+{20 + 5 * dj}",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.HealProcedure(20 + 5 * skill.Dj, induced: false);
                    await caster.HealOppoProcedure(20 + 5 * skill.Dj, induced: false);
                    return null;
                }),

            new(id:                         "0607",
                name:                       "枯木",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
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
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"玄武吐息法",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    return null;
                }),

            new(id:                         "0609",
                name:                       "毒性",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加3内伤",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GiveBuffProcedure("内伤", 3);
                    return null;
                }),

            #endregion

            #region 机关牌

            // 筑基
            
            new(id:                         "0700", // 香
                name:                       "醒神香", // 香
                wuXing:                     null,
                jingJieBound:               JingJie.ZhuJiOnly,
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
                jingJieBound:               JingJie.ZhuJiOnly,
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
                jingJieBound:               JingJie.ZhuJiOnly,
                skillTypeComposite:         SkillType.SunHao,
                castDescription:            (j, dj, costResult, castResult) => "护甲+12",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(12, induced: false);
                    return null;
                }),
            
            new(id:                         "0703", // 轮
                name:                       "滑索", // 轮
                wuXing:                     null,
                jingJieBound:               JingJie.ZhuJiOnly,
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
                jingJieBound:               JingJie.YuanYingOnly,
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
                jingJieBound:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.LingQi | SkillType.LingQi,
                castDescription:            (j, dj, costResult, castResult) => "10攻\n击伤：灵气+1，对手灵气-1",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.AttackProcedure(10,
                        didDamage: async d =>
                        {
                            await caster.GainBuffProcedure("灵气");
                            await caster.RemoveBuffProcedure("灵气");
                        });
                    return null;
                }),
            
            new(id:                         "0706", // 香匣
                name:                       "防护罩", // 香匣
                wuXing:                     null,
                jingJieBound:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao,
                castDescription:            (j, dj, costResult, castResult) => "护甲+8\n每有1灵气，护甲+4",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    int add = caster.GetStackOfBuff("灵气");
                    await caster.GainArmorProcedure(8 + add, induced: false);
                    return null;
                }),
            
            new(id:                         "0707", // 香轮
                name:                       "能量饮料", // 香轮
                wuXing:                     null,
                jingJieBound:               JingJie.YuanYingOnly,
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
                jingJieBound:               JingJie.YuanYingOnly,
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
                jingJieBound:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) => "护甲+12\n10攻",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(12, induced: false);
                    await caster.AttackProcedure(10);
                    return null;
                }),
            
            new(id:                         "0710", // 刃轮
                name:                       "铁陀螺", // 刃轮
                wuXing:                     null,
                jingJieBound:               JingJie.YuanYingOnly,
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
                jingJieBound:               JingJie.YuanYingOnly,
                skillTypeComposite:         SkillType.SunHao,
                castDescription:            (j, dj, costResult, castResult) => "护甲+20\n柔韧+2",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    await caster.GainArmorProcedure(20, induced: false);
                    await caster.GainBuffProcedure("柔韧", 2);
                    return null;
                }),
            
            new(id:                         "0712", // 匣轮
                name:                       "不倒翁", // 匣轮
                wuXing:                     null,
                jingJieBound:               JingJie.YuanYingOnly,
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
                jingJieBound:               JingJie.YuanYingOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
                skillTypeComposite:         SkillType.SunHao | SkillType.XiaoHao,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) => "没有效果",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive) =>
                {
                    return null;
                }),
            
            new(id:                         "0720", // 刃刃香
                name:                       "弩炮", // 刃刃香
                wuXing:                     null,
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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
                jingJieBound:               JingJie.FanXuOnly,
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

            #endregion
        });
    }

    public void Init()
    {
        List.Do(entry => entry.GenerateAnnotations());
    }

    public override SkillEntry DefaultEntry() => this["0000"];
}
