
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                    "卡池已空",
                withinPool:                 false),

            new(id:                         "0001",
                name:                       "聚气术",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQiOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    "灵气+1".ApplyMana(),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("灵气");
                }),
            
            new(id:                         "0002",
                name:                       "冲撞",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQiOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"4攻".ApplyAttack(),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(4);
                }),
            
            new(id:                         "0003",
                name:                       "点水",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(3 + dj) * 2}攻".ApplyAttack() + " 吸血".ApplyHeal(),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    int value = Fib.ToValue(3 + skill.Dj) * 2;
                    await caster.AttackProcedure(value, lifeSteal: true, wuXing: skill.Entry.WuXing);
                }),

            #endregion

            #region 标准牌

            new(id:                         "0101",
                name:                       "金刃",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(4 + dj)}攻".ApplyAttack() +
                    $"\n施加{Fib.ToValue(3 + dj)}减甲",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(Fib.ToValue(4 + skill.Dj), wuXing: skill.Entry.WuXing);
                    await caster.RemoveArmorProcedure(Fib.ToValue(3 + skill.Dj));
                }),

            new(id:                         "0102",
                name:                       "寻猎",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n击伤：施加{5 + 5 * dj}破甲".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(2,
                        didDamage: async d =>
                        {
                            await caster.RemoveArmorProcedure(5 + 5 * skill.Dj);
                            cond = true;
                        },
                        wuXing: skill.Entry.WuXing);

                    castResult.AppendCond(cond);
                }),
            
            new(id:                         "0115",
                name:                       "山风",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                cost:                       CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    (j == JingJie.LianQi ? "" : $"护甲+{5 * dj}\n".ApplyDefend()) +
                    (j <= JingJie.JinDan ? "锋锐+1" : $"每{30 - 5 * dj}护甲，锋锐+1"),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    if (skill.GetJingJie() != JingJie.LianQi)
                        await caster.GainArmorProcedure(5 * skill.Dj, induced: false);

                    int gain = (skill.GetJingJie() <= JingJie.JinDan) ? 1 : caster.Armor / (30 - 5 * skill.Dj);
                    await caster.CycleProcedure(WuXing.Jin, gain);
                }),

            new(id:                         "0104",
                name:                       "起势",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"4攻".ApplyAttack() +
                    ($"\n击伤：" + $"灵气+{1 + dj}".ApplyMana()).ApplyCond(castResult) +
                    $"\n开局：" + $"灵气+{1 + dj}".ApplyMana(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(4,
                        didDamage: async d =>
                        {
                            await caster.GainBuffProcedure("灵气", 1 + skill.Dj, induced: true);
                            cond = true;
                        },
                        wuXing: skill.Entry.WuXing);
                    castResult.AppendCond(cond);
                },
                startStageCast:             async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj, induced: false);
                }),

            new(id:                         "0105",
                name:                       "杀意",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{6 + 3 * dj}攻".ApplyAttack() +
                    $"\n击伤：暴击+1".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(6 + 3 * skill.Dj,
                        didDamage: async d =>
                        {
                            await caster.GainBuffProcedure("暴击", 1, induced: true);
                            cond = true;
                        },
                        wuXing: skill.Entry.WuXing);
                    castResult.AppendCond(cond);
                }),

            // new(id:                         "0107",
            //     name:                       "追击",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     cost:                       CostResult.ManaFromDj(dj => 2 - ((dj + 1) / 2)),
            //     costDescription:            CostDescription.ManaFromDj(dj => 2 - ((dj + 1) / 2)),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{3 + dj}攻 每携带1金：多{3 + dj}",
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         int count = caster.CountSuch(s => s.Entry.WuXing == WuXing.Jin);
            //         await caster.AttackProcedure(count * (3 + skill.Dj), wuXing: skill.Entry.WuXing);
            //         return null;
            //     }),
            
            new(id:                         "0108",
                name:                       "刺穴",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+8".ApplyMana() + " 二动" +
                    $"\n遭受{5 - dj}滞气".ApplyDebuff(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("灵气", 8);
                    caster.SetActionPoint(2);
                    await caster.GainBuffProcedure("滞气", 5 - skill.Dj);
                }),

            new(id:                         "0109",
                name:                       "灵动",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{6 + 4 * dj}攻".ApplyAttack() +
                    $"\n敌方有破甲：追加{6 + 4 * dj}攻".ApplyStyle(castResult, "cond"),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond = await caster.OppoHasFragile(useFocus: true);
                    await caster.AttackProcedure(6 + 4 * skill.Dj, wuXing: skill.Entry.WuXing);
                    cond = cond || await caster.OppoHasFragile(useFocus: true);
                    if (cond)
                        await caster.AttackProcedure(6 + 4 * skill.Dj, wuXing: skill.Entry.WuXing);
                    castResult.AppendCond(cond);
                }),

            new(id:                         "0111",
                name:                       "凛冽",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                cost:                       CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"锋锐+{1 + dj}" +
                    $"\n每1锋锐，多1攻" +
                    $"\n吸血".ApplyHeal(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("锋锐", 1 + skill.Dj);
                    await caster.CycleProcedure(WuXing.Jin);
                    int stack = caster.GetStackOfBuff("锋锐");
                    await caster.AttackProcedure(stack, lifeSteal: true, wuXing: skill.Entry.WuXing);
                }),

            new(id:                         "0110",
                name:                       "天地同寿",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加{5 + 5 * dj}破甲" +
                    $"\n自身每1破甲，多1" +
                    $"\n开局：施加{5 + 5 * dj}破甲",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    int fragile = -caster.Armor.ClampUpper(0);
                    int value = 5 + 5 * skill.Dj + fragile;
                    await caster.RemoveArmorProcedure(value);
                },
                startStageCast:             async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.RemoveArmorProcedure(5 + 5 * skill.Dj);
                }),

            new(id:                         "0113",
                name:                       "无妄",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{3 + 3 * dj}攻x2".ApplyAttack() +
                    $"\n敌方有破甲：暴击".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond = await caster.OppoHasFragile(useFocus: true);
                    await caster.AttackProcedure(3 + 3 * skill.Dj, crit: cond, wuXing: skill.Entry.WuXing);
                    cond = cond || await caster.OppoHasFragile(useFocus: true);
                    await caster.AttackProcedure(3 + 3 * skill.Dj, crit: cond, wuXing: skill.Entry.WuXing);
                    castResult.AppendCond(cond);
                }),

            new(id:                         "0114",
                name:                       "素弦",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n灵气+2".ApplyMana() +
                    $"\n下{1 + dj}次攻击也触发",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(2, wuXing: skill.Entry.WuXing);
                    await caster.GainBuffProcedure("灵气", 2);
                    await caster.GainBuffProcedure("素弦", 1 + skill.Dj);
                }),

            new(id:                         "0116",
                name:                       "袖里乾坤",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                cost:                       CostResult.ManaFromDj(dj => dj + 1),
                costDescription:            CostDescription.ManaFromDj(dj => dj + 1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+12".ApplyDefend() + $" 暴击+{1 + dj}" +
                    $"\n开局：暴击+{1 + dj}",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainArmorProcedure(12, induced: false);
                    await caster.GainBuffProcedure("暴击", 1 + skill.Dj);
                },
                startStageCast:             async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("暴击", 1 + skill.Dj);
                }),

            new(id:                         "0112",
                name:                       "凝水",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"锋锐+{1 + dj}" +
                    $"\n每1锋锐，灵气+1".ApplyMana(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.CycleProcedure(WuXing.Jin, gain: 1 + skill.Dj);
                    int stack = caster.GetStackOfBuff("锋锐");
                    await caster.GainBuffProcedure("灵气", stack, induced: true);
                }),

            new(id:                         "0117",
                name:                       "一莲托生",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"暴击+2" + $" 施加10破甲" +
                    ($"\n终结：" + "1攻".ApplyAttack() + " 暴击释放").ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("暴击", 2);
                    await caster.RemoveArmorProcedure(10);
                    
                    bool cond = await skill.IsEnd();
                    if (cond)
                    {
                        int critStack = caster.GetStackOfBuff("暴击");
                        await caster.TryConsumeProcedure("暴击", critStack);
                        await caster.AttackProcedure(1, wuXing: skill.Entry.WuXing,
                            wilDamage: async d => d.Value *= 1 + critStack);
                    }

                    // Func<DamageDetails, Task> wilDamage = async d =>
                    // {
                    //     int critStack = caster.GetStackOfBuff("暴击");
                    //     await caster.TryConsumeProcedure("暴击", critStack);
                    //     d.Value *= 1 + critStack;
                    // };

                    castResult.AppendCond(cond);
                }),

            new(id:                         "0118",
                name:                       "敛息",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"30攻".ApplyAttack() +
                    $"\n击伤：伤害转为破甲".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(30, wuXing: skill.Entry.WuXing,
                        wilDamage: async d =>
                        {
                            await caster.RemoveArmorProcedure(d.Value);
                            d.Cancel = true;
                            cond = true;
                        });
                    castResult.AppendCond(cond);
                }),
            
            new(id:                         "0119",
                name:                       "停云",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"锋锐+3" +
                    $"\n开局：锋锐+1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.CycleProcedure(WuXing.Jin, gain: 3);
                },
                startStageCast:             async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.CycleProcedure(WuXing.Jin, gain: 1);
                }),
            
            new(id:                         "0120",
                name:                       "弹指",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+4".ApplyMana() +
                    $"\n消耗1暴击：翻倍".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond = await caster.TryConsumeProcedure("暴击");
                    int bitShift = cond ? 1 : 0;
                    await caster.GainBuffProcedure("灵气", 4 << bitShift);
                    castResult.AppendCond(cond);
                }),
            
            new(id:                         "0201",
                name:                       "恋花",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(3 + dj) * 2}攻".ApplyAttack() + " 吸血".ApplyHeal(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    int value = Fib.ToValue(3 + skill.Dj) * 2;
                    await caster.AttackProcedure(value, lifeSteal: true, wuXing: skill.Entry.WuXing);
                }),
            
            new(id:                         "0203",
                name:                       "流霰",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{2 + 4 * dj}攻".ApplyAttack() +
                    (j < JingJie.JinDan ?
                        ($"\n击伤：" + "格挡+1".ApplyDefend()).ApplyCond(castResult) :
                        $"\n每造成{10 - dj}点伤害，格挡+1".ApplyDefend()),
                cast:                       async (env, caster, skill, recursive, castResult) =>
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
                        castResult.AppendCond(cond);
                    }
                    else
                    {
                        await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
                            didDamage: d => caster.GainBuffProcedure("格挡", d.Value / (10 - skill.Dj)));
                        await caster.CycleProcedure(WuXing.Shui);
                    }
                }),

            new(id:                         "0204",
                name:                       "吐纳",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{2 + dj}".ApplyMana() +
                    $"\n气血上限+{4 + 4 * dj}" +
                    (j >= JingJie.HuaShen ? "\n治疗可以穿上限" : ""),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("灵气", 2 + skill.Dj);
                    if (skill.GetJingJie() >= JingJie.HuaShen)
                        await caster.GainBuffProcedure("吐纳");
                    // await Procedure
                    caster.MaxHp += 4 + 4 * skill.Dj;
                }),
            
            new(id:                         "0205",
                name:                       "止水",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    (j < JingJie.HuaShen ? $"{8 + 4 * dj}攻".ApplyAttack() : $"28攻".ApplyAttack()) +
                    $"\n未击伤：受到治疗".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
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
                    castResult.AppendCond(cond);
                }),
            
            new(id:                         "0206",
                name:                       "空幻",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                cost:                       async (env, entity, skill, recursive) => new ManaCostResult(4 + skill.Dj - entity.CountSuch(s => s.Entry.WuXing == WuXing.Shui)),
                costDescription:            CostDescription.ManaFromDj(dj => 4 + dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{12 + 6 * dj}攻".ApplyAttack() + " 吸血".ApplyHeal() +
                    $"\n每携带1水：消耗-1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(12 + 6 * skill.Dj, lifeSteal: true, wuXing: skill.Entry.WuXing);
                }),
            
            new(id:                         "0208",
                name:                       "踏浪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                cost:                       CostResult.ManaFromDj(dj => 3 + dj),
                costDescription:            CostDescription.ManaFromDj(dj => 3 + dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{7 + 2 * dj}".ApplyMana(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("灵气", 7 + 2 * skill.Dj);
                }),
            
            new(id:                         "0209",
                name:                       "写意",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ManaFromDj(dj => 2 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{10 + 4 * dj}攻".ApplyAttack() +
                    $"\n返还" +
                    $"暴击".ApplyStyle(castResult, "0") +
                    $"/" +
                    $"吸血".ApplyStyle(castResult, "1") +
                    $"/" +
                    $"穿透".ApplyStyle(castResult, "2"),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(10 + 4 * skill.Dj, wuXing: skill.Entry.WuXing);
                    
                    if (caster.TriggeredCrit)
                        await caster.GainBuffProcedure("暴击", induced: true);
                    if (caster.TriggeredLifesteal)
                        await caster.GainBuffProcedure("吸血", induced: true);
                    if (caster.TriggeredPenetrate)
                        await caster.GainBuffProcedure("穿透", induced: true);
                    
                    castResult.AppendBools(caster.TriggeredCrit, caster.TriggeredLifesteal, caster.TriggeredPenetrate);
                }),
            
            new(id:                         "0211",
                name:                       "瑞雪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Defend | SkillType.Health,
                cost:                       CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"格挡+{1 + dj}".ApplyDefend() +
                    $"\n每1格挡，气血+2".ApplyHeal(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("格挡", 1 + skill.Dj);
                    await caster.CycleProcedure(WuXing.Shui);
                    int stack = caster.GetStackOfBuff("格挡");
                    await caster.HealProcedure(2 * stack, induced: false);
                }),
            
            new(id:                         "0210",
                name:                       "无念无想",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Health,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"治疗{20 + 10 * dj}".ApplyHeal() +
                    $"\n遭受5缠绕".ApplyDebuff(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.HealProcedure(20 + 10 * skill.Dj, induced: false);
                    await caster.GainBuffProcedure("缠绕", 5);
                }),
            
            new(id:                         "0213",
                name:                       "大鱼",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend | SkillType.Swift,
                cost:                       CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{8 + 8 * dj}攻".ApplyAttack() +
                    $"\n护甲+{8 + 8 * dj}".ApplyDefend() + $" 二动",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(8 + 8 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.GainArmorProcedure(8 + 8 * skill.Dj, induced: false);
                    caster.SetActionPoint(2);
                }),
            
            new(id:                         "0214",
                name:                       "苦寒",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Swift,
                cost:                       CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n二动+1" +
                    $"\n下{1 + dj}次攻击也触发",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(2, wuXing: skill.Entry.WuXing);
                    if (caster.GetActionPoint() < 2)
                        caster.SetActionPoint(2);
                    else
                        await caster.GainBuffProcedure("二动");
                    await caster.GainBuffProcedure("苦寒", 1 + skill.Dj);
                }),
            
            new(id:                         "0220",
                name:                       "奔腾",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Swift,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    (j <= JingJie.YuanYing ? "二动" : "三动"),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    caster.SetActionPoint(skill.GetJingJie() <= JingJie.YuanYing ? 2 : 3);
                }),

            new(id:                         "0216",
                name:                       "气吞山河",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气补至本局最高+{1 + dj}".ApplyMana(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    int space = caster.HighestManaRecord - caster.GetMana() + 1 + skill.Dj;
                    await caster.GainBuffProcedure("灵气", space);
                }),

            new(id:                         "0217",
                name:                       "一梦如是",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                withinPool:                 false,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() +
                    ($"\n击伤：" + "下1次受伤转为治疗".ApplyHeal()).ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond = false;
                    await caster.AttackProcedure(1, skill.Entry.WuXing, didDamage:
                        async d =>
                        {
                            cond = true;
                            await caster.GainBuffProcedure("一梦如是", induced: true);
                        });
                    castResult.AppendCond(cond);
                }),
            
            new(id:                         "0218",
                name:                       "吞天",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                cost:                       CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"15攻".ApplyAttack() + " 暴击" + " 吸血".ApplyHeal(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(15, crit: true, lifeSteal: true, wuXing: skill.Entry.WuXing);
                }),
            
            new(id:                         "0219",
                name:                       "飞鸿踏雪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Defend | SkillType.Swift,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"格挡+1".ApplyDefend() +
                    $"\n二动",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("格挡", 1 + skill.Dj);
                    await caster.CycleProcedure(WuXing.Shui);
                    caster.SetActionPoint(2);
                }),
            
            new(id:                         "0221",
                name:                       "镜花水月",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Mana | SkillType.Health,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗每40生命，灵气+1".ApplyMana() +
                    $"\n消耗每1灵气，生命+10".ApplyHeal(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    int healthConsume = (caster.Hp - 1).ClampLower(0) / 40 * 40;
                    int manaConsume = caster.GetStackOfBuff("灵气");

                    if (healthConsume > 0)
                        await caster.LoseHealthProcedure(healthConsume);
                    if (manaConsume > 0)
                        await caster.LoseBuffProcedure("灵气", manaConsume);

                    int healthGain = manaConsume * 10;
                    int manaGain = healthConsume / 40;

                    if (healthGain > 0)
                        await caster.HealProcedure(healthGain, induced: true);
                    if (manaGain > 0)
                        await caster.GainBuffProcedure("灵气", manaGain, induced: false);
                }),
            
            new(id:                         "0301",
                name:                       "小松",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(4 + dj)}攻".ApplyAttack() + " 穿透" +
                    $"\n成长：多{Fib.ToValue(3 + dj)}攻",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    int value = Fib.ToValue(4 + skill.Dj) + (Fib.ToValue(3 + skill.Dj)) * skill.StageCastedCount;
                    await caster.AttackProcedure(value, penetrate: true, wuXing: skill.Entry.WuXing);
                }),

            new(id:                         "0302",
                name:                       "潜龙在渊",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"闪避+1".ApplyDefend() +
                    $"\n成长{2 + dj}次后：" + $"{(4 + 2 * dj) * (4 + 2 * dj)}攻".ApplyAttack(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    if (skill.StageCastedCount >= 2 + skill.Dj)
                        await caster.AttackProcedure((4 + 2 * skill.Dj) * (4 + 2 * skill.Dj), wuXing: skill.Entry.WuXing);

                    await caster.GainBuffProcedure("闪避");
                }),
            
            new(id:                         "0304",
                name:                       "明神",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}".ApplyMana() +
                    $"\n成长：多1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj + skill.StageCastedCount);
                }),

            new(id:                         "0305",
                name:                       "水滴石穿",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{7 + 3 * dj}攻".ApplyAttack() +
                    $"\n终结：穿透".ApplyStyle(castResult, "0") +
                    $"\n击伤：穿透+1".ApplyStyle(castResult, "1"),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool isEnd = await skill.IsEnd(useFocus: true);
                    bool damaged = false;
                    
                    await caster.AttackProcedure(7 + 3 * skill.Dj, penetrate: isEnd, wuXing: skill.Entry.WuXing,
                        didDamage: async d =>
                        {
                            damaged = true;
                            await caster.GainBuffProcedure("穿透", induced: true);
                        });
                    
                    castResult.AppendBools(isEnd, damaged);
                }),

            new(id:                         "0306",
                name:                       "见龙在田",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                cost:                       async (env, entity, skill, recursive) => new ChannelCostResult(4 - skill.Dj - skill.StageCastedCount),
                costDescription:            CostDescription.ChannelFromDj(dj => 4 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻".ApplyAttack() + " 闪避+2".ApplyDefend() +
                    $"\n成长：吟唱-1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(10, wuXing: skill.Entry.WuXing);
                    await caster.GainBuffProcedure("闪避", 2);
                }),

            new(id:                         "0307",
                name:                       "回春",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Defend | SkillType.Health,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"双方护甲+{10 + 6 * dj}".ApplyDefend() +
                    $"\n双方气血+{10 + 6 * dj}".ApplyHeal(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    int value = 10 + 6 * skill.Dj;
                    await caster.GainArmorProcedure(value, induced: false);
                    await caster.GiveArmorProcedure(value, induced: false);
                    await caster.HealProcedure(value, induced: false);
                    await caster.HealOppoProcedure(value, induced: false);
                }),
            
            new(id:                         "0308",
                name:                       "若竹",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}".ApplyMana() +
                    $"\n穿透+1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj);
                    await caster.GainBuffProcedure("穿透");
                }),

            new(id:                         "0309",
                name:                       "入木三分",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() +
                    $"\n对方每有{4 - dj}护甲，多1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    int value = 1 + caster.Opponent().Armor / (4 - skill.Dj);
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);
                }),

            new(id:                         "0310",
                name:                       "飞龙在天",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"闪避+1".ApplyDefend() +
                    $"\n初次：跳过下{2 + 2 * dj}张牌，成长+1".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("闪避");
                    bool cond = await skill.IsFirstTime();
                    if (cond)
                        await caster.GainBuffProcedure("飞龙在天", 2 + 2 * skill.Dj, induced: true);
                    castResult.AppendCond(cond);
                }),

            new(id:                         "0311",
                name:                       "狂花",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"力量+{2 + dj}" +
                    $"\n每1力量：遭受1腐朽".ApplyDebuff(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("力量", 2 + skill.Dj);
                    await caster.CycleProcedure(WuXing.Mu);
                    int stack = caster.GetStackOfBuff("力量");
                    await caster.GainBuffProcedure("腐朽", stack);
                }),
            
            new(id:                         "0312",
                name:                       "钟声",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"使下{1 + dj}张牌升级",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("钟声", 1 + skill.Dj);
                }),
            
            new(id:                         "0313",
                name:                       "弱昙",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n力量+1" +
                    $"\n下{1 + dj}次攻击也触发",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(2, wuXing: skill.Entry.WuXing);
                    await caster.GainBuffProcedure("力量");
                    await caster.GainBuffProcedure("弱昙", 1 + skill.Dj);
                }),
    
            new(id:                         "0314",
                name:                       "鹤回翔",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Swift,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"交换左右牌" +
                    (j <= JingJie.YuanYing ? $"" :
                        $"\n二动"),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    int leftIndex = skill.Prev(true).SlotIndex;
                    int rightIndex = skill.Next(true).SlotIndex;
                    
                    var tempStageSkill = caster._skills[leftIndex];
                    caster._skills[leftIndex] = caster._skills[rightIndex];
                    caster._skills[rightIndex] = tempStageSkill;

                    var tempIndex = caster._skills[leftIndex].SlotIndex;
                    caster._skills[leftIndex].SlotIndex = caster._skills[rightIndex].SlotIndex;
                    caster._skills[rightIndex].SlotIndex = tempIndex;
                    
                    if (skill.GetJingJie() > JingJie.YuanYing)
                        caster.SetActionPoint(2);
                }),

            new(id:                         "0315",
                name:                       "落英",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"力量+{2 + dj}" +
                    $"\n消耗每{4 - dj}灵气，多1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("力量", 2 + skill.Dj);
                    await caster.CycleProcedure(WuXing.Mu);
                    await caster.TransferProcedure(4 - skill.Dj, "灵气", 1, "力量", true);
                }),

            new(id:                         "0316",
                name:                       "回响",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    (j < JingJie.HuaShen ? $"使用第一张牌\n已疲劳牌无效" : $"使用前两张牌\n已疲劳牌无效"),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    if (!recursive)
                        return;
                    if (!caster._skills[0].Exhausted)
                        await caster.CastProcedure(caster._skills[0], false);
                    
                    if (skill.GetJingJie() >= JingJie.HuaShen)
                        if (!caster._skills[1].Exhausted)
                            await caster.CastProcedure(caster._skills[1], false);
                }),

            new(id:                         "0317",
                name:                       "一叶知秋",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() +
                    $"\n具有所有已触发的攻击描述（未实现）",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(1, wuXing: skill.Entry.WuXing);
                }),

            new(id:                         "0318",
                name:                       "亢龙有悔",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"穿透/力量/" + "闪避".ApplyDefend() + "+3" +
                    $"\n遭受不堪一击".ApplyDebuff(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("穿透", stack: 3, induced: true);
                    await caster.GainBuffProcedure("力量", stack: 3, induced: true);
                    await caster.GainBuffProcedure("闪避", stack: 3, induced: true);
                    await caster.GainBuffProcedure("不堪一击", induced: false);
                }),

            new(id:                         "0319",
                name:                       "刹那芳华",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Defend | SkillType.Exhaust,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"疲劳 力量/" + "闪避".ApplyDefend() + "+1" +
                    $"\n成长：多1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("力量", 1 + skill.StageCastedCount);
                    await caster.GainBuffProcedure("闪避", 1 + skill.StageCastedCount);
                }),

            new(id:                         "0320",
                name:                       "一念无量劫",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗每6灵气，多重+1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.TransferProcedure(6, "灵气", 1, "多重", true, upperBound: 20);
                },
                trivia:"在个人量子超算还没普及的时代，凡人只能体验个二十劫意思意思"),
            
            new(id:                         "0401",
                name:                       "云袖",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{2 + 2 * dj}攻x2".ApplyAttack() +
                    $"\n护甲+{2 + 2 * dj}".ApplyDefend(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    int value = 2 + 2 * skill.Dj;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing, 2);
                    await caster.GainArmorProcedure(value, induced: false);
                }),

            new(id:                         "0402",
                name:                       "轰天",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.HealthFromDj(dj => Fib.ToValue(4 + dj)),
                costDescription:            CostDescription.HealthFromDj(dj => Fib.ToValue(4 + dj)),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(6 + dj)}攻".ApplyAttack(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(Fib.ToValue(6 + skill.Dj), skill.Entry.WuXing);
                }),

            new(id:                         "0403",
                name:                       "正念",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                cost:                       CostResult.ChannelFromDj(dj => 5 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"疲劳" +
                    $"\n护甲+{10 + 10 * dj}".ApplyDefend(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainArmorProcedure(10 + 10 * skill.Dj, induced: false);
                }),

            new(id:                         "0404",
                name:                       "拂晓",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend | SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{3 + 2 * dj}".ApplyDefend() +
                    $"\n灵气+{1 + dj / 2}".ApplyMana(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainArmorProcedure(3 + 2 * skill.Dj, induced: false);
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj / 2);
                }),

            new(id:                         "0405",
                name:                       "九射",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{7 + dj}攻x{3 + 2 * dj}".ApplyAttack(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(7 + skill.Dj, times: 3 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                }),

            new(id:                         "0406",
                name:                       "不动明王诀",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(2 + dj) * 10}攻".ApplyAttack() +
                    "\n成为残血",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(Fib.ToValue(2 + skill.Dj) * 10, wuXing: skill.Entry.WuXing);
                    await caster.BecomeLowHealth();
                }),

            new(id:                         "0407",
                name:                       "浴火",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                cost:                       CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灼烧+{1 + dj}" +
                    $"\n每1灼烧，净化1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("灼烧", 1 + skill.Dj);
                    await caster.CycleProcedure(WuXing.Huo);
                    int stack = caster.GetStackOfBuff("灼烧");
                    await caster.DispelProcedure(stack);
                }),
            
            new(id:                         "0408",
                name:                       "舍生",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                cost:                       async (env, entity, skill, recursive) =>
                    new HealthCostResult(await entity.JiaShiProcedure() ? 0 : 8),
                costDescription:            CostDescription.HealthFromValue(8),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{4 + dj}".ApplyMana() +
                    $"\n架势：免除消耗",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("灵气", 4 + skill.Dj);
                }),
            
            new(id:                         "0409",
                name:                       "天衣无缝",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+2".ApplyDefend() +
                    $"\n直到使用攻击牌：" + $"每回合{3 + 2 * dj}攻".ApplyAttack(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainArmorProcedure(2, false);
                    await caster.GainBuffProcedure("天衣无缝", 3 + skill.Dj * 2);
                }),

            new(id:                         "0410",
                name:                       "登宝塔",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Exhaust | SkillType.Defend,
                cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"疲劳" +
                    $"\n护甲+6".ApplyDefend() +
                    $"\n每1张已疲劳牌，多6",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainArmorProcedure(6 * (1 + caster.ExhaustedCount), induced: false);
                }),

            new(id:                         "0411",
                name:                       "燎原",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灼烧+{dj}" +
                    $"\n成长:多1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("灼烧", skill.Dj + skill.StageCastedCount);
                }),

            new(id:                         "0412",
                name:                       "坐忘",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Mana | SkillType.Exhaust,
                cost:                       CostResult.ChannelFromDj(dj => 3 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 3 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"疲劳" + $" 免费+1".ApplyMana() +
                    $"\n每1张已疲劳牌，多1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("免费", 1 + caster.ExhaustedCount);
                }),

            new(id:                         "0413",
                name:                       "剑王行",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻x{4 + 2 * dj}".ApplyAttack(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(1, wuXing: skill.Entry.WuXing, times: 4 + 2 * skill.Dj);
                }),
            
            new(id:                         "0414",
                name:                       "狂焰",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n追加12攻".ApplyAttack() +
                    $"\n下{1 + dj}次攻击也触发",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(2, wuXing: skill.Entry.WuXing);
                    await caster.AttackProcedure(12, wuXing: skill.Entry.WuXing);
                    await caster.GainBuffProcedure("狂焰", 1 + skill.Dj);
                }),

            new(id:                         "0415",
                name:                       "观众生",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Exhaust,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"使下{1 + dj}张非攻击卡疲劳",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("观众生", 1 + skill.Dj);
                }),

            new(id:                         "0416",
                name:                       "晚霞",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                skillTypeComposite:         SkillType.Exhaust | SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"疲劳" +
                    $"\n灵气+8".ApplyMana(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await skill.ExhaustProcedure();
                    await caster.GainBuffProcedure("灵气", 8);
                }),

            new(id:                         "0417",
                name:                       "一舞惊鸿",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() +
                    $"\n每获得过1闪避，多1次",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(1, times: caster.GainedEvadeRecord + 1, wuXing: skill.Entry.WuXing);
                }),

            new(id:                         "0419",
                name:                       "净天地",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Exhaust,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"疲劳" +
                    $"\n使用所有已疲劳牌",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    if (!recursive)
                        return;
                    
                    await skill.ExhaustProcedure();
                    foreach (StageSkill s in caster._skills)
                    {
                        if (!s.Exhausted)
                            continue;
                        await caster.CastProcedure(s, recursive: false);
                    }
                }),

            new(id:                         "0420",
                name:                       "窑土",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灼烧+3" +
                    $"\n每1灼烧，护甲+2".ApplyDefend(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("灼烧", 3);
                    await caster.CycleProcedure(WuXing.Huo);
                    int stack = caster.GetStackOfBuff("灼烧");
                    await caster.GainArmorProcedure(2 * stack, induced: false);
                }),

            new(id:                         "0501",
                name:                       "寸劲",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(5 + dj) * 2 - 2}攻".ApplyAttack() +
                    $"\n遭受{3 + 2 * dj}软弱".ApplyDebuff() +
                    $"\n架势：免除".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(Fib.ToValue(5 + skill.Dj) * 2 - 2, wuXing: WuXing.Tu);
                    bool cond = await caster.JiaShiProcedure();
                    if (!cond)
                        await caster.GainBuffProcedure("软弱", 3 + 2 * skill.Dj);
                    castResult.AppendCond(cond);
                }),

            new(id:                         "0502",
                name:                       "八极拳",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{Fib.ToValue(4 + dj)}".ApplyDefend() +
                    ($"\n架势：" + "每1护甲，1攻".ApplyAttack()).ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainArmorProcedure(Fib.ToValue(4 + skill.Dj), induced: false);
                    bool cond = await caster.JiaShiProcedure();
                    if (cond)
                        await caster.AttackProcedure(Mathf.Max(1, caster.Armor), wuXing: skill.Entry.WuXing);
                    castResult.AppendCond(cond);
                }),

            new(id:                         "0503",
                name:                       "满招损",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(4 + dj)}攻".ApplyAttack() +
                    $"\n对手有灵气：多{Fib.ToValue(4 + dj)}".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond = caster.Opponent().GetStackOfBuff("灵气") > 0;
                    int gain = cond ? Fib.ToValue(4 + skill.Dj) : 0;
                    await caster.AttackProcedure(Fib.ToValue(4 + skill.Dj) + gain, wuXing: skill.Entry.WuXing);
                    castResult.AppendCond(cond);
                }),
            
            new(id:                         "0504",
                name:                       "流沙",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{3 + 2 * dj}攻".ApplyAttack() +
                    $"\n灵气+{1 + dj / 2}".ApplyMana(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(3 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.GainBuffProcedure("灵气", 1 + skill.Dj / 2);
                }),

            new(id:                         "0505",
                name:                       "一力降十会",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{(5 * dj * dj + 35 * dj + 50) / 2}攻".ApplyAttack() +
                    $"\n遭受4跳回合".ApplyDebuff() +
                    $"\n唯一攻击牌：免除".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    int value = (5 * skill.Dj * skill.Dj + 35 * skill.Dj + 50) / 2;
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);

                    bool cond = skill.NoOtherAttack;
                    if (!cond)
                        await caster.GainBuffProcedure("跳回合", 4);
                    castResult.AppendCond(cond);
                }),
            
            new(id:                         "0506",
                name:                       "边腿",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{8 + 3 * dj}攻".ApplyAttack() +
                    $"\n架势：消耗每1灵气，多{1 + dj}攻".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    int value = 8 + 3 * skill.Dj;
                    
                    bool cond = await caster.JiaShiProcedure();
                    if (cond)
                    {
                        int mana = caster.GetMana();
                        await caster.LoseBuffProcedure("灵气", mana);
                        value += mana * (1 + skill.Dj);
                    }
                    
                    await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);
                    
                    castResult.AppendCond(cond);
                }),
            
            new(id:                         "0521",
                name:                       "连环腿",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"造成本局最高攻".ApplyAttack(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(Mathf.Max(1, caster.HighestAttackRecord), wuXing: skill.Entry.WuXing);
                }),

            new(id:                         "0510",
                name:                       "崩山掌",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{15 + 5 * dj}".ApplyDefend() +
                    $"\n架势：下{1 + dj}次失去护甲时，返还".ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainArmorProcedure(15 + 5 * skill.Dj, induced: false);
                    bool cond = await caster.JiaShiProcedure();
                    if (cond)
                        await caster.GainBuffProcedure("护甲返还", 1 + skill.Dj, induced: true);
                    castResult.AppendCond(cond);
                }),

            new(id:                         "0511",
                name:                       "玉骨",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cost:                       CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"柔韧+{2 + dj}" +
                    $"\n每4柔韧，暴击+1",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("柔韧", 2 + skill.Dj);
                    await caster.CycleProcedure(WuXing.Tu);
                    int stack = caster.GetStackOfBuff("柔韧");
                    int add = stack / 4;
                    await caster.GainBuffProcedure("暴击", add);
                }),

            new(id:                         "0512",
                name:                       "箭疾步",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Mana | SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{2 + dj}".ApplyMana() +
                    ($"\n架势：" + $"每1灵气，护甲+{1 + dj}".ApplyDefend()).ApplyCond(castResult),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("灵气", 2 + skill.Dj);

                    bool cond = await caster.JiaShiProcedure();
                    if (cond)
                        await caster.GainArmorProcedure(caster.GetMana() * (1 + skill.Dj), induced: false);
                    
                    castResult.AppendCond(cond);
                }),

            new(id:                         "0513",
                name:                       "震脚",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{16 + 9 * dj}攻".ApplyAttack() +
                    ($"\n击伤：" + "护甲+击伤值".ApplyDefend()).ApplyStyle(castResult, "0") +
                    $"\n架势：下{1 + dj}次攻击也触发".ApplyStyle(castResult, "1"),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond0 = false;
                    await caster.AttackProcedure(16 + 9 * skill.Dj, wuXing: skill.Entry.WuXing,
                        didDamage: async d =>
                        {
                            cond0 = true;
                            await caster.GainArmorProcedure(d.Value, induced: true);
                        });

                    bool cond1 = await caster.JiaShiProcedure();
                    if (cond1)
                        await caster.GainBuffProcedure("击伤赋予护甲", 1 + skill.Dj);
                    
                    castResult.AppendBools(cond0, cond1);
                }),

            new(id:                         "0514",
                name:                       "孤山",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    (j < JingJie.HuaShen ? $"2攻".ApplyAttack() : "2攻x2".ApplyAttack()) +
                    $"\n不消耗剑阵效果",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond = skill.GetJingJie() < JingJie.HuaShen;
                    int times = cond ? 1 : 2;

                    BuffEntry[] buffs = new BuffEntry[] { "素弦", "苦寒", "弱昙", "狂焰" };

                    foreach (BuffEntry b in buffs)
                        if (caster.GetStackOfBuff(b) > 0)
                            await caster.GainBuffProcedure(b, times, induced: true);
                    
                    await caster.AttackProcedure(2, times: times, wuXing: skill.Entry.WuXing);

                }),

            new(id:                         "0515",
                name:                       "龟息",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ChannelFromDj(dj => 2 + dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 + dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"柔韧+{4 + 5 * dj}",
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("柔韧", 4 + 5 * skill.Dj);
                    await caster.CycleProcedure(WuXing.Tu);
                }),

            new(id:                         "0516",
                name:                       "冰肌",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"柔韧+{3 + dj}" +
                    $"\n遭受{3 + dj}滞气".ApplyDebuff(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("柔韧", 3 + skill.Dj);
                    await caster.CycleProcedure(WuXing.Tu);
                    await caster.GainBuffProcedure("滞气", 3 + skill.Dj);
                }),

            new(id:                         "0517",
                name:                       "一诺五岳",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻 ".ApplyAttack() +
                    $"残血".ApplyStyle(castResult, "0") +
                    $"|" +
                    $"无护甲".ApplyStyle(castResult, "1") +
                    $"|" +
                    $"无灵气".ApplyStyle(castResult, "2") +
                    $"|" +
                    $"吟唱".ApplyStyle(castResult, "3") +
                    $"|" +
                    $"滞气".ApplyStyle(castResult, "4") +
                    $"|" +
                    $"缠绕".ApplyStyle(castResult, "5") +
                    $"|" +
                    $"软弱".ApplyStyle(castResult, "6") +
                    $"|" +
                    $"内伤".ApplyStyle(castResult, "7") +
                    $"|" +
                    $"腐朽".ApplyStyle(castResult, "8") +
                    $"|" +
                    $"架势".ApplyStyle(castResult, "9") +
                    $"|" +
                    $"终结".ApplyStyle(castResult, "10") +
                    $"|" +
                    $"初次".ApplyStyle(castResult, "11") +
                    $"：翻倍",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond0 = caster.IsLowHealth || await caster.IsFocused();
                    bool cond1 = (caster.Armor <= 0) || await caster.IsFocused();
                    bool cond2 = (caster.GetStackOfBuff("灵气") == 0) || await caster.IsFocused();
                    bool cond3 = (caster.HasChannelRecord) || await caster.IsFocused();
                    bool cond4 = (caster.HasZhiQiRecord) || await caster.IsFocused();
                    bool cond5 = (caster.HasChanRaoRecord) || await caster.IsFocused();
                    bool cond6 = (caster.HasRuanRuoRecord) || await caster.IsFocused();
                    bool cond7 = (caster.HasNeiShangRecord) || await caster.IsFocused();
                    bool cond8 = (caster.HasFuXiuRecord) || await caster.IsFocused();
                    bool cond9 = caster.TriggeredJiaShiRecord || await caster.JiaShiProcedure();
                    bool cond10 = caster.TriggeredEndRecord || await skill.IsEnd(useFocus: true);
                    bool cond11 = caster.TriggeredFirstTimeRecord || await skill.IsFirstTime(useFocus: true);

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
                    castResult.AppendBools(cond0, cond1, cond2, cond3, cond4, cond5, cond6, cond7, cond8, cond9, cond10, cond11);
                }),
            
            #endregion

            #region 事件牌

            new(id:                         "0603",
                name:                       "遗憾",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"对手失去{3 + dj}灵气",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.RemoveBuffProcedure("灵气", 3 + skill.Dj, false);
                }),

            new(id:                         "0604",
                name:                       "爱恋",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"获得{2 + dj}集中",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("集中", 2 + skill.Dj, false);
                }),

            new(id:                         "0606",
                name:                       "春雨",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Health,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"双方气血+{20 + 5 * dj}".ApplyHeal(),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.HealProcedure(20 + 5 * skill.Dj, induced: false);
                    await caster.HealOppoProcedure(20 + 5 * skill.Dj, induced: false);
                }),

            new(id:                         "0607",
                name:                       "枯木",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"双方遭受{5 + dj}腐朽".ApplyDebuff(),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GainBuffProcedure("腐朽", 5 + skill.Dj);
                    await caster.GiveBuffProcedure("腐朽", 5 + skill.Dj);
                }),

            new(id:                         "0600",
                name:                       "须臾",
                wuXing:                     null,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Swift,
                cost:                       CostResult.HealthFromDj(dj => 8 - 2 * dj),
                costDescription:            CostDescription.HealthFromDj(dj => 8 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    j <= JingJie.ZhuJi ? "二动" : "三动",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond = skill.GetJingJie() <= JingJie.ZhuJi;
                    caster.SetActionPoint(cond ? 2 : 3);
                }),

            new(id:                         "0601",
                name:                       "永远",
                wuXing:                     null,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Health,
                cost:                       CostResult.ChannelFromValue(3),
                costDescription:            CostDescription.ChannelFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"治疗{18 + dj * 6}".ApplyHeal(),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.HealProcedure(18 + skill.Dj * 6, induced: false);
                }),

            new(id:                         "0610",
                name:                       "童趣",
                wuXing:                     null,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Swift,
                cost:                       CostResult.HealthFromDj(dj => 8 - 2 * dj),
                costDescription:            CostDescription.HealthFromDj(dj => 8 - 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    j <= JingJie.ZhuJi ? "二动" : "三动",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    bool cond = skill.GetJingJie() <= JingJie.ZhuJi;
                    caster.SetActionPoint(cond ? 2 : 3);
                }),

            new(id:                         "0611",
                name:                       "一心",
                wuXing:                     null,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Health,
                cost:                       CostResult.ChannelFromValue(3),
                costDescription:            CostDescription.ChannelFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"治疗{18 + dj * 6}".ApplyHeal(),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.HealProcedure(18 + skill.Dj * 6, induced: false);
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
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    BuffEntry[] buffEntries = new BuffEntry[] { "锋锐", "格挡", "力量", "闪避", "灼烧", };

                    foreach (BuffEntry buffEntry in buffEntries)
                    {
                        Buff b = caster.FindBuff(buffEntry);
                        if (b != null)
                            await caster.GainBuffProcedure(b.GetEntry(), 1 + skill.Dj);
                    }

                }),

            new(id:                         "0605",
                name:                       "射落金乌",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"5攻x4".ApplyAttack(),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(5, times: 4, wuXing: skill.Entry.WuXing);
                }),

            new(id:                         "0608",
                name:                       "玄武吐息法",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"玄武吐息法",
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                }),

            new(id:                         "0609",
                name:                       "毒性",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加3内伤".ApplyDebuff(),
                withinPool:                 false,
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.GiveBuffProcedure("内伤", 3);
                }),

            #endregion

            #region 之前的被动们

            // new(id:                         "0108",
            //     name:                       "诸行无常",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromDj(dj => 4 - dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 4 - dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n击伤时：施加{3 + 2 * dj}减甲" +
            //         $"\n每携带1金，层数+1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         int n = caster.CountSuch(stageSkill => stageSkill.Entry.WuXing == WuXing.Jin);
            //         await caster.GainBuffProcedure("诸行无常", n + 3 + 2 * skill.Dj);
            //         return null;
            //     }),
            //
            // new(id:                         "0121",
            //     name:                       "齐物论",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n奇偶同时激活两个效果",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("齐物论");
            //         return null;
            //     }),
            //
            // new(id:                         "0122",
            //     name:                       "人间无戈",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n20锋锐觉醒：死亡不会导致Stage结算",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("待激活的人间无戈");
            //         return null;
            //     }),
            //
            // new(id:                         "0406",
            //     name:                       "抱朴",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust | SkillType.Mana,
            //     cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n每回合：灵气+1" +
            //         $"\n唯一灵气牌：多1层".ApplyCond(castResult),
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         bool cond = skill.NoOtherLingQi;
            //         await caster.GainBuffProcedure("抱朴", cond ? 2 : 1);
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "0214",
            //     name:                       "幻月狂乱",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳 所有攻击具有吸血" +
            //         $"\n回合内未攻击：遭受1跳回合",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("幻月狂乱");
            //         return null;
            //     }),
            //
            // new(id:                         "0217",
            //     name:                       "摩诃钵特摩",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         SkillType.Exhaust | SkillType.Swift,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n20格挡觉醒：连续行动八次，回合结束死亡",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("待激活的摩诃钵特摩");
            //         return null;
            //     }),
            //
            // new(id:                         "0312",
            //     name:                       "盛开",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n受治疗时：力量+1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("盛开");
            //         return null;
            //     }),
            //
            // new(id:                         "0316",
            //     name:                       "心斋",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         SkillType.Mana | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n所有耗蓝-1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("心斋");
            //         return null;
            //     }),
            //
            // new(id:                         "0320",
            //     name:                       "通透世界",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n20力量觉醒：攻击具有穿透",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("待激活的通透世界");
            //         return null;
            //     }),
            //
            // new(id:                         "0408",
            //     name:                       "天衣无缝",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust | SkillType.Attack,
            //     cost:                       CostResult.ChannelFromValue(5),
            //     costDescription:            CostDescription.ChannelFromValue(5),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n每回合：{6 + 2 * dj}攻" +
            //         $"\n卡组无其他攻击牌：多3层".ApplyCond(castResult) +
            //         $"\n无法使用其他方式攻击",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         bool cond = skill.NoOtherAttack;
            //         int stack = 6 + 2 * skill.Dj;
            //         if (cond)
            //             stack += 3;
            //         await caster.GainBuffProcedure("天衣无缝", stack);
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "0410",
            //     name:                       "淬体",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromDj(dj => 5 - dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 5 - dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n受伤时：灼烧+1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("淬体");
            //         return null;
            //     }),
            //
            // new(id:                         "0415",
            //     name:                       "凤凰涅槃",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n20灼烧激活：每轮：气血回满",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("待激活的凤凰涅槃");
            //         return null;
            //     }),
            //
            // new(id:                         "0111",
            //     name:                       "两仪",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n获得护甲时/施加减甲时：额外+{3 + dj}",
            //     cost:                       CostResult.ChannelFromDj(dj => 5 - 2 * dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 5 - 2 * dj),
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("两仪", 3 + skill.Dj);
            //         return null;
            //     }),
            //
            // new(id:                         "0519",
            //     name:                       "天人合一",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     cost:                       CostResult.ManaFromDj(dj => 5 - 2 * dj),
            //     costDescription:            CostDescription.ManaFromDj(dj => 5 - 2 * dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n激活所有架势",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("天人合一");
            //         return null;
            //     }),
            //
            // new(id:                         "0521",
            //     name:                       "那由他",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"20柔韧觉醒：没有耗蓝阶段，Step阶段无法受影响，所有Buff层数不会再变化",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("待激活的那由他");
            //         return null;
            //     }),

            #endregion
            
            #region 待选池子
            
            new(id:                         "0507",
                name:                       "罗刹",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                withinPool:                 false,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{20 + 20 * dj}攻".ApplyAttack() +
                    $"\n遭受{3 + 2 * dj}腐朽".ApplyDebuff(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(20 + 20 * skill.Dj, wuXing: skill.Entry.WuXing);
                    await caster.GainBuffProcedure("腐朽", stack: 3 + 2 * skill.Dj, induced: true);
                }),
            
            new(id:                         "0509",
                name:                       "正域彼四方",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                withinPool:                 false,
                cost:                       CostResult.ManaFromValue(8),
                costDescription:            CostDescription.ManaFromValue(8),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{40 + 20 * dj}攻".ApplyAttack(),
                cast:                       async (env, caster, skill, recursive, castResult) =>
                {
                    await caster.AttackProcedure(40 + 20 * skill.Dj, wuXing: skill.Entry.WuXing);
                }),
            
            // new(id:                         "0322",
            //     name:                       "飞龙在天",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     cost:                       CostResult.ChannelFromDj(dj => dj - 1),
            //     costDescription:            CostDescription.ChannelFromDj(dj => dj - 1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"10攻 闪避+1\n" +
            //         $"初次：重置走步计数".ApplyCond(castResult),
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.AttackProcedure(10, wuXing: WuXing.Mu);
            //         await caster.GainBuffProcedure("闪避");
            //
            //         bool cond = await skill.IsFirstTime();
            //         if (cond)
            //         {
            //             caster._p = 0;
            //             if (!caster._skills[0].Exhausted)
            //                 await caster.GainBuffProcedure("跳走步");
            //         }
            //         
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "0101",
            //     name:                       "乘风",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{5 + dj}攻\n" +
            //         $"若有锋锐：{3 + dj}攻".ApplyCond(castResult),
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         bool cond = caster.GetStackOfBuff("锋锐") > 0 || await caster.IsFocused();
            //         int add = cond ? 3 + skill.Dj : 0;
            //         await caster.AttackProcedure(5 + skill.Dj + add, wuXing: skill.Entry.WuXing);
            //
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "0104",
            //     name:                       "掠影",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"奇偶：" +
            //         $"{5 + 2 * dj}攻".ApplyOdd(castResult) +
            //         $"/" +
            //         $"护甲+{5 + 2 * dj}".ApplyEven(castResult),
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         int value = 5 + 2 * skill.Dj;
            //         bool odd = skill.IsOdd || await caster.IsFocused();
            //         if (odd)
            //             await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing);
            //         bool even = skill.IsEven || await caster.IsFocused();
            //         if (even)
            //             await caster.GainArmorProcedure(value, induced: false);
            //         return Style.CastResultFromOddEven(odd, even);
            //     }),
            //
            // new(id:                         "0115",
            //     name:                       "流云",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     cost:                       CostResult.ManaFromDj(dj => 2 - ((dj + 1) / 2)),
            //     costDescription:            CostDescription.ManaFromDj(dj => 2 - ((dj + 1) / 2)),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{7 + 3 * (dj / 2)}攻\n" +
            //         $"击伤：下回合，{7 + 3 * (dj / 2)}攻".ApplyCond(castResult),
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         bool cond = false;
            //         int val = 7 + 3 * (skill.Dj / 2);
            //         await caster.AttackProcedure(val,
            //             wuXing: skill.Entry.WuXing,
            //             didDamage: async d =>
            //             {
            //                 await caster.GainBuffProcedure("延迟攻", val);
            //                 cond = true;
            //             });
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "0107",
            //     name:                       "飞絮",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         null,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"奇偶：" +
            //         $"施加{8 + 2 * dj}减甲".ApplyOdd(castResult) +
            //         $"/" +
            //         $"锋锐+{1 + dj}".ApplyEven(castResult),
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         bool odd = skill.IsOdd || await caster.IsFocused();
            //         if (odd)
            //             await caster.RemoveArmorProcedure(8 + 2 * skill.Dj);
            //         bool even = skill.IsEven || await caster.IsFocused(); 
            //         if (even)
            //             await caster.GainBuffProcedure("锋锐", 1 + skill.Dj);
            //         return Style.CastResultFromOddEven(odd, even);
            //     }),
            //
            // new(id:                         "0119",
            //     name:                       "追命",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"每1柔韧，施加2减甲",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.RemoveArmorProcedure(2 * caster.GetStackOfBuff("柔韧"));
            //         return null;
            //     }),
            //
            // new(id:                         "0120",
            //     name:                       "千里神行符",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         SkillType.Exhaust | SkillType.Swift,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"奇偶：" +
            //         $"疲劳".ApplyOdd(castResult) +
            //         $"/" +
            //         $"二动".ApplyEven(castResult) +
            //         $"\n灵气+4",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         bool odd = skill.IsOdd || await caster.IsFocused();
            //         if (odd)
            //             await skill.ExhaustProcedure();
            //         bool even = skill.IsEven || await caster.IsFocused();
            //         if (even)
            //             caster.SetActionPoint(2);
            //
            //         await caster.GainBuffProcedure("灵气", 4);
            //         return Style.CastResultFromOddEven(odd, even);
            //     }),
            //
            // new(id:                         "0204",
            //     name:                       "归意",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{10 + 2 * dj}攻\n" +
            //         $"终结：吸血".ApplyCond(castResult),
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         bool cond = await skill.IsEnd(useFocus: true);
            //         await caster.AttackProcedure(10 + 2 * skill.Dj, lifeSteal: cond,
            //             wuXing: skill.Entry.WuXing);
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "0207",
            //     name:                       "勤能补拙",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"护甲+{10 + 4 * dj}\n" +
            //         $"初次：遭受1跳回合".ApplyCond(castResult),
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainArmorProcedure(10 + 4 * skill.Dj, induced: false);
            //         bool cond = !await skill.IsFirstTime();
            //         if (!cond)
            //             await caster.GainBuffProcedure("跳回合");
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "0211",
            //     name:                       "观棋烂柯",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     cost:                       CostResult.ManaFromDj(dj => 1 - dj),
            //     costDescription:            CostDescription.ManaFromDj(dj => 1 - dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"施加1跳回合",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GiveBuffProcedure("跳回合");
            //         return null;
            //     }),
            //
            // new(id:                         "0212",
            //     name:                       "激流",
            //     wuXing:                     WuXing.Shui,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         SkillType.Heal,
            //     cost:                       CostResult.ManaFromDj(dj => 1 - dj),
            //     costDescription:            CostDescription.ManaFromDj(dj => 1 - dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"气血+{5 + 5 * dj}\n" +
            //         $"下一次使用牌时二动",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.HealProcedure(5 + 5 * skill.Dj, induced: false);
            //         await caster.GainBuffProcedure("二动");
            //         return null;
            //     }),
            //
            // new(id:                         "0314",
            //     name:                       "旧飞龙在天",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n每轮：闪避补至1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("轮闪避");
            //         return null;
            //     }),
            //
            // new(id:                         "0307",
            //     name:                       "回马枪",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"下次受攻击时：{12 + 4 * dj}攻",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainBuffProcedure("回马枪", 12 + 4 * skill.Dj);
            //         return null;
            //     }),
            //
            // new(id:                         "0315",
            //     name:                       "二重",
            //     wuXing:                     WuXing.Mu,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     cost:                       CostResult.ChannelFromDj(dj => 1 - dj),
            //     costDescription:            CostDescription.ChannelFromDj(dj => 1 - dj),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"下{1 + dj}张牌使用两次",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainBuffProcedure("二重", 1 + skill.Dj);
            //         return null;
            //     }),
            //
            // new(id:                         "0401",
            //     name:                       "化焰",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{4 + 2 * dj}攻\n" +
            //         $"灼烧+{1 + dj / 2}",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.AttackProcedure(4 + 2 * skill.Dj, wuXing: skill.Entry.WuXing);
            //         await caster.GainBuffProcedure("灼烧", 1 + skill.Dj / 2);
            //         return null;
            //     }),
            //
            // new(id:                         "0405",
            //     name:                       "聚火",
            //     wuXing:                     WuXing.Huo,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n灼烧+{2 + dj}",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("灼烧", 2 + skill.Dj);
            //         return null;
            //     }),
            //
            // new(id:                         "0503",
            //     name:                       "地龙",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{7 + 2 * dj}攻\n" +
            //         $"击伤：护甲+{7 + 2 * dj}",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         int value = 7 + 2 * skill.Dj;
            //         bool cond = false;
            //         await caster.AttackProcedure(value, wuXing: skill.Entry.WuXing,
            //             didDamage: async d =>
            //             {
            //                 cond = true;
            //                 await caster.GainArmorProcedure(value, induced: true);
            //             });
            //         return cond.ToCastResult();
            //     }),
            //
            // new(id:                         "0504",
            //     name:                       "铁骨",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         SkillType.Exhaust,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"疲劳" +
            //         $"\n柔韧+{1 + (1 + dj) / 2}",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("柔韧", 1 + (1 + skill.Dj) / 2);
            //         return null;
            //     }),
            //
            // new(id:                         "0505",
            //     name:                       "点星",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.JinDan2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{8 + 2 * dj}攻\n" +
            //         $"相邻牌都非攻击：翻倍".ApplyStyle(castResult, "0") +
            //         $"\n" +
            //         $"消耗1灵气：翻倍".ApplyStyle(castResult, "1"),
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         bool cond0 = skill.NoAttackAdjacents || await caster.IsFocused();
            //         bool cond1 = await caster.TryConsumeProcedure("灵气") || await caster.IsFocused();
            //         int bitShift = 0;
            //         bitShift += cond0 ? 1 : 0;
            //         bitShift += cond1 ? 1 : 0;
            //         await caster.AttackProcedure((8 + 2 * skill.Dj) << bitShift);
            //         return Style.CastResultFromBools(cond0, cond1);
            //     }),
            //
            // new(id:                         "0517",
            //     name:                       "铁布衫",
            //     wuXing:                     WuXing.Tu,
            //     jingJieBound:               JingJie.YuanYing2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"1次，受伤时：护甲+[受伤值]，" +
            //         $"架势：2次".ApplyCond(castResult),
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         bool cond = await caster.ToggleJiaShiProcedure();
            //         int stack = cond ? 2 : 1;
            //         await caster.GainBuffProcedure("铁布衫", stack);
            //         return cond.ToCastResult();
            //     }),

            #endregion

            #region 机关牌

            // // 筑基
            //
            // new(id:                         "0700", // 香
            //     name:                       "醒神香", // 香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.ZhuJiOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Mana,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n灵气+4",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainBuffProcedure("灵气", 4);
            //         return null;
            //     }),
            //
            // new(id:                         "0701", // 刃
            //     name:                       "飞镖", // 刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.ZhuJiOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n12攻",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.AttackProcedure(12);
            //         return null;
            //     }),
            //
            // new(id:                         "0702", // 匣
            //     name:                       "铁匣", // 匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.ZhuJiOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+12",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainArmorProcedure(12, induced: false);
            //         return null;
            //     }),
            //
            // new(id:                         "0703", // 轮
            //     name:                       "滑索", // 轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.ZhuJiOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Swift | SkillType.Exhaust,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n三动 疲劳",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         caster.SetActionPoint(3);
            //         await skill.ExhaustProcedure();
            //         return null;
            //     }),
            //
            // // 元婴
            //
            // new(id:                         "0704", // 香香
            //     name:                       "还魂香", // 香香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Mana,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n灵气+8",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainBuffProcedure("灵气", 8);
            //         return null;
            //     }),
            //
            // new(id:                         "0705", // 香刃
            //     name:                       "净魂刀", // 香刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Mana | SkillType.Mana,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n10攻\n击伤：灵气+1，对手灵气-1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.AttackProcedure(10,
            //             didDamage: async d =>
            //             {
            //                 await caster.GainBuffProcedure("灵气");
            //                 await caster.RemoveBuffProcedure("灵气");
            //             });
            //         return null;
            //     }),
            //
            // new(id:                         "0706", // 香匣
            //     name:                       "防护罩", // 香匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+8\n每有1灵气，护甲+4",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         int add = caster.GetStackOfBuff("灵气");
            //         await caster.GainArmorProcedure(8 + add, induced: false);
            //         return null;
            //     }),
            //
            // new(id:                         "0707", // 香轮
            //     name:                       "能量饮料", // 香轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Mana,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n下1次灵气减少时，加回",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainBuffProcedure("灵气回收");
            //         return null;
            //     }),
            //
            // new(id:                         "0708", // 刃刃
            //     name:                       "炎铳", // 刃刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n25攻",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.AttackProcedure(25);
            //         return null;
            //     }),
            //
            // new(id:                         "0709", // 刃匣
            //     name:                       "机关人偶", // 刃匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+12\n10攻",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainArmorProcedure(12, induced: false);
            //         await caster.AttackProcedure(10);
            //         return null;
            //     }),
            //
            // new(id:                         "0710", // 刃轮
            //     name:                       "铁陀螺", // 刃轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n2攻x6",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.AttackProcedure(2, times: 6);
            //         return null;
            //     }),
            //
            // new(id:                         "0711", // 匣匣
            //     name:                       "防壁", // 匣匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+20\n柔韧+2",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainArmorProcedure(20, induced: false);
            //         await caster.GainBuffProcedure("柔韧", 2);
            //         return null;
            //     }),
            //
            // new(id:                         "0712", // 匣轮
            //     name:                       "不倒翁", // 匣轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n下2次护甲减少时，加回",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainBuffProcedure("护甲回收", 2);
            //         return null;
            //     }),
            //
            // new(id:                         "0713", // 轮轮
            //     name:                       "助推器", // 轮轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.YuanYingOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Swift,
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n二动 二重",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         caster.SetActionPoint(2);
            //         await caster.GainBuffProcedure("二重");
            //         return null;
            //     }),
            //
            // // 返虚
            //
            // new(id:                         "0714", // 香香香
            //     name:                       "反应堆", // 香香香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n疲劳\n遭受1不堪一击，永久二重+1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainBuffProcedure("不堪一击");
            //         await caster.GainBuffProcedure("永久二重");
            //         return null;
            //     }),
            //
            // new(id:                         "0715", // 香香刃
            //     name:                       "烟花", // 香香刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n消耗所有灵气，每1，力量+1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         int stack = caster.GetStackOfBuff("灵气");
            //         await caster.TryConsumeProcedure("灵气", stack);
            //         await caster.GainBuffProcedure("力量", stack);
            //         return null;
            //     }),
            //
            // new(id:                         "0716", // 香香匣
            //     name:                       "长明灯", // 香香匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n疲劳\n获得灵气时：每1，气血+3",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("长明灯", 3);
            //         return null;
            //     }),
            //
            // new(id:                         "0717", // 香香轮
            //     name:                       "大往生香", // 香香轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust | SkillType.Mana,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n疲劳\n永久免费+1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("永久免费");
            //         return null;
            //     }),
            //
            // new(id:                         "0718", // 缺少匣
            //     name:                       "地府通讯器", // 缺少匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Mana,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n失去一半气血，每8，灵气+1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         int gain = caster.Hp / 16;
            //         await caster.LoseHealthProcedure(gain * 8);
            //         await caster.GainBuffProcedure("灵气", gain);
            //         return null;
            //     }),
            //
            // new(id:                         "0719", // 刃刃刃
            //     name:                       "无人机阵列", // 刃刃刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n没有效果",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         return null;
            //     }),
            //
            // new(id:                         "0720", // 刃刃香
            //     name:                       "弩炮", // 刃刃香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Attack,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n50攻 吸血",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.AttackProcedure(50, lifeSteal: true);
            //         return null;
            //     }),
            //
            // new(id:                         "0721", // 刃刃匣
            //     name:                       "尖刺陷阱", // 刃刃匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n下次受到攻击时，对对方施加等量减甲",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainBuffProcedure("尖刺陷阱");
            //         return null;
            //     }),
            //
            // new(id:                         "0722", // 刃刃轮
            //     name:                       "暴雨梨花针", // 刃刃轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Attack,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n1攻x10",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.AttackProcedure(1, times: 10);
            //         return null;
            //     }),
            //
            // new(id:                         "0723", // 缺少轮
            //     name:                       "炼丹炉", // 缺少轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n每回合力量+1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("回合力量");
            //         return null;
            //     }),
            //
            // new(id:                         "0724", // 匣匣匣
            //     name:                       "浮空艇", // 匣匣匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n疲劳\n回合被跳过时，该回合无法受到伤害\n遭受12跳回合",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("浮空艇");
            //         await caster.GainBuffProcedure("跳回合", 12);
            //         return null;
            //     }),
            //
            // new(id:                         "0725", // 匣匣香
            //     name:                       "动量中和器", // 匣匣香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n格挡+10",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainBuffProcedure("格挡", 10);
            //         return null;
            //     }),
            //
            // new(id:                         "0726", // 匣匣刃
            //     name:                       "机关伞", // 匣匣刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n灼烧+8",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainBuffProcedure("灼烧", 8);
            //         return null;
            //     }),
            //
            // new(id:                         "0727", // 匣匣轮
            //     name:                       "一轮马", // 匣匣轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n闪避+6",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await caster.GainBuffProcedure("闪避", 6);
            //         return null;
            //     }),
            //
            // new(id:                         "0728", // 缺少香
            //     name:                       "外骨骼", // 缺少香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n疲劳\n攻击时，护甲+3",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("外骨骼", 3);
            //         return null;
            //     }),
            //
            // new(id:                         "0729", // 轮轮轮
            //     name:                       "永动机", // 轮轮轮
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust | SkillType.Mana,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n疲劳\n力量+8 灵气+8\n8回合后死亡",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("力量", 8);
            //         await caster.GainBuffProcedure("灵气", 8);
            //         await caster.GainBuffProcedure("永动机", 8);
            //         return null;
            //     }),
            //
            // new(id:                         "0730", // 轮轮香
            //     name:                       "火箭靴", // 轮轮香
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n疲劳\n使用灵气牌时，获得二动",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("火箭靴");
            //         return null;
            //     }),
            //
            // new(id:                         "0731", // 轮轮刃
            //     name:                       "定龙桩", // 轮轮刃
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n疲劳\n对方二动时，如果没有暴击，获得1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("定龙桩");
            //         return null;
            //     }),
            //
            // new(id:                         "0732", // 轮轮匣
            //     name:                       "飞行器", // 轮轮匣
            //     wuXing:                     null,
            //     jingJieBound:               JingJie.FanXuOnly,
            //     skillTypeComposite:         SkillType.Deplete | SkillType.Exhaust,
            //     cost:                       CostResult.ChannelFromValue(2),
            //     costDescription:            CostDescription.ChannelFromValue(2),
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n疲劳\n成功闪避时，如果对方没有跳回合，施加1",
            //     withinPool:                 false,
            //     cast:                       async (env, caster, skill, recursive, castResult) =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("飞行器");
            //         return null;
            //     }),

            #endregion
        });
    }

    public void Init()
    {
        List.Do(entry => entry.GenerateAnnotations());
    }

    public override SkillEntry DefaultEntry() => this["0000"];
}
