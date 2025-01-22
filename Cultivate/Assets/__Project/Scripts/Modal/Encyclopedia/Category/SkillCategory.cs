
using System.Collections.Generic;
using UnityEngine;
using CLLibrary;
using UnityEditor;

public class SkillCategory : Category<SkillEntry>
{
    public SkillCategory()
    {
        AddRange(new List<SkillEntry>()
        {
            #region 标准牌

            new(id:                         "0101",
                name:                       "金刃",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(4 + dj)}攻".ApplyAttack() +
                    $"\n施加{Fib.ToValue(3 + dj)}破甲",
                cast:                       async d =>
                {
                    await d.AttackProcedure(Fib.ToValue(4 + d.Dj));
                    await d.RemoveArmorProcedure(Fib.ToValue(3 + d.Dj), false);
                }),

            new(id:                         "0102",
                name:                       "寻猎",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n击伤：施加{5 + 5 * dj}破甲".ApplyCond(castResult),
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.RemoveArmorProcedure(5 + 5 * d.SrcSkill.Dj, true);
                            d.CastResult.AppendCond(true);
                        });

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(2,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0115",
                name:                       "山风",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                cost:                       CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    (j <= JingJie.LianQi ? "" : $"护甲+{5 + 5 * dj}\n".ApplyDefend()) +
                    (j <= JingJie.JinDan ? "锋锐+1" : $"每{30 - 5 * dj}护甲，锋锐+1"),
                cast:                       async d =>
                {
                    if (d.J > JingJie.LianQi)
                        await d.GainArmorProcedure(5 + 5 * d.Dj, induced: false);

                    int gain = (d.J <= JingJie.JinDan) ? 1 : d.Caster.Armor / (30 - 5 * d.Dj);
                    await d.CycleProcedure(WuXing.Jin, gain);
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
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.GainBuffProcedure("灵气", 1 + d.SrcSkill.Dj, induced: true);
                            d.CastResult.AppendCond(true);
                        });

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(4,
                        closures: new [] { closure });
                },
                startStageCast:             async d =>
                {
                    await d.Caster.GainBuffProcedure("灵气", 1 + d.Skill.Dj, induced: false);
                }),

            new(id:                         "0105",
                name:                       "杀意",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{6 + 3 * dj}攻".ApplyAttack() +
                    $"\n暴击+1",
                withinPool:                 false,
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_FULL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.GainBuffProcedure("暴击", induced: true);
                        });

                    await d.AttackProcedure(6 + 3 * d.Dj,
                        closures: new [] { closure });
                }),

            new(id:                         "0118",
                name:                       "敛息",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{6 + 3 * dj}攻".ApplyAttack() +
                    $"\n击伤：伤害转为破甲".ApplyCond(castResult),
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_DAMAGE, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.RemoveArmorProcedure(d.Value, false);
                            d.Cancel = true;
                            d.CastResult.AppendCond(true);
                        });
                    
                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(6 + 3 * d.Dj,
                        closures: new [] { closure });
                }),

            new(id:                         "0110",
                name:                       "天地同寿",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加{Fib.ToValue(6 + dj)}破甲" +
                    $"\n自身每1破甲，多1" +
                    $"\n开局：施加20破甲",
                cast:                       async d =>
                {
                    int fragile = -d.Caster.Armor.ClampUpper(0);
                    int value = Fib.ToValue(6 + d.Dj) + fragile;
                    await d.RemoveArmorProcedure(value, false);
                },
                startStageCast:             async d =>
                {
                    await d.Caster.RemoveArmorProcedure(20, false);
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
             //     cast:                       async d =>
             //     {
             //         int count = caster.CountSuch(s => s.Entry.WuXing == WuXing.Jin);
             //         await caster.AttackProcedure(count * (3 + d.Dj), wuXing: skill.Entry.WuXing, srcSkill: skill,
             //             castResult: castResult);
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
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("灵气", 8);
                    d.Caster.SetActionPoint(2);
                    await d.GainBuffProcedure("滞气", 5 - d.Dj);
                }),

            new(id:                         "0109",
                name:                       "流云",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{8 + 4 * dj}攻".ApplyAttack() +
                    $"\n下回合{8 + 4 * dj}攻",
                cast:                       async d =>
                {
                    int value = 8 + 4 * d.Dj;
                    await d.AttackProcedure(value);
                    await d.GainBuffProcedure("延迟攻", value, induced: true);
                }),

            new(id:                         "0121",
                name:                       "贪狼",
                wuXing:                     WuXing.Jin,
                skillTypeComposite:         SkillType.Attack | SkillType.Mana,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻".ApplyAttack() +
                    $"\n灵气+{1 + dj}".ApplyMana() +
                    $"\n击伤：移除{1 + dj}灵气".ApplyCond(castResult),
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.RemoveBuffProcedure("灵气", 1 + d.SrcSkill.Dj, induced: true);
                            d.CastResult.AppendCond(true);
                        });

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(10,
                        closures: new [] { closure });
                    await d.GainBuffProcedure("灵气", 1 + d.Dj, induced: true);
                }),
            
            new(id:                         "0119",
                name:                       "停云",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"锋锐+{1 + dj}" +
                    $"\n开局：锋锐+{1 + dj}",
                cast:                       async d =>
                {
                    await d.CycleProcedure(WuXing.Jin, gain: 1 + d.Dj);
                },
                startStageCast:             async d =>
                {
                    await d.Caster.CycleProcedure(WuXing.Jin, gain: 1 + d.Skill.Dj);
                }),

            new(id:                         "0113",
                name:                       "无妄",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"6攻x{2 + dj}".ApplyAttack() +
                    $"\n暴击",
                cast:                       async d =>
                {

                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.GainBuffProcedure("暴击");
                        });

                    await d.AttackProcedure(6, times: 2 + d.Dj,
                        closures: new[] { closure });
                }),

            new(id:                         "0116",
                name:                       "袖里乾坤",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"暴击+{1 + dj}" +
                    $"\n护甲+6".ApplyDefend() +
                    $"\n开局：暴击+{1 + dj}",
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("暴击", 1 + d.Dj);
                    await d.GainArmorProcedure(6, induced: true);
                },
                startStageCast:             async d =>
                {
                    await d.Caster.GainBuffProcedure("暴击", 1 + d.Skill.Dj);
                }),

            new(id:                         "0112",
                name:                       "凝水",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"锋锐+{1 + dj}" +
                    $"\n每1锋锐，灵气+1".ApplyMana(),
                cast:                       async d =>
                {
                    await d.CycleProcedure(WuXing.Jin, gain: 1 + d.Dj);
                    int stack = d.Caster.GetStackOfBuff("锋锐");
                    await d.GainBuffProcedure("灵气", stack, induced: true);
                }),

            new(id:                         "0117",
                name:                       "一莲托生",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() + " 暴击释放" +
                    $"\n开局：施加1跳回合",
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_DAMAGE, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;
                            
                            int critStack = d.Src.GetStackOfBuff("暴击");
                            await d.Src.TryConsumeProcedure("暴击", critStack);
                            d.Value *= 1 + critStack;
                        });

                    await d.AttackProcedure(1,
                        closures: new [] { closure });
                },
                startStageCast:             async d =>
                {
                    await d.Caster.GiveBuffProcedure("跳行动", induced: false);
                }),

            new(id:                         "0122",
                name:                       "闪击",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() +
                    $"\n破甲将补齐至攻击前的数值",
                cast:                       async d =>
                {
                    StageClosure closure0 = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;

                            int oppoFragileBeforeAttack = d.SrcSkill.Owner.Opponent().Armor;
                            if (oppoFragileBeforeAttack < 0)
                                d.SrcSkill.Owner.Memory.SetVariable("OppoFragileBeforeAttack", oppoFragileBeforeAttack);
                        });
                    
                    StageClosure closure1 = new(StageClosureDict.DID_FULL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;

                            int oppoFragileBeforeAttack =
                                d.SrcSkill.Owner.Memory.TryGetVariable("OppoFragileBeforeAttack", 0);

                            int gap = d.SrcSkill.Owner.Opponent().Armor - oppoFragileBeforeAttack;
                            if (oppoFragileBeforeAttack < 0 && gap > 0)
                            {
                                await d.SrcSkill.Owner.Opponent().LoseArmorProcedure(gap, induced: true);
                            }
                            
                            d.SrcSkill.Owner.Memory.SetVariable("OppoFragileBeforeAttack", 0);
                        });

                    await d.AttackProcedure(1,
                        closures: new [] { closure0, closure1 });
                },
                startStageCast:             async d =>
                {
                    await d.Caster.GiveBuffProcedure("跳回合", induced: false);
                }),

            new(id:                         "0111",
                name:                       "凛冽",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"锋锐+3" +
                    $"\n锋锐具有吸血".ApplyHeal() +
                    $"\n无法攻击",
                cast:                       async d =>
                {
                    await d.CycleProcedure(WuXing.Jin, gain: 3);
                    await d.GainBuffProcedure("凛冽", induced: true);
                    await d.GainBuffProcedure("无法攻击", induced: true);
                }),
            
            new(id:                         "0120",
                name:                       "弹指",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+4".ApplyMana() +
                    $"\n消耗1暴击：翻倍".ApplyCond(castResult),
                cast:                       async d =>
                {
                    bool cond = await d.TryConsumeProcedure("暴击");
                    int bitShift = cond ? 1 : 0;
                    await d.GainBuffProcedure("灵气", 4 << bitShift);
                    d.CastResult.AppendCond(cond);
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
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            d.LifeSteal = true;
                        });
                    
                    await d.AttackProcedure(Fib.ToValue(3 + d.Dj) * 2,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0206",
                name:                       "空幻",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                cost:                       async (env, entity, skill, recursive) => new ManaCostResult(3 + skill.Dj - entity.CountSuch(s => s.Entry.WuXing == WuXing.Shui)),
                costDescription:            CostDescription.ManaFromDj(dj => 3 + dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{8 + 6 * dj}攻".ApplyAttack() +
                    $"\n每携带1水：消耗-1",
                cast:                       async d =>
                {
                    await d.AttackProcedure(8 + 6 * d.Dj);
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
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("灵气", 2 + d.Dj);
                    if (d.J >= JingJie.HuaShen)
                        await d.GainBuffProcedure("吐纳");
                    // await Procedure
                    d.Caster.MaxHp += 4 + 4 * d.Dj;
                }),
            
            new(id:                         "0205",
                name:                       "止水",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(6 + dj)}攻".ApplyAttack() +
                    $"\n未击伤：受到治疗".ApplyCond(castResult),
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.UNDAMAGED, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.HealProcedure(d.Value, induced: true);
                            d.CastResult.AppendCond(true);
                        });

                    int value = Fib.ToValue(6 + d.Dj);

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(value,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0203",
                name:                       "流霰",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{9 + 3 * dj}攻".ApplyAttack() +
                    $"\n每造成{9 - dj}点伤害，格挡+1".ApplyDefend(),
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;

                            int gain = d.Value / (9 - d.SrcSkill.Dj);
                            await d.Src.CycleProcedure(WuXing.Shui, gain: gain);
                        });

                    await d.AttackProcedure(9 + 3 * d.Dj,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0210",
                name:                       "无念无想",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Health,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"治疗{20 + 10 * dj}".ApplyHeal() +
                    $"\n遭受5缠绕".ApplyDebuff(),
                cast:                       async d =>
                {
                    await d.HealProcedure(20 + 10 * d.Dj, induced: false);
                    await d.GainBuffProcedure("缠绕", 5);
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
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("灵气", 7 + 2 * d.Dj);
                }),
            
            new(id:                         "0213",
                name:                       "大鱼",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend | SkillType.Swift,
                cost:                       CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{4 * (1 << dj)}攻".ApplyAttack() +
                    $"\n护甲+{4 * (1 << dj)}".ApplyDefend() + $" 二动",
                cast:                       async d =>
                {
                    int value = 4 * (1 << d.Dj);
                    await d.AttackProcedure(value);
                    await d.GainArmorProcedure(value, induced: false);
                    d.Caster.SetActionPoint(2);
                }),
            
            new(id:                         "0209",
                name:                       "写意",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_DAMAGE, 1, async (owner, closureDetails) =>
                    {
                        StageSkill s = owner as StageSkill;
                        DamageDetails d = (DamageDetails)closureDetails;
                    
                        if (s.Owner != d.Src) return;
                    
                        string critKey = "TriggeredCrit";
                        s.Owner.Memory.PerformOperation(critKey, false, record => record | d.Crit);
                        
                        string lifestealKey = "TriggeredLifesteal";
                        s.Owner.Memory.PerformOperation(lifestealKey, false, record => record | d.LifeSteal);
                    }),
                    new(StageClosureDict.WIL_ATTACK, 1, async (owner, closureDetails) =>
                    {
                        StageSkill s = owner as StageSkill;
                        AttackDetails d = (AttackDetails)closureDetails;
                    
                        if (s.Owner != d.Src) return;
                        
                        string penetrateKey = "TriggeredPenetrate";
                        s.Owner.Memory.PerformOperation(penetrateKey, false, record => record | d.Penetrate);
                    }),
                },
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{10 + 4 * dj}攻".ApplyAttack() +
                    $"\n返还触发过的" +
                    $"暴击".ApplyStyle(castResult, "0") +
                    $"/" +
                    $"吸血".ApplyStyle(castResult, "1") +
                    $"/" +
                    $"穿透".ApplyStyle(castResult, "2"),
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            string critKey = "TriggeredCrit";
                            string lifestealKey = "TriggeredLifesteal";
                            string penetrateKey = "TriggeredPenetrate";
                            bool crit = d.Src.Memory.TryGetVariable(critKey, false);
                            bool lifeSteal = d.Src.Memory.TryGetVariable(lifestealKey, false);
                            bool penetrate = d.Src.Memory.TryGetVariable(penetrateKey, false);
                            if (crit) await d.Src.GainBuffProcedure("暴击", induced: true);
                            if (lifeSteal) await d.Src.GainBuffProcedure("吸血", induced: true);
                            if (penetrate) await d.Src.GainBuffProcedure("穿透", induced: true);
                            d.CastResult.AppendBools(crit, lifeSteal, penetrate);
                        });

                    d.CastResult.AppendBools(false, false, false);
                    await d.AttackProcedure(10 + 4 * d.Dj,
                        closures: new [] { closure });
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
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("格挡", 1 + d.Dj);
                    await d.CycleProcedure(WuXing.Shui);
                    int stack = d.Caster.GetStackOfBuff("格挡");
                    await d.HealProcedure(2 * stack, induced: false);
                }),

            new(id:                         "0216",
                name:                       "气吞山河",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_GAIN_BUFF, -1, async (owner, closureDetails) =>
                    {
                        StageSkill s = owner as StageSkill;
                        GainBuffDetails d = (GainBuffDetails)closureDetails;

                        if (s.Owner != d.Tgt) return;
                        if (d._buffEntry.GetName() != "灵气") return;

                        string key = "HighestManaRecord";
                        s.Owner.Memory.PerformOperation(key, 0, record => Mathf.Max(record, s.Owner.GetStackOfBuff("灵气")));
                    }),
                },
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气补至本局最高+{1 + dj}".ApplyMana(),
                cast:                       async d =>
                {
                    string key = "HighestManaRecord";

                    int highestManaRecord = d.Caster.Memory.TryGetVariable(key, 0);
                    int space = highestManaRecord - d.Caster.GetStackOfBuff("灵气") + 1 + d.Dj;
                    await d.GainBuffProcedure("灵气", space);
                }),
            
            new(id:                         "0218",
                name:                       "吞天",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_HEAL, 1, async (owner, closureDetails) =>
                    {
                        StageSkill s = owner as StageSkill;
                        HealDetails d = (HealDetails)closureDetails;
                        
                        if (s.Owner != d.Src) return;
                        string key = "healRecord";
                        s.Owner.Memory.PerformOperation(key, 0, record => record += d.Value);
                    }),
                },
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻\n每{6 - dj}累计治疗，多1攻".ApplyAttack(),
                cast:                       async d =>
                {

                    StageClosure closure = new(StageClosureDict.DID_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            string key = "healRecord";

                            int healed = d.Src.Memory.TryGetVariable(key, 0);
                            int gain = healed / (6 - d.SrcSkill.Dj);
                            d.Value += gain;
                        });

                    await d.AttackProcedure(1,
                        closures: new [] { closure });
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
                cast:                       async d =>
                {
                    d.Caster.SetActionPoint(d.J <= JingJie.YuanYing ? 2 : 3);
                }),

            new(id:                         "0217",
                name:                       "一梦如是",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack | SkillType.Health,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() +
                    ($"\n击伤：" + "下1次受伤转为治疗".ApplyHeal()).ApplyCond(castResult),
                cast:                       async d =>
                {

                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.GainBuffProcedure("一梦如是", induced: true);
                            d.CastResult.AppendCond(true);
                        });

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(1,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0219",
                name:                       "飞鸿踏雪",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Defend | SkillType.Swift,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"二动时：获得1格挡" +
                    $"\n二动",
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("飞鸿踏雪");
                    d.Caster.SetActionPoint(2);
                }),
            
            new(id:                         "0221",
                name:                       "镜花水月",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Mana | SkillType.Health,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗每40生命，灵气+1".ApplyMana() +
                    $"\n消耗每1灵气，生命+10".ApplyHeal(),
                cast:                       async d =>
                {
                    int healthConsume = (d.Caster.Hp - 1).ClampLower(0) / 40 * 40;
                    int manaConsume = d.Caster.GetStackOfBuff("灵气");

                    if (healthConsume > 0)
                        await d.LoseHealthProcedure(healthConsume, induced: true);
                    if (manaConsume > 0)
                        await d.LoseBuffProcedure("灵气", manaConsume);

                    int healthGain = manaConsume * 10;
                    int manaGain = healthConsume / 40;

                    if (healthGain > 0)
                        await d.HealProcedure(healthGain, induced: true);
                    if (manaGain > 0)
                        await d.GainBuffProcedure("灵气", manaGain);
                }),
            
            new(id:                         "0301",
                name:                       "小松",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.ZiZhi,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(4 + dj)}攻".ApplyAttack() + " 穿透" +
                    $"\n成长：多{Fib.ToValue(3 + dj)}攻",
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            d.Penetrate = true;
                            d.Value += Fib.ToValue(3 + d.SrcSkill.Dj) * d.SrcSkill.StageCastedCount;
                        });

                    await d.AttackProcedure(Fib.ToValue(4 + d.Dj),
                        closures: new [] { closure });
                }),

            new(id:                         "0302",
                name:                       "潜龙在渊",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend | SkillType.ZiZhi,
                cost:                       CostResult.ManaFromValue(1),
                costDescription:            CostDescription.ManaFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"闪避+1".ApplyDefend() +
                    $"\n成长{2 + dj}次后：" + $"{(4 + 2 * dj) * (4 + 2 * dj)}攻".ApplyAttack(),
                cast:                       async d =>
                {
                    if (d.Cc >= 2 + d.Dj)
                        await d.AttackProcedure((4 + 2 * d.Dj) * (4 + 2 * d.Dj));

                    await d.GainBuffProcedure("闪避");
                }),

            new(id:                         "0311",
                name:                       "彼岸花",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"力量+{1 + dj}" +
                    $"\n遭受{3 + 2 * dj}腐朽".ApplyDebuff(),
                cast:                       async d =>
                {
                    await d.CycleProcedure(WuXing.Mu, gain: 1 + d.Dj);
                    await d.GainBuffProcedure("腐朽", 3 + 2 * d.Dj);
                }),
            
            new(id:                         "0304",
                name:                       "明神",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Mana | SkillType.ZiZhi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}".ApplyMana() +
                    $"\n成长：多" + (j >= JingJie.HuaShen ? "2" : "1"),
                cast:                       async d =>
                {
                    int mul = d.J >= JingJie.HuaShen ? 2 : 1;
                    await d.GainBuffProcedure("灵气", 1 + d.Dj + d.Cc * mul);
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
                cast:                       async d =>
                {

                    StageClosure wilAttack = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            bool cond = await d.SrcSkill.IsEnd(useFocus: true);
                            d.Penetrate |= cond;
                            d.CastResult.AppendBool(0, cond);
                        });
                    
                    StageClosure didDamage = new(StageClosureDict.DID_DAMAGE, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.GainBuffProcedure("穿透", induced: true);
                            d.CastResult.AppendBool(1, true);
                        });

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(7 + 3 * d.Dj,
                        closures: new [] { wilAttack, didDamage });
                }),

            new(id:                         "0306",
                name:                       "见龙在田",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend | SkillType.ZiZhi,
                cost:                       async (env, entity, skill, recursive) => new ChannelCostResult(4 - skill.Dj - skill.StageCastedCount),
                costDescription:            CostDescription.ChannelFromDj(dj => 4 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻".ApplyAttack() + " 闪避+2".ApplyDefend() +
                    $"\n成长：吟唱-1",
                cast:                       async d =>
                {
                    await d.AttackProcedure(10);
                    await d.GainBuffProcedure("闪避", 2);
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
                cast:                       async d =>
                {
                    int value = 10 + 6 * d.Dj;
                    await d.GainArmorProcedure(value, induced: false);
                    await d.GiveArmorProcedure(value, induced: true);
                    await d.HealProcedure(value, induced: false);
                    await d.HealOppoProcedure(value, induced: true);
                }),
            
            new(id:                         "0308",
                name:                       "若竹",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + dj}".ApplyMana() +
                    $"\n穿透+1",
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("灵气", 1 + d.Dj);
                    await d.GainBuffProcedure("穿透");
                }),

            new(id:                         "0310",
                name:                       "飞龙在天",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Defend | SkillType.ZiZhi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"闪避+1".ApplyDefend() +
                    $"\n初次：跳过下{2 + 2 * dj}张牌，使其成长".ApplyCond(castResult),
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("闪避");
                    bool cond = await d.Skill.IsFirstTime();
                    if (cond)
                        await d.GainBuffProcedure("飞龙在天", 2 + 2 * d.Dj, induced: true);
                    d.CastResult.AppendCond(cond);
                }),

            new(id:                         "0315",
                name:                       "落英",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"力量+{1 + dj}" +
                    $"\n消耗每{5 - dj}灵气，多1",
                cast:                       async d =>
                {
                    await d.CycleProcedure(WuXing.Mu, gain: 1 + d.Dj);
                    await d.TransferProcedure(5 - d.Dj, "灵气", 1, "力量", true);
                }),
            
            new(id:                         "0312",
                name:                       "钟声",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"使下{1 + dj}张牌升级",
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("钟声", 1 + d.Dj);
                }),

            new(id:                         "0309",
                name:                       "入木三分",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() +
                    $"\n对方每有{3 - dj}护甲，多1",
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            d.Value += d.Src.Opponent().Armor / (3 - d.SrcSkill.Dj);
                        });

                    await d.AttackProcedure(1,
                        closures: new [] { closure });
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
                cast:                       async d =>
                {
                    int leftIndex = d.Skill.Prev(true).SlotIndex;
                    int rightIndex = d.Skill.Next(true).SlotIndex;
                    
                    var tempStageSkill = d.Caster._skills[leftIndex];
                    d.Caster._skills[leftIndex] = d.Caster._skills[rightIndex];
                    d.Caster._skills[rightIndex] = tempStageSkill;

                    var tempIndex = d.Caster._skills[leftIndex].SlotIndex;
                    d.Caster._skills[leftIndex].SlotIndex = d.Caster._skills[rightIndex].SlotIndex;
                    d.Caster._skills[rightIndex].SlotIndex = tempIndex;
                    
                    if (d.J > JingJie.YuanYing)
                        d.Caster.SetActionPoint(2);
                }),

            new(id:                         "0316",
                name:                       "回响",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    (j < JingJie.HuaShen ? $"使用第一张牌\n已升华的牌无效" : $"使用前两张牌\n已升华的牌无效"),
                cast:                       async d =>
                {
                    if (!d.Recursive)
                        return;
                    if (!d.Caster._skills[0].Exhausted)
                        await d.Caster.CastProcedure(d.Caster._skills[0], false);
                    
                    if (d.J >= JingJie.HuaShen)
                        if (!d.Caster._skills[1].Exhausted)
                            await d.Caster.CastProcedure(d.Caster._skills[1], false);
                }),

            new(id:                         "0317",
                name:                       "一叶知秋",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_FULL_ATTACK, -1, async (owner, closureDetails) =>
                    {
                        StageSkill s = owner as StageSkill;
                        AttackDetails d = (AttackDetails)closureDetails;

                        if (s.Owner != d.Src) return;
                        if (d.SrcSkill == s) return;

                        string key = "UsedClosureDict";
                        s.Owner.Memory.PerformOperation(key, new Dictionary<StageSkill, StageClosure[]>(), record =>
                        {
                            if (record.ContainsKey(d.SrcSkill))
                            {
                                bool newHasMore = d.Closures.Length > record[d.SrcSkill].Length;
                                if (newHasMore)
                                    record[d.SrcSkill] = d.Closures;
                                return record;
                            }
                            record[d.SrcSkill] = d.Closures;
                            return record;
                        });
                    }),
                },
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() +
                    $"\n具有所有已触发的攻击描述",
                cast:                       async d =>
                {
                    string key = "UsedClosureDict";
                    Dictionary<StageSkill, StageClosure[]> dict = d.Caster.Memory.TryGetVariable(key, new Dictionary<StageSkill, StageClosure[]>());
                    List<StageClosure> flattenedList = new List<StageClosure>();
                    dict.Do(kvp => flattenedList.AddRange(kvp.Value));
                    
                    StageClosure[] closures = flattenedList.ToArray();
                    await d.AttackProcedure(1, closures: closures);
                }),

            new(id:                         "0318",
                name:                       "亢龙有悔",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"穿透/力量/" + "闪避".ApplyDefend() + "+3" +
                    $"\n遭受不堪一击".ApplyDebuff(),
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("穿透", stack: 3, induced: true);
                    await d.GainBuffProcedure("力量", stack: 3, induced: false);
                    await d.GainBuffProcedure("闪避", stack: 3, induced: false);
                    await d.GainBuffProcedure("不堪一击", induced: false);
                }),

            new(id:                         "0319",
                name:                       "刹那芳华",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Defend | SkillType.Exhaust | SkillType.ZiZhi,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"升华 力量/" + "闪避".ApplyDefend() + "+1" +
                    $"\n成长：多1",
                cast:                       async d =>
                {
                    await d.Skill.ExhaustProcedure();
                    await d.GainBuffProcedure("力量", 1 + d.Cc);
                    await d.GainBuffProcedure("闪避", 1 + d.Cc);
                }),

            new(id:                         "0320",
                name:                       "一念无量劫",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.HuaShenOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"消耗每8灵气，多重+1",
                cast:                       async d =>
                {
                    await d.TransferProcedure(8, "灵气", 1, "多重", true, upperBound: 20);
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
                cast:                       async d =>
                {
                    int value = 2 + 2 * d.Dj;
                    await d.AttackProcedure(value, times: 2);
                    await d.GainArmorProcedure(value, induced: false);
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
                cast:                       async d =>
                {
                    await d.AttackProcedure(Fib.ToValue(6 + d.Dj));
                }),

            new(id:                         "0403",
                name:                       "正念",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                cost:                       CostResult.ChannelFromDj(dj => 5 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 5 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"升华" +
                    $"\n护甲+{10 + 10 * dj}".ApplyDefend(),
                cast:                       async d =>
                {
                    await d.Skill.ExhaustProcedure();
                    await d.GainArmorProcedure(10 + 10 * d.Dj, induced: false);
                }),

            new(id:                         "0404",
                name:                       "拂晓",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{3 + 2 * dj}攻".ApplyAttack() +
                    $"\n灵气+{1 + dj / 2}".ApplyMana(),
                cast:                       async d =>
                {
                    await d.AttackProcedure(3 + 2 * d.Dj);
                    await d.GainBuffProcedure("灵气", 1 + d.Dj / 2);
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
                cast:                       async d =>
                {
                    await d.AttackProcedure(7 + d.Dj, times: 3 + 2 * d.Dj);
                }),

            new(id:                         "0422",
                name:                       "绝境",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    (j >= JingJie.HuaShen ? $"力量+2" : $"力量+1") +
                    $"\n残血：多{Fib.ToValue(2 + dj)}".ApplyCond(castResult),
                cast:                       async d =>
                {
                    int value = d.J >= JingJie.HuaShen ? 2 : 1;
                    bool cond = d.Caster.IsLowHealth;
                    if (cond)
                    {
                        value += Fib.ToValue(2 + d.Dj);
                    }
                    await d.GainBuffProcedure("力量", value);
                    
                    d.CastResult.AppendCond(cond);
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
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("灼烧", 1 + d.Dj);
                    await d.CycleProcedure(WuXing.Huo);
                    int stack = d.Caster.GetStackOfBuff("灼烧");
                    await d.DispelProcedure(stack);
                }),
            
            new(id:                         "0408",
                name:                       "舍生",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                cost:                       async (env, entity, skill, recursive) =>
                    new HealthCostResult(entity.IsLowHealth ? 0 : 8),
                costDescription:            CostDescription.HealthFromValue(8),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{(3 + dj) * (4 + dj)}".ApplyDefend() +
                    $"\n残血：免除消耗",
                cast:                       async d =>
                {
                    int value = (3 + d.Dj) * (4 + d.Dj);
                    await d.GainArmorProcedure(value, induced: false);
                }),
            
            new(id:                         "0409",
                name:                       "天衣无缝",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+2".ApplyDefend() +
                    $"\n直到使用攻击牌：" + $"每回合{3 + 2 * dj}攻".ApplyAttack(),
                cast:                       async d =>
                {
                    await d.GainArmorProcedure(2, false);
                    await d.GainBuffProcedure("天衣无缝", 3 + d.Dj * 2);
                }),

            new(id:                         "0410",
                name:                       "登宝塔",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Exhaust | SkillType.Defend,
                cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                withinPool:                 false,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"升华" +
                    $"\n护甲+6".ApplyDefend() +
                    $"\n每1张已升华牌，多6",
                cast:                       async d =>
                {
                    await d.Skill.ExhaustProcedure();
                    await d.GainArmorProcedure(6 * (1 + d.Caster.ExhaustedCount), induced: false);
                }),

            new(id:                         "0423",
                name:                       "风中残烛",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                cost:                       CostResult.HealthFromValue(8),
                costDescription:            CostDescription.HealthFromValue(8),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"锻体+{5 + 5 * dj}" +
                    $"\n残血：获得2闪避".ApplyCond(castResult),
                cast:                       async d =>
                {
                    bool cond = d.Caster.IsLowHealth;

                    await d.GainBuffProcedure("锻体", 5 + 5 * d.Dj);
                    
                    if (cond)
                    {
                        await d.GainBuffProcedure("闪避", 2);
                    }
                    d.CastResult.AppendCond(cond);
                }),

            new(id:                         "0411",
                name:                       "燎原",
                wuXing:                     WuXing.Huo,
                skillTypeComposite:         SkillType.ZiZhi,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灼烧+{dj}" +
                    $"\n成长:多1",
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("灼烧", d.Dj + d.Cc);
                }),

            new(id:                         "0412",
                name:                       "坐忘",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Exhaust,
                cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"升华" +
                    $"\n下一张牌升华",
                cast:                       async d =>
                {
                    await d.Skill.ExhaustProcedure();
                    await d.GainBuffProcedure("坐忘");
                }),

            new(id:                         "0413",
                name:                       "剑王行",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻x{4 + 2 * dj}".ApplyAttack(),
                cast:                       async d =>
                {
                    await d.AttackProcedure(1, times: 4 + 2 * d.Dj);
                }),

            new(id:                         "0415",
                name:                       "观众生",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Exhaust,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"升华左边的{1 + dj}张牌",
                cast:                       async d =>
                {
                    for (int i = 0; i < 1 + d.Dj; i++)
                    {
                        StageSkill skill = d.Skill.Prevs(false).FirstObj(skill => !skill.Exhausted) ?? d.Skill;
                        await skill.ExhaustProcedure();
                    }
                }),

            new(id:                         "0416",
                name:                       "晚霞",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ChannelFromDj(dj => 2 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 2 - dj),
                skillTypeComposite:         SkillType.Exhaust | SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"升华" +
                    $"\n灵气+8".ApplyMana(),
                cast:                       async d =>
                {
                    await d.Skill.ExhaustProcedure();
                    await d.GainBuffProcedure("灵气", 8);
                }),

            new(id:                         "0417",
                name:                       "一舞惊鸿",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_GAIN_BUFF, -1, async (owner, closureDetails) =>
                    {
                        StageSkill s = owner as StageSkill;
                        GainBuffDetails d = (GainBuffDetails)closureDetails;

                        if (s.Owner != d.Tgt) return;
                        if (d._buffEntry.GetName() != "闪避") return;

                        string key = "GainedEvadeRecord";
                        s.Owner.Memory.PerformOperation(key, 0, record => record + d._stack);
                    }),
                },
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() +
                    $"\n每获得过1闪避，多1次",
                cast:                       async d =>
                {

                    StageClosure closure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            string key = "GainedEvadeRecord";
                            int gainedEvadeRecord = d.Src.Memory.TryGetVariable(key, 0);
                            d.Times += gainedEvadeRecord;
                        });

                    await d.AttackProcedure(1,
                        closures: new [] { closure });
                }),

            new(id:                         "0406",
                name:                       "不动明王诀",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    "成为残血" +
                    "\n下一次受伤时，如果致死，则无效",
                cast:                       async d =>
                {
                    await d.BecomeLowHealth();
                    await d.GainBuffProcedure("不动明王诀");
                }),

            new(id:                         "0419",
                name:                       "净天地",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.HuaShenOnly,
                skillTypeComposite:         SkillType.Exhaust,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"升华" +
                    $"\n使用所有已升华牌",
                cast:                       async d =>
                {
                    if (!d.Recursive)
                        return;
                    
                    await d.Skill.ExhaustProcedure();
                    foreach (StageSkill s in d.Caster._skills)
                    {
                        if (!s.Exhausted)
                            continue;
                        await d.Caster.CastProcedure(s, recursive: false);
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
                cast:                       async d =>
                {
                    await d.CycleProcedure(WuXing.Huo, gain: 3);
                    int stack = d.Caster.GetStackOfBuff("灼烧");
                    await d.GainArmorProcedure(2 * stack, induced: false);
                }),

            new(id:                         "0501",
                name:                       "寸劲",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(5 + dj) * 2 - 2}攻".ApplyAttack() +
                    $"\n遭受{3 + 2 * dj}软弱".ApplyDebuff(),
                cast:                       async d =>
                {
                    await d.AttackProcedure(Fib.ToValue(5 + d.Dj) * 2 - 2);
                    await d.GainBuffProcedure("软弱", 3 + 2 * d.Dj);
                }),

            new(id:                         "0502",
                name:                       "八极拳",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{Fib.ToValue(4 + dj)}".ApplyDefend() +
                    ($"\n终结：" + $"每1护甲，1攻".ApplyAttack()).ApplyCond(castResult),
                cast:                       async d =>
                {

                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            d.Value += Mathf.Max(0, d.Src.Armor);
                        });
                    
                    await d.GainArmorProcedure(Fib.ToValue(4 + d.Dj), induced: false);
                    bool isEnd = await d.Skill.IsEnd();
                    if (isEnd)
                        await d.AttackProcedure(1,
                            closures: new [] { closure });
                    d.CastResult.AppendCond(isEnd);
                }),

            new(id:                         "0503",
                name:                       "点穴",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(4 + dj)}攻".ApplyAttack() +
                    $"\n对手有灵气：多{Fib.ToValue(4 + dj)}攻".ApplyCond(castResult),
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            bool cond = d.Src.Opponent().GetStackOfBuff("灵气") > 0;
                            d.Value += cond ? Fib.ToValue(4 + d.SrcSkill.Dj) : 0;
                            d.CastResult.AppendCond(cond);
                        });

                    await d.AttackProcedure(Fib.ToValue(4 + d.Dj),
                        closures: new [] { closure });
                }),
            
            new(id:                         "0504",
                name:                       "流沙",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Defend | SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{3 + 2 * dj}".ApplyDefend() +
                    $"\n灵气+{1 + dj / 2}".ApplyMana(),
                cast:                       async d =>
                {
                    await d.GainArmorProcedure(3 + 2 * d.Dj, induced: false);
                    await d.GainBuffProcedure("灵气", 1 + d.Dj / 2);
                }),

            new(id:                         "0505",
                name:                       "一力降十会",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{(5 * dj * dj + 35 * dj + 50) / 2}攻".ApplyAttack() +
                    $"\n遭受4跳行动".ApplyDebuff() +
                    $"\n唯一攻击牌：免除".ApplyCond(castResult),
                cast:                       async d =>
                {
                    int value = (5 * d.Dj * d.Dj + 35 * d.Dj + 50) / 2;
                    await d.AttackProcedure(value);

                    bool cond = d.Skill.NoOtherAttack;
                    if (!cond)
                        await d.GainBuffProcedure("跳行动", 4);
                    d.CastResult.AppendCond(cond);
                }),
            
            new(id:                         "0506",
                name:                       "边腿",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{8 + 3 * dj}攻".ApplyAttack() +
                    $"\n消耗每1灵气，多{1 + dj}攻" +
                    $"\n终结：不消耗".ApplyCond(castResult),
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            int mana = d.Src.GetStackOfBuff("灵气");

                            bool cond = await d.SrcSkill.IsEnd();
                            if (!cond)
                                await d.Src.LoseBuffProcedure("灵气", mana);
                            
                            d.Value += mana * (1 + d.SrcSkill.Dj);
                            d.CastResult.AppendCond(cond);
                        });
                    
                    await d.AttackProcedure(8 + 3 * d.Dj,
                        closures: new [] { closure });
                }),

            new(id:                         "0513",
                name:                       "震脚",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cost:                       async (env, entity, skill, recursive) =>
                    new ChannelCostResult(await skill.IsEnd(true) ? 0 : 1),
                costDescription:            CostDescription.ChannelFromValue(1),
                skillTypeComposite:         SkillType.Attack | SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{7 + 5 * dj}攻".ApplyAttack() +
                    ($"\n击伤：" + "护甲+击伤值".ApplyDefend()).ApplyCond(castResult) +
                    $"终结：无需吟唱",
                cast:                       async d =>
                {

                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.GainArmorProcedure(d.Value, induced: true);
                            d.CastResult.AppendCond(true);
                        });
                    
                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(7 + 5 * d.Dj,
                        closures: new [] { closure });
                }),

            new(id:                         "0516",
                name:                       "冰肌",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"坚毅+{Fib.ToValue(4 + dj)}" +
                    $"\n遭受{Fib.ToValue(4 + dj) * 2}腐朽".ApplyDebuff(),
                cast:                       async d =>
                {
                    await d.CycleProcedure(WuXing.Tu, gain: Fib.ToValue(4 + d.Dj));
                    await d.GainBuffProcedure("腐朽", Fib.ToValue(4 + d.Dj) * 2);
                }),

            new(id:                         "0512",
                name:                       "箭疾步",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Mana | SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{2 + dj}".ApplyMana() +
                    $"\n每1灵气，护甲+{1 + dj}".ApplyDefend(),
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("灵气", 2 + d.Dj);
                    await d.GainArmorProcedure(d.Caster.GetStackOfBuff("灵气") * (1 + d.Dj), induced: false);
                }),
            
            new(id:                         "0521",
                name:                       "连环腿",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_ATTACK, -1, async (owner, closureDetails) =>
                    {
                        StageSkill s = owner as StageSkill;
                        AttackDetails d = (AttackDetails)closureDetails;

                        if (s.Owner != d.Src) return;

                        string key = "HighestAttackRecord";
                        s.Owner.Memory.PerformOperation(key, 0, record => Mathf.Max(record, d.Value));
                    }),
                },
                castDescription:            (j, dj, costResult, castResult) =>
                    (j >= JingJie.HuaShen ? $"造成本局最高攻，2次" : $"造成本局最高攻").ApplyAttack(),
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, -1,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            string key = "HighestAttackRecord";
                            int highestAttackRecord = d.Src.Memory.TryGetVariable(key, 0);
                            d.Value = Mathf.Max(d.Value, highestAttackRecord);
                        });

                    int times = d.J >= JingJie.HuaShen ? 2 : 1;

                    await d.AttackProcedure(1, times: times,
                        closures: new [] { closure });
                }),

            new(id:                         "0511",
                name:                       "玉骨",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"坚毅+{2 + dj}" +
                    $"\n每4坚毅，暴击+1",
                cast:                       async d =>
                {
                    await d.CycleProcedure(WuXing.Tu, 2 + d.Dj);
                    int stack = d.Caster.GetStackOfBuff("坚毅");
                    int add = stack / 4;
                    await d.GainBuffProcedure("暴击", add);
                }),

            new(id:                         "0510",
                name:                       "崩山掌",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Defend,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+20".ApplyDefend() +
                    $"\n下{1 + dj}次失去护甲时，返还",
                cast:                       async d =>
                {
                    await d.GainArmorProcedure(20, induced: false);
                    await d.GainBuffProcedure("护甲返还", 1 + d.Dj, induced: true);
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
                cast:                       async d =>
                {
                    bool cond0 = d.Caster.IsLowHealth || await d.Caster.IsFocused();
                    bool cond1 = (d.Caster.Armor <= 0) || await d.Caster.IsFocused();
                    bool cond2 = (d.Caster.GetStackOfBuff("灵气") == 0) || await d.Caster.IsFocused();
                    bool cond3 = (d.Caster.HasChannelRecord) || await d.Caster.IsFocused();
                    bool cond4 = (d.Caster.HasZhiQiRecord) || await d.Caster.IsFocused();
                    bool cond5 = (d.Caster.HasChanRaoRecord) || await d.Caster.IsFocused();
                    bool cond6 = (d.Caster.HasRuanRuoRecord) || await d.Caster.IsFocused();
                    bool cond7 = (d.Caster.HasNeiShangRecord) || await d.Caster.IsFocused();
                    bool cond8 = (d.Caster.HasFuXiuRecord) || await d.Caster.IsFocused();
                    bool cond9 = d.Caster.TriggeredJiaShiRecord || await d.Caster.JiaShiProcedure();
                    bool cond10 = d.Caster.TriggeredEndRecord || await d.Skill.IsEnd(useFocus: true);
                    bool cond11 = d.Caster.TriggeredFirstTimeRecord || await d.Skill.IsFirstTime(useFocus: true);

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
                    await d.AttackProcedure(1 << bitShift);
                    d.CastResult.AppendBools(cond0, cond1, cond2, cond3, cond4, cond5, cond6, cond7, cond8, cond9, cond10, cond11);
                }),

            new(id:                         "0515",
                name:                       "龟息",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.HuaShenOnly,
                cost:                       CostResult.ChannelFromValue(3),
                costDescription:            CostDescription.ChannelFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"坚毅+9",
                cast:                       async d =>
                {
                    await d.CycleProcedure(WuXing.Tu, gain: 9);
                }),
            
            #endregion

            #region 特殊牌
            
            new(id:                         "0000",
                name:                       "卡池已空",
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
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("灵气");
                }),

            new(id:                         "0002",
                name:                       "灵气匮乏",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQiOnly,
                castDescription:            (j, dj, costResult, castResult) =>
                    "灵气+1".ApplyMana(),
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("灵气");
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
                cast:                       async d =>
                {
                    await d.RemoveBuffProcedure("灵气", 3 + d.Dj, false);
                }),

            new(id:                         "0604",
                name:                       "爱恋",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"获得{2 + dj}集中",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("集中", 2 + d.Dj, false);
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
                cast:                       async d =>
                {
                    await d.HealProcedure(20 + 5 * d.Dj, induced: false);
                    await d.HealOppoProcedure(20 + 5 * d.Dj, induced: false);
                }),

            new(id:                         "0607",
                name:                       "枯木",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"双方遭受{5 + dj}腐朽".ApplyDebuff(),
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("腐朽", 5 + d.Dj);
                    await d.GiveBuffProcedure("腐朽", 5 + d.Dj);
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
                cast:                       async d =>
                {
                    bool cond = d.J <= JingJie.ZhuJi;
                    d.Caster.SetActionPoint(cond ? 2 : 3);
                }),

            new(id:                         "0601",
                name:                       "永远",
                wuXing:                     null,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Health,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"治疗{18 + dj * 6}".ApplyHeal(),
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.HealProcedure(18 + d.Dj * 6, induced: false);
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
                cast:                       async d =>
                {
                    bool cond = d.J <= JingJie.ZhuJi;
                    d.Caster.SetActionPoint(cond ? 2 : 3);
                }),

            new(id:                         "0611",
                name:                       "一心",
                wuXing:                     null,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Health,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"治疗{24 + dj * 6}".ApplyHeal(),
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.HealProcedure(24 + d.Dj * 6, induced: false);
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
                cast:                       async d =>
                {
                    BuffEntry[] buffEntries = new BuffEntry[] { "锋锐", "格挡", "力量", "闪避", "灼烧", };

                    foreach (BuffEntry buffEntry in buffEntries)
                    {
                        Buff b = d.Caster.FindBuff(buffEntry);
                        if (b != null)
                            await d.GainBuffProcedure(b.GetEntry(), 1 + d.Dj);
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
                cast:                       async d =>
                {
                    await d.AttackProcedure(5, times: 4);
                }),

            new(id:                         "0608",
                name:                       "玄武吐息法",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"升华\n生命回复至上限",
                withinPool:                 false,
                cast:                       async d =>
                {
                    int gap = d.Caster.MaxHp - d.Caster.Hp;
                    await d.Caster.HealProcedure(gap);
                    await d.Skill.ExhaustProcedure();
                }),

            new(id:                         "0609",
                name:                       "毒性",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.JinDan2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加3内伤".ApplyDebuff(),
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GiveBuffProcedure("内伤", 3);
                }),
            
            new(id:                         "0612",
                name:                       "观棋烂柯",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                cost:                       CostResult.ManaFromDj(dj => 1 - dj),
                costDescription:            CostDescription.ManaFromDj(dj => 1 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加1跳行动",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.Caster.GiveBuffProcedure("跳行动");
                }),

            #endregion
            
            #region 教程
            
            new(id:                         "0701",
                name:                       "冲撞",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{3 + dj}攻".ApplyAttack(),
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.AttackProcedure(3 + d.Dj);
                }),
            
            new(id:                         "0702",
                name:                       "劈砍",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{4 + 2 * dj}攻".ApplyAttack(),
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.AttackProcedure(4 + 2 * d.Dj);
                }),
            
            new(id:                         "0703",
                name:                       "冰弹",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQiOnly,
                skillTypeComposite:         SkillType.Attack,
                cost:                       CostResult.ManaFromValue(2),
                costDescription:            CostDescription.ManaFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{8 + dj}攻".ApplyAttack() +
                    $"\n格挡+1",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.AttackProcedure(8 + d.Dj);
                    await d.GainBuffProcedure("格挡");
                }),

            #endregion

            #region 待选池子

            new(id:                         "0114",
                name:                       "素弦",
                wuXing:                     WuXing.Jin,
                jingJieBound:               JingJie.JinDan2HuaShen,
                cost:                       CostResult.ChannelFromValue(1),
                costDescription:            CostDescription.ChannelFromValue(1),
                skillTypeComposite:         SkillType.Attack | SkillType.Mana,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n灵气+3".ApplyMana() +
                    $"\n下{1 + dj}次攻击也触发",
                withinPool:                 false,
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.GainBuffProcedure("灵气", 3);
                            await d.Src.GainBuffProcedure("素弦", 1 + d.SrcSkill.Dj);
                        });

                    await d.AttackProcedure(2,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0214",
                name:                       "苦寒",
                wuXing:                     WuXing.Shui,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack | SkillType.Swift,
                cost:                       CostResult.ManaFromValue(8),
                costDescription:            CostDescription.ManaFromValue(8),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n二动+1" +
                    $"\n下{2 + dj}次攻击也触发",
                withinPool:                 false,
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            if (d.Src.GetActionPoint() < 2)
                                d.Src.SetActionPoint(2);
                            else
                                await d.Src.GainBuffProcedure("二动");
                            await d.Src.GainBuffProcedure("苦寒", 2 + d.SrcSkill.Dj);
                        });

                    await d.AttackProcedure(2,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0313",
                name:                       "弱昙",
                wuXing:                     WuXing.Mu,
                jingJieBound:               JingJie.JinDan2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n力量+1" +
                    $"\n下{1 + dj}次攻击也触发",
                withinPool:                 false,
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.GainBuffProcedure("力量");
                            await d.Src.GainBuffProcedure("弱昙", 1 + d.SrcSkill.Dj);
                        });

                    await d.AttackProcedure(2,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0414",
                name:                       "狂焰",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"2攻".ApplyAttack() +
                    $"\n攻击多8攻".ApplyAttack() +
                    $"\n下{1 + dj}次攻击也触发",
                withinPool:                 false,
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_FULL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            d.Value += 8;
                            // await d.Src.GainBuffProcedure("狂焰", 1 + d.SrcSkill.Dj);
                        });
                    
                    await d.AttackProcedure(2,
                        closures: new [] { closure });
                    
                    await d.GainBuffProcedure("狂焰", 1 + d.Dj);
                }),

            new(id:                         "0514",
                name:                       "孤山",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.YuanYing2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                castDescription:            (j, dj, costResult, castResult) =>
                    (j < JingJie.HuaShen ? $"2攻".ApplyAttack() : "2攻x2".ApplyAttack()) +
                    $"\n不消耗剑阵效果",
                withinPool:                 false,
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            BuffEntry[] buffs = new BuffEntry[] { "素弦", "苦寒", "弱昙", "狂焰" };

                            bool cond = d.SrcSkill.GetJingJie() < JingJie.HuaShen;
                            int times = cond ? 1 : 2;
                            foreach (BuffEntry b in buffs)
                                if (d.Src.GetStackOfBuff(b) > 0)
                                    await d.Src.GainBuffProcedure(b, times, induced: true);
                        });
                    
                    bool cond = d.J < JingJie.HuaShen;
                    int times = cond ? 1 : 2;
                    await d.AttackProcedure(2, times,
                        closures: new [] { closure });
                }),
            
            new(id:                         "0507",
                name:                       "罗刹",
                wuXing:                     WuXing.Tu,
                jingJieBound:               JingJie.ZhuJi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                withinPool:                 false,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{20 + 20 * dj}攻".ApplyAttack() +
                    $"\n遭受{3 + 2 * dj}腐朽".ApplyDebuff(),
                cast:                       async d =>
                {
                    await d.AttackProcedure(20 + 20 * d.Dj);
                    await d.GainBuffProcedure("腐朽", stack: 3 + 2 * d.Dj, induced: true);
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
                cast:                       async d =>
                {
                    await d.AttackProcedure(40 + 20 * d.Dj);
                }),
            
            new(id:                         "0421",
                name:                       "多段测试",
                wuXing:                     WuXing.Huo,
                jingJieBound:               JingJie.LianQi2HuaShen,
                skillTypeComposite:         SkillType.Attack,
                withinPool:                 false,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻".ApplyAttack() +
                    $"\n每耗1灵气，多1次".ApplyAttack(),
                cast:                       async d =>
                {
                    int times = d.Caster.GetStackOfBuff("灵气").ClampLower(1);
                    await d.LoseBuffProcedure("灵气", times);
                    await d.AttackProcedure(1, times: times);
                }),
            
            // new(id:                         "0101",
            //     name:                       "乘风",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.LianQi2HuaShen,
            //     skillTypeComposite:         SkillType.Attack,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"{5 + dj}攻\n" +
            //         $"若有锋锐：{3 + dj}攻".ApplyCond(castResult),
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            // new(id:                         "0107",
            //     name:                       "飞絮",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.ZhuJi2HuaShen,
            //     skillTypeComposite:         null,
            //     cost:                       CostResult.ManaFromValue(1),
            //     costDescription:            CostDescription.ManaFromValue(1),
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"奇偶：" +
            //         $"施加{8 + 2 * dj}破甲".ApplyOdd(castResult) +
            //         $"/" +
            //         $"锋锐+{1 + dj}".ApplyEven(castResult),
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            // new(id:                         "0120",
            //     name:                       "千里神行符",
            //     wuXing:                     WuXing.Jin,
            //     jingJieBound:               JingJie.HuaShenOnly,
            //     skillTypeComposite:         SkillType.Exhaust | SkillType.Swift,
            //     castDescription:            (j, dj, costResult, castResult) =>
            //         $"奇偶：" +
            //         $"升华".ApplyOdd(castResult) +
            //         $"/" +
            //         $"二动".ApplyEven(castResult) +
            //         $"\n灵气+4",
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //         $"初次：遭受1跳行动".ApplyCond(castResult),
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainArmorProcedure(10 + 4 * skill.Dj, induced: false);
            //         bool cond = !await skill.IsFirstTime();
            //         if (!cond)
            //             await caster.GainBuffProcedure("跳行动");
            //         return cond.ToCastResult();
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
            //     cast:                       async d =>
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
            //         $"升华" +
            //         $"\n每轮：闪避补至1",
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //         $"升华" +
            //         $"\n灼烧+{2 + dj}",
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
            //     {
            //         bool cond0 = skill.NoAttackAdjacents || await caster.IsFocused();
            //         bool cond1 = await caster.TryConsumeProcedure("灵气") || await caster.IsFocused();
            //         int bitShift = 0;
            //         bitShift += cond0 ? 1 : 0;
            //         bitShift += cond1 ? 1 : 0;
            //         await caster.AttackProcedure((8 + 2 * skill.Dj) << bitShift);
            //         return Style.CastResultFromBools(cond0, cond1);
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n三动 升华",
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n护甲+20\n坚毅+2",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await caster.GainArmorProcedure(20, induced: false);
            //         await caster.GainBuffProcedure("坚毅", 2);
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n遭受1不堪一击，永久二重+1",
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n获得灵气时：每1，气血+3",
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n永久免费+1",
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n下次受到攻击时，对对方施加等量破甲",
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n回合被跳过时，该回合无法受到伤害\n遭受12跳行动",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("浮空艇");
            //         await caster.GainBuffProcedure("跳行动", 12);
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     cast:                       async d =>
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
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n攻击时，护甲+3",
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n力量+8 灵气+8\n8回合后死亡",
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n使用灵气牌时，获得二动",
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n对方二动时，如果没有暴击，获得1",
            //     withinPool:                 false,
            //     cast:                       async d =>
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
            //     castDescription:            (j, dj, costResult, castResult) => "枯竭\n升华\n成功闪避时，如果对方没有跳行动，施加1",
            //     withinPool:                 false,
            //     cast:                       async d =>
            //     {
            //         await skill.ExhaustProcedure();
            //         await caster.GainBuffProcedure("飞行器");
            //         return null;
            //     }),

            #endregion
            
            #region 怪物专属
            
            // 3 5 8 13 21
            new(id:                         "1001",
                name:                       "攻击",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{Fib.ToValue(4 + dj)}攻",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.AttackProcedure(Fib.ToValue(4 + d.Dj));
                }),
            
            // 3 5 8 13 21
            new(id:                         "1002",
                name:                       "防御",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{Fib.ToValue(4 + dj)}",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainArmorProcedure(Fib.ToValue(4 + d.Dj), false);
                }),
            
            // 3 5 8 13 21
            new(id:                         "1003",
                name:                       "聚灵",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+{1 + (dj / 2)}",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("灵气", 1 + (d.Dj / 2), induced: false);
                }),
            
            // 3 5 8 13 21
            new(id:                         "1004",
                name:                       "调息",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"生命+{Fib.ToValue(4 + dj)}",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.HealProcedure(Fib.ToValue(4 + d.Dj), false);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1101",
                name:                       "啃咬",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加{6 << dj}破甲",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.RemoveArmorProcedure(6 << d.Dj, false);
                }),

            // 6 12 26 52 102
            new(id:                         "1102",
                name:                       "切割",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{2 << dj}攻".ApplyAttack() +
                    $"\n获得{4 << dj}护甲",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.AttackProcedure(2 << d.Dj);
                    await d.GainArmorProcedure(4 << d.Dj, true);
                }),

            // 8 24 52 105 204
            new(id:                         "1103",
                name:                       "腐蚀",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+5" +
                    $"\n每有1护甲，施加1破甲",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainArmorProcedure(5, induced: false);
                    int value = d.Caster.Armor.ClampLower(0);
                    await d.RemoveArmorProcedure(value, induced: true);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1104",
                name:                       "剑芒",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"锋锐+{1 << (Mathf.Max(dj - 1, 0))}",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("锋锐", 1 << (Mathf.Max(d.Dj - 1, 0)));
                }),

            // 6 12 26 52 102
            new(id:                         "1105",
                name:                       "祥瑞",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"开局：二动 双发",
                withinPool:                 false,
                startStageCast:             async d =>
                {
                    await d.Caster.GainBuffProcedure("二动");
                    await d.Caster.GainBuffProcedure("一念无量劫");
                },
                cast:                       async d =>
                {
                }),

            // 8 24 52 105 204
            new(id:                         "1106",
                name:                       "万千光辉",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"锋锐+1" +
                    $"\n每1锋锐，多{2 + dj}攻",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("锋锐");
                    int value = d.Caster.GetStackOfBuff("锋锐") * (2 + d.Dj);
                    await d.AttackProcedure(value);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1107",
                name:                       "恶意",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"之后{1 + dj}次施加破甲时将会造成生命流失",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("恶意", 1 + d.Dj);
                }),

            // 6 12 26 52 102
            new(id:                         "1108",
                name:                       "岁月",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{10 + 10 * dj}攻".ApplyAttack() +
                    $"\n击伤：施加{3 + 2 * dj}腐朽".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.GiveBuffProcedure("腐朽", 3 + 2 * d.SrcSkill.Dj, induced: true);
                            d.CastResult.AppendCond(true);
                        });

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(10 + 10 * d.Dj,
                        closures: new [] { closure });
                }),

            // 8 24 52 105 204
            new(id:                         "1109",
                name:                       "恶灵招徕",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"若敌方有腐朽，施加10破甲".ApplyStyle(castResult, "0") +
                    $"\n否则，将破甲转为腐朽".ApplyStyle(castResult, "1"),
                withinPool:                 false,
                cast:                       async d =>
                {
                    bool cond = d.Caster.Opponent().GetStackOfBuff("腐朽") > 0;
                    
                    if (cond)
                    {
                        await d.RemoveArmorProcedure(10, induced: false);
                    }
                    else
                    {
                        int value = -d.Caster.Opponent().Armor;
                        await d.GiveBuffProcedure("腐朽", value, induced: false);
                        await d.GiveArmorProcedure(value, induced: true);
                    }
                    
                    d.CastResult.AppendBools(cond, !cond);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1201",
                name:                       "浪击",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{4 + 4 * dj}攻" +
                    $"\n击伤：灵气+{2 + dj}".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.GainBuffProcedure("灵气", 2 + d.SrcSkill.Dj);
                            d.CastResult.AppendCond(true);
                        });

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(4 + 4 * d.Dj,
                        closures: new [] { closure });
                }),

            // 6 12 26 52 102
            new(id:                         "1202",
                name:                       "惊涛",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"4攻" +
                    $"\n爆能{10 + 2 * dj}：多{4 + 2 * dj}攻，多{1 + dj}次，吸血".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async d =>
                {
                    bool cond = await d.TryConsumeProcedure("灵气", 10 + 2 * d.Dj);
                    d.CastResult.AppendCond(cond);
                    
                    if (cond)
                    {
                        StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                            async (owner, closureDetails) =>
                            {
                                AttackDetails d = closureDetails as AttackDetails;
                                if (owner != d.SrcSkill) return;
                                d.LifeSteal = true;
                            });
                        await d.AttackProcedure(4 + 4 + 2 * d.Dj, 1 + 1 + d.Dj,
                            closures: new[] { closure });
                    }
                    else
                    {
                        await d.AttackProcedure(4);
                    }
                }),

            // 8 24 52 105 204
            new(id:                         "1203",
                name:                       "放血",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加4内伤",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GiveBuffProcedure("内伤", 4);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1204",
                name:                       "高速",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"二动" +
                    $"\n每1格挡，造成{1 + dj}伤害",
                withinPool:                 false,
                cast:                       async d =>
                {
                    int value = d.Caster.GetStackOfBuff("格挡") * d.Dj;
                    await d.AttackProcedure(value);
                    d.Caster.SetActionPoint(2);
                }),

            // 6 12 26 52 102
            new(id:                         "1205",
                name:                       "逍遥游",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"格挡减半" +
                    $"\n格挡+{2 + 2 * dj}",
                withinPool:                 false,
                cast:                       async d =>
                {
                    int value = d.Caster.GetStackOfBuff("格挡") / 2;
                    await d.LoseBuffProcedure("格挡", value);
                    await d.GainBuffProcedure("格挡", 2 + 2 * d.Dj);
                }),

            // 8 24 52 105 204
            new(id:                         "1206",
                name:                       "爱睡",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       CostResult.ChannelFromDj(dj => 4 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 4 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"生命回满",
                withinPool:                 false,
                cast:                       async d =>
                {
                    int value = d.Caster.MaxHp - d.Caster.Hp;
                    await d.HealProcedure(value, induced: false);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1207",
                name:                       "幻雾",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       async (env, entity, skill, recursive) => new ChannelCostResult(4 * (1 - skill.StageCastedCount.Clamp(0, 1))),
                costDescription:            CostDescription.ChannelFromValue(4),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"30攻 吸血" +
                    $"\n非初次：无需吟唱".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async d =>
                {
                    bool cond = d.Skill.StageCastedCount != 0;
                    d.CastResult.AppendCond(cond);
                    
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            d.LifeSteal = true;
                        });
                    await d.AttackProcedure(30,
                        closures: new[] { closure });
                }),

            // 6 12 26 52 102
            new(id:                         "1208",
                name:                       "须臾",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"十二动 升华",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.Skill.ExhaustProcedure();
                    d.Caster.SetActionPoint(12);
                }),

            // 8 24 52 105 204
            new(id:                         "1209",
                name:                       "月华清辉",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"敌方失去所有灵气",
                withinPool:                 false,
                cast:                       async d =>
                {
                    int value = d.Caster.Opponent().GetStackOfBuff("灵气");
                    await d.RemoveBuffProcedure("灵气", value, induced: false);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1301",
                name:                       "木刺",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       CostResult.ChannelFromValue(2),
                costDescription:            CostDescription.ChannelFromValue(2),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{10 + 10 * dj}攻 穿透",
                withinPool:                 false,
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            d.Penetrate = true;
                        });

                    await d.AttackProcedure(10 + 10 * d.Dj,
                        closures: new [] { closure });
                }),

            // 6 12 26 52 102
            new(id:                         "1302",
                name:                       "滑水",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"闪避+{1 + dj}",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("闪避", 1 + d.Dj);
                }),

            // 8 24 52 105 204
            new(id:                         "1303",
                name:                       "驱藤",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"双方坚毅+{2 + dj}",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("坚毅", 2 + d.Dj, induced: false);
                    await d.GiveBuffProcedure("坚毅", 2 + d.Dj, induced: true);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1304",
                name:                       "花海",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"升华" +
                    $"\n每回合力量+1",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.Skill.ExhaustProcedure();
                    await d.GainBuffProcedure("花海");
                }),

            // 6 12 26 52 102
            new(id:                         "1305",
                name:                       "啄击",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"1攻" +
                    $"\n成长：多1次",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.AttackProcedure(1, times: 1 + d.Skill.StageCastedCount);
                }),

            // 8 24 52 105 204
            new(id:                         "1306",
                name:                       "娑婆双树",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"将左边牌的成长次数给右边牌",
                withinPool:                 false,
                cast:                       async d =>
                {
                    StageSkill leftSkill = d.Skill.Prev(false);
                    StageSkill rightSkill = d.Skill.Next(false);
                    if (leftSkill == null || rightSkill == null)
                        return;

                    rightSkill.SetStageCastedCount(leftSkill.StageCastedCount + rightSkill.StageCastedCount);
                    leftSkill.SetStageCastedCount(0);
                    // animation
                }),
            
            // 6 12 26 52 102
            new(id:                         "1307",
                name:                       "灵虚步",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       CostResult.ManaFromValue(3),
                costDescription:            CostDescription.ManaFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"闪避+3" +
                    $"\n成功闪避时：双发+1",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("闪避", 3);
                    await d.GainBuffProcedure("灵虚步", induced: true);
                }),

            // 6 12 26 52 102
            new(id:                         "1308",
                name:                       "灵犀剑",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灵气+3" +
                    $"\n1攻 每1灵气，多{1 + dj}",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("灵气", 3);
                    
                    StageClosure closure = new(StageClosureDict.WIL_ATTACK, 0,
                        async (owner, closureDetails) =>
                        {
                            AttackDetails d = closureDetails as AttackDetails;
                            if (owner != d.SrcSkill) return;
                            int mana = d.Src.GetStackOfBuff("灵气");
                            d.Value += mana * (1 + d.SrcSkill.Dj);
                        });
                    
                    await d.AttackProcedure(1,
                        closures: new [] { closure });
                }),

            // 8 24 52 105 204
            new(id:                         "1309",
                name:                       "他心通",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"下一次对方获得增益时：自己也获得",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("他心通");
                }),
            
            // 6 12 26 52 102
            new(id:                         "1401",
                name:                       "吞炎",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"成为残血" +
                    $"\n护甲+{10 + 20 * dj}",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainArmorProcedure(10 + 20 * d.Dj, induced: false);
                    await d.BecomeLowHealth(induced: true);
                }),

            // 6 12 26 52 102
            new(id:                         "1402",
                name:                       "焚天",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{10 + 10 * dj}攻" +
                    $"\n残血：多{10 + 10 * dj}攻".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async d =>
                {
                    int value = (10 + 10 * d.Dj) + (d.Caster.IsLowHealth ? (10 + 10 * d.Dj) : 0);
                    await d.AttackProcedure(value);
                }),

            // 8 24 52 105 204
            new(id:                         "1403",
                name:                       "常夏",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{3 + dj}攻 每携带1张火，多1次",
                withinPool:                 false,
                cast:                       async d =>
                {
                    int value = d.Caster.CountSuch(s => s.Entry.WuXing == WuXing.Huo);
                    await d.AttackProcedure(3 + d.Dj, times: 1 + value);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1404",
                name:                       "天劫火",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       CostResult.HealthFromDj(dj => 2 + 2 * dj),
                costDescription:            CostDescription.HealthFromDj(dj => 2 + 2 * dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"灼烧+{2 + dj}",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("灼烧", 2 + d.Dj);
                }),

            // 6 12 26 52 102
            new(id:                         "1405",
                name:                       "火墙",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"若在下次使用前，没有遭受{1 + dj}次伤害".ApplyCond(castResult) +
                    $"\n50攻".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async d =>
                {
                    int stack = d.Caster.GetStackOfBuff("火墙");
                    bool cond = stack > 0;
                    d.CastResult.AppendCond(cond);

                    if (cond)
                    {
                        await d.Caster.RemoveBuffProcedure("火墙", stack);
                        await d.AttackProcedure(50);
                    }

                    await d.Caster.GainBuffProcedure("火墙", 1 + d.Dj);
                }),

            // 8 24 52 105 204
            new(id:                         "1406",
                name:                       "献祭",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"升华所有卡牌",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.Caster._skills.Do(async s => await s.ExhaustProcedure());
                }),

            // 6 12 26 52 102
            new(id:                         "1407",
                name:                       "须弥",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+{10 + 10 * dj}" +
                    $"\n二动 升华",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainArmorProcedure(10 + 10 * d.Dj, induced: false);
                    d.Caster.SetActionPoint(2);
                    await d.Skill.ExhaustProcedure();
                }),
            
            // 6 12 26 52 102
            new(id:                         "1408",
                name:                       "仙人抚顶",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       CostResult.ChannelFromDj(dj => 4 - dj),
                costDescription:            CostDescription.ChannelFromDj(dj => 4 - dj),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"使用3次后：将对方生命变为0".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("仙人抚顶");
                    bool cond = d.Caster.GetStackOfBuff("仙人抚顶") >= 3;
                    d.CastResult.AppendCond(cond);
                    if (cond)
                    {
                        d.Caster.Opponent().Hp = 0;
                    }
                }),

            // 8 24 52 105 204
            new(id:                         "1409",
                name:                       "消愁",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"下一张牌取消升华",
                withinPool:                 false,
                cast:                       async d =>
                {
                    StageSkill s = d.Skill.Next(loop: false);
                    s.Exhausted = false;
                }),
            
            // 6 12 26 52 102
            new(id:                         "1501",
                name:                       "滚石",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n有护甲：多5攻".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async d =>
                {
                    bool cond = d.Caster.Armor > 0;
                    d.CastResult.AppendCond(cond);

                    int value = 10 + (cond ? 1 : 0) * 5;
                    await d.AttackProcedure(value);
                }),

            // 6 12 26 52 102
            new(id:                         "1502",
                name:                       "瓮城",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       CostResult.ChannelFromValue(3),
                costDescription:            CostDescription.ChannelFromValue(3),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"护甲+10" +
                    $"\n每有{6 - dj}生命，多1",
                withinPool:                 false,
                cast:                       async d =>
                {
                    int value = d.Caster.Hp / (6 - d.Dj) + 10;
                    await d.GainArmorProcedure(value, induced: false);
                }),

            // 8 24 52 105 204
            new(id:                         "1503",
                name:                       "风魔",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n遭受1跳走步",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.AttackProcedure(10);
                    await d.GainBuffProcedure("跳走步");
                }),
            
            // 6 12 26 52 102
            new(id:                         "1504",
                name:                       "震地",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"10攻" +
                    $"\n击伤：对手灵气-2".ApplyCond(castResult),
                withinPool:                 false,
                cast:                       async d =>
                {
                    StageClosure closure = new(StageClosureDict.DID_DAMAGE, 0,
                        async (owner, closureDetails) =>
                        {
                            DamageDetails d = closureDetails as DamageDetails;
                            if (owner != d.SrcSkill) return;
                            await d.Src.RemoveBuffProcedure("灵气", 2);
                            d.CastResult.AppendCond(true);
                        });

                    d.CastResult.AppendCond(false);
                    await d.AttackProcedure(10,
                        closures: new [] { closure });
                }),

            // 6 12 26 52 102
            new(id:                         "1505",
                name:                       "硬化",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"坚毅+2" +
                    $"\n失去所有护甲",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("坚毅", 2);
                    int value = d.Caster.Armor;
                    if (value > 0)
                        await d.LoseArmorProcedure(value, induced: true);
                }),

            // 8 24 52 105 204
            new(id:                         "1506",
                name:                       "怒瞳",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加4滞气",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GiveBuffProcedure("滞气", 4);
                }),
            
            // 6 12 26 52 102
            new(id:                         "1507",
                name:                       "金刚不坏",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"升华\n受到伤害时：最多20",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.GainBuffProcedure("金刚不坏", 20);
                    await d.Skill.ExhaustProcedure();
                }),

            // 6 12 26 52 102
            new(id:                         "1508",
                name:                       "钢拳",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                cost:                       async (env, entity, skill, recursive) =>
                    new ChannelCostResult(4 - entity.Opponent().CountSuchBuff(b => !b.GetEntry().Friendly)),
                costDescription:            CostDescription.ChannelFromValue(4),
                castDescription:            (j, dj, costResult, castResult) =>
                    $"{40 + 40 * dj}攻" +
                    $"\n对手每有1种debuff，吟唱-1",
                withinPool:                 false,
                cast:                       async d =>
                {
                    await d.AttackProcedure(40 + 40 * d.Dj);
                }),

            // 8 24 52 105 204
            new(id:                         "1509",
                name:                       "天人五衰",
                wuXing:                     null,
                jingJieBound:               JingJie.LianQi2HuaShen,
                castDescription:            (j, dj, costResult, castResult) =>
                    $"施加滞气，缠绕，软弱，腐朽，内伤各5层",
                withinPool:                 false,
                cast:                       async d =>
                {
                    BuffEntry[] buffs = new BuffEntry[] { "滞气", "缠绕", "软弱", "腐朽", "内伤" };
                    for (int i = 0; i < buffs.Length; i++)
                        await d.GiveBuffProcedure(buffs[i], 5, induced: i != 0);
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
