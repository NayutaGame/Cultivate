
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class BuffCategory : Category<BuffEntry>
{
    public BuffCategory()
    {
        AddRange(new List<BuffEntry>()
        {
            // new("治疗转灵气", "受到治疗时：灵气+[Stack]", BuffStackRule.Add, true, false,
            //     healed: async (buff, d) => await d.Tgt.BuffSelfProcedure("灵气", buff.Stack)),
            // new("治疗转二动", "被治疗时，如果实际治疗>=20，二动", BuffStackRule.Wasted, true, false,
            //     healed: (buff, d) =>
            //     {
            //         int actualHealed = Mathf.Min(d.Tgt.MaxHp - d.Tgt.Hp, d.Value);
            //         d.Tgt.Swift |= actualHealed >= 20;
            //     }),

            new("不存在的Buff", "不存在的Buff", BuffStackRule.Add, true, false),
            new("灵气", "可以消耗灵气使用技能", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.GAIN_BUFF, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                            d.Value -= b.Stack;
                    }),
                }),

            new("春雨", "下1次治疗时，效果变为1.5倍", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_HEAL, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        HealDetails d = (HealDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                        {
                            d.Value = (int)1.5f * d.Value;
                            await b.SetDStack(-1);
                        }
                    }),
                }),

            new("枯木", "回合结束时，受到{2 + dj}减甲", BuffStackRule.Add, false, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.END_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner == d.Owner)
                        {
                            await b.Owner.ArmorLoseSelfProcedure(b.Stack);
                        }
                    }),
                }),

            new("脆弱", "受到攻击：攻击力+[层数]", BuffStackRule.Add, false, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (d.Pierce) return;
                        if (b.Owner == d.Tgt && d.Src != d.Tgt)
                        {
                            d.Value += b.Stack;
                        }
                    }),
                }),

            new("软弱", "攻击时，少[层数]攻", BuffStackRule.Add, false, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                            d.Value -= b.Stack;
                    }),
                }),

            new("跳回合", "跳过回合", BuffStackRule.Add, false, false),
            new("跳卡牌", "行动时跳过下张卡牌", BuffStackRule.Add, false, false),
            new("双发", "下一张牌使用两次", BuffStackRule.Add, true, false),
            new("永久双发", "所有牌使用两次", BuffStackRule.One, true, false),
            new("二动", "下一张牌二动", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        d.Owner.Swift = true;
                        await b.SetDStack(-1);
                    }),
                }),
            new("免费", "下一次耗蓝时无需灵气", BuffStackRule.Add, true, false),
            new("永久免费", "所有牌无需灵气", BuffStackRule.One, true, false),
            new("集中", "下一次使用牌时，条件算作激活", BuffStackRule.Add, true, false),
            new("永久集中", "所有牌，条件算作激活", BuffStackRule.Add, true, false),
            new("六爻化劫", "第二轮开始时，双方重置生命上限，回[层数]%血", BuffStackRule.Max, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_ROUND, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;

                        StageEntity self = b.Owner;
                        StageEntity oppo = b.Owner.Opponent();

                        int selfMaxHp = Mathf.Max(self.MaxHp, self.RunEntity.GetFinalHealth());
                        int oppoMaxHp = Mathf.Max(oppo.MaxHp, oppo.RunEntity.GetFinalHealth());
                        self.MaxHp = self.RunEntity.GetFinalHealth();
                        oppo.MaxHp = oppo.RunEntity.GetFinalHealth();

                        int selfHpGap = self.MaxHp - (int)((float)selfMaxHp * b.Stack / 100);
                        int oppoHpGap = oppo.MaxHp - (int)((float)oppoMaxHp * b.Stack / 100);

                        await self.HealProcedure(selfHpGap);
                        await oppo.HealProcedure(oppoHpGap);

                        await self.RemoveBuff(b);
                    }),
                }),

            new("灵气回收", "下一次灵气减少时，加回", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_DISPEL, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DispelDetails d = (DispelDetails)eventDetails;
                        if (b.Owner != d.Tgt) return;
                        if (d._buffEntry.Name != "灵气") return;

                        await d.Tgt.BuffSelfProcedure("灵气", d._stack);
                        await b.SetDStack(-1);
                    }),
                }),

            new("护甲回收", "下一次护甲减少时，加回", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.ARMOR_DID_LOSE, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ArmorLoseDetails d = (ArmorLoseDetails)eventDetails;
                        if (b.Owner == d.Tgt)
                        {
                            await b.Owner.ArmorGainSelfProcedure(d.Value);
                            await b.SetDStack(-1);
                        }
                    }),
                }),

            new("禁止治疗", "禁止治疗", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_HEAL, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        HealDetails d = (HealDetails)eventDetails;
                        if (b.Owner == d.Tgt)
                            d.Cancel = true;
                    }),
                }),

            new("长明灯", "获得灵气时：每1，生命+3", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_BUFF, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        BuffDetails d = (BuffDetails)eventDetails;
                        if (b.Owner != d.Tgt) return;
                        if (d._buffEntry.GetName() != "灵气") return;
                        await b.Owner.HealProcedure(d._stack * 3);
                    }),
                }),

            new("尖刺陷阱", "下次受到攻击时，对对方施加等量减甲", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_ATTACK, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)eventDetails;
                        if (b.Owner != d.Tgt || d.Src == d.Tgt) return;
                        await b.Owner.ArmorLoseOppoProcedure(d.Value);
                    }),
                }),

            new("回合力量", "每回合：力量+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_TURN, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)eventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.Owner.BuffSelfProcedure("力量", b.Stack);
                    }),
                }),

            new("浮空艇", "本场战斗中，回合被跳过后，生命及上线无法下降", BuffStackRule.Add, true, false),

            new("回合免疫", "此回合无法收到伤害", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_TURN, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)eventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.Owner.RemoveBuff(b);
                    }),
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_DAMAGE, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)eventDetails;
                        if (b.Owner == d.Tgt)
                            d.Cancel = true;
                    }),
                }),

            new("外骨骼", "每次攻击前，护甲+3", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_ATTACK, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)eventDetails;
                        if (b.Owner != d.Src) return;
                        await b.Owner.ArmorGainSelfProcedure(3 * b.Stack);
                    }),
                }),

            new("永动机", "[层数]回合后死亡", BuffStackRule.Min, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_TURN, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)eventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.SetDStack(-1);
                        if (b.Owner.GetStackOfBuff("永动机") == 0)
                            await b.Owner.LoseHealthProcedure(b.Owner.Hp);
                    }),
                }),

            new("火箭靴", "使用灵气牌时，获得二动", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.END_STEP, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StepDetails d = (StepDetails)eventDetails;
                        if (b.Owner != d.Owner) return;
                        if (d.Skill != null && d.Skill.GetSkillType().Contains(SkillType.LingQi))
                            b.Owner.Swift = true;
                    }),
                }),

            new("定龙桩", "对方二动时，如果没有暴击，获得1", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_SWIFT, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        SwiftDetails d = (SwiftDetails)eventDetails;
                        if (b.Owner == d.Owner) return;
                        if (b.Owner.GetStackOfBuff("暴击") == 0)
                            await b.Owner.BuffSelfProcedure("暴击");
                    }),
                }),

            new("飞行器", "成功闪避时，如果对方没有跳回合，施加1", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_EVADE, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        EvadeDetails d = (EvadeDetails)eventDetails;
                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.GetStackOfBuff("跳回合") == 0)
                            await b.Owner.BuffSelfProcedure("跳回合");
                    }),
                }),

            new("时光机", "使用一张牌前，升级", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_EXECUTE, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ExecuteDetails d = (ExecuteDetails)eventDetails;
                        if (b.Owner != d.Caster || d.Caster != d.Skill.Owner) return;
                        await d.Skill.TryUpgradeJingJie();
                    }),
                }),

            new("延迟护甲", "下回合护甲+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        await b.Owner.ArmorGainSelfProcedure(b.Stack);
                        await b.Owner.RemoveBuff(b);
                    }),
                }),

            new("无常已至", "造成伤害：施加[伤害值，最多Stack]减甲", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;
                        if (!(b.Owner == d.Src && d.Src != d.Tgt))
                            return;
                        await b.Owner.ArmorLoseOppoProcedure(Mathf.Min(d.Value, b.Stack));
                    }),
                }),

            new("锋锐", "每回合：[层数]攻\n受到伤害后层数-1", BuffStackRule.Add, true, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.END_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        await b.Owner.AttackProcedure(b.Stack, wuXing: WuXing.Jin);
                    }),
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            b.SetDStack(-1);
                    }),
                }),
            new("森罗万象", "奇偶同时激活两个效果", BuffStackRule.One, true, false),

            new("自动灵气", "每回合：灵气+[层数]", BuffStackRule.Add, true, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        await b.Owner.BuffSelfProcedure("灵气", b.Stack);
                    }),
                }),
            new("敛息", "造成伤害时：取消伤害，施加减甲", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;

                        if (b.Owner == d.Src && d.Src != d.Tgt)
                        {
                            d.Cancel = true;
                            await b.Owner.ArmorLoseOppoProcedure(d.Value);
                            await b.SetDStack(-1);
                        }
                    }),
                }),

            new("吸血", "下一次攻击造成伤害时，回复生命", BuffStackRule.Add, true, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src)
                        {
                            d.LifeSteal = true;
                            await b.SetDStack(-1);
                        }
                    }),
                }),
            new("凝神", "下一次受到治疗：护甲+治疗量", BuffStackRule.Add, true, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_HEAL, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        HealDetails d = (HealDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                        {
                            await b.Owner.ArmorGainSelfProcedure(d.Value);
                            await b.SetDStack(-1);
                        }
                    }),
                }),
            new("永久吸血", "攻击一直具有吸血，直到使用非攻击牌", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STEP, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StepDetails d = (StepDetails)stageEventDetails;
                        if (b.Owner == d.Owner)
                        {
                            if (!d.Skill.GetSkillType().Contains(SkillType.Attack))
                                await b.SetDStack(-1);
                        }
                    }),
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src)
                        {
                            d.LifeSteal = true;
                        }
                    }),
                }),

            new("缠绕", "无法二动/三动\n每回合：层数-1", BuffStackRule.Add, false, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_SWIFT, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        SwiftDetails d = (SwiftDetails)stageEventDetails;
                        if (b.Owner == d.Owner)
                            d.Cancel = true;
                    }),
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.END_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.SetDStack(-1);
                    }),
                }),
            new("永久缠绕", "无法二动/三动", BuffStackRule.One, false, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_SWIFT, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        SwiftDetails d = (SwiftDetails)stageEventDetails;
                        if (b.Owner == d.Owner)
                            d.Cancel = true;
                    }),
                }),
            new("玄武吐息法", "治疗可以穿上限", BuffStackRule.Add, true, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_HEAL, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        HealDetails d = (HealDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            d.Penetrate = true;
                    }),
                }),
            new("格挡", "受到攻击：攻击力-[层数]", BuffStackRule.Add, true, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (d.Pierce) return;
                        if (b.Owner == d.Tgt && d.Src != d.Tgt)
                        {
                            d.Value -= b.Stack;
                        }
                    }),
                }),
            new("自动格挡", "每轮：格挡+[层数]", BuffStackRule.Add, true, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_ROUND, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner == d.Owner)
                            await b.Owner.BuffSelfProcedure("格挡", b.Stack);
                    }),
                }),

            new("闪避", "受到攻击时，减少1层，忽略此次攻击", BuffStackRule.Add, true, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Tgt && d.Src != d.Tgt)
                        {
                            d.Evade = true;
                            await b.SetDStack(-1);
                        }
                    }),
                }),
            new("自动闪避", "每轮：闪避补至[层数]", BuffStackRule.Add, true, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_ROUND, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner == d.Owner)
                            await b.Owner.BuffSelfProcedure("闪避", b.Stack - b.Owner.GetStackOfBuff("闪避"));
                    }),
                }),
            new("穿透", "下一次攻击时，忽略对方护甲/闪避/格挡", BuffStackRule.Add, true, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                        {
                            d.Pierce = true;
                            await b.SetDStack(-1);
                        }
                    }),
                }),
            new("永久穿透", "所有牌攻击时，忽略对方护甲/闪避/格挡", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                            d.Pierce = true;
                    }),
                }),
            new("力量", "攻击时，多[层数]攻", BuffStackRule.Add, true, true,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                            d.Value += b.Stack;
                    }),
                }),
            new("回马枪", "下次受攻击后：[层数]攻 穿透", BuffStackRule.Max, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (!d.Recursive) return;
                        if (b.Owner == d.Tgt && d.Src != d.Tgt)
                        {
                            await b.Owner.AttackProcedure(b.Stack, wuXing: WuXing.Mu, recursive: false);
                            await b.Owner.RemoveBuff(b);
                        }
                    }),
                }),

            new("天衣无缝", "每回合：[层数]攻", BuffStackRule.Max, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await d.Owner.AttackProcedure(b.Stack, wuXing: WuXing.Huo);
                    }),
                }),
            new("业火", "消耗牌时：使用2次", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_EXHAUST, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ExhaustDetails d = (ExhaustDetails)stageEventDetails;
                        if (b.Owner == d.Owner)
                            await d.Skill.Execute(d.Owner);
                    }),
                }),
            new("淬体", "消耗生命时：灼烧+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;
                        if (!(b.Owner == d.Src && b.Owner == d.Tgt))
                            return;
                        await b.Owner.BuffSelfProcedure("灼烧", b.Stack);
                    }),
                }),
            new("追击", "持续[层数]次，下次攻击时，次数+1", BuffStackRule.Add, true, true),
            new("净天地", "使用非攻击卡不消耗灵气，使用之后消耗", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_STEP, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StepDetails d = (StepDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        if (d.Skill.GetSkillType().Contains(SkillType.Attack))
                            return;

                        await d.Skill.ExhaustProcedure();
                        bool noBuff = b.Owner.GetStackOfBuff("免费") == 0;
                        if (noBuff)
                            await b.Owner.BuffSelfProcedure("免费");

                        await b.SetDStack(-1);
                    }),
                }),

            new("心斋", "所有耗蓝-[层数]", BuffStackRule.Add, true, false),

            new("盛开", "受到治疗时：力量+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_HEAL, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        HealDetails d = (HealDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            await b.Owner.BuffSelfProcedure("力量", b.Stack);
                    }),
                }),

            new("通透世界", "攻击具有穿透", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                            d.Pierce = true;
                    }),
                }),
            new("鹤回翔", "反转出牌顺序", BuffStackRule.One, true, false),

            new("待激活的凤凰涅槃", "累计20灼烧后激活", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_BUFF, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        BuffDetails d = (BuffDetails)stageEventDetails;

                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.GainedBurningRecord < 20) return;

                        await b.Owner.BuffSelfProcedure("涅槃");
                        await b.Owner.RemoveBuff(b);
                    }),
                }),
            new("涅槃", "每轮以及强制结算前：生命恢复至上限", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.END_STAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StageDetails d = (StageDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        await b.Owner.HealProcedure(b.Owner.MaxHp - b.Owner.Hp);
                    }),
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_ROUND, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner == d.Owner)
                            await b.Owner.HealProcedure(b.Owner.MaxHp - b.Owner.Hp);
                    }),
                }),
            new("抱元守一", "每回合：消耗[层数]生命，护甲+[层数]", BuffStackRule.One, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.Owner.DamageSelfProcedure(b.Stack);
                        await b.Owner.ArmorGainSelfProcedure(b.Stack);
                    }),
                }),

            new("灼烧", "受到敌方攻击后：造成[层数]伤害", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.DID_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (!d.Recursive) return;
                        if (d.Src != b.Owner && d.Tgt == b.Owner)
                        {
                            await b.Owner.DamageOppoProcedure(b.Stack, recursive: false);
                        }
                    }),
                }),

            new("自动护甲", "每回合：护甲+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.END_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await d.Owner.ArmorGainSelfProcedure(b.Stack);
                    }),
                }),
            new("少阳", "获得护甲：额外+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.ARMOR_WILL_GAIN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ArmorGainDetails d = (ArmorGainDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            d.Value += b.Stack;
                    }),
                }),
            new("少阴", "施加减甲：额外+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.ARMOR_WILL_LOSE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ArmorLoseDetails d = (ArmorLoseDetails)stageEventDetails;
                        if (b.Owner == d.Src && b.Owner != d.Tgt)
                            d.Value += b.Stack;
                    }),
                }),
            new("永久暴击", "攻击附带暴击", BuffStackRule.One, true, false),
            new("天人合一", "激活所有架势", BuffStackRule.One, true, false),

            new("看破", "无效化敌人下一次攻击，并且反击", BuffStackRule.Add, true, false,
                eventDescriptors: new CLEventDescriptor[]
                {
                    new(CLEventDict.STAGE_ENVIRONMENT, CLEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (!d.Recursive) return;
                        if (d.Src != b.Owner && d.Tgt == b.Owner)
                        {
                            await b.Owner.AttackProcedure(d.Value, d.WuXing, 1, d.LifeSteal, d.Pierce, d.Crit, false, d.Damaged);
                            d.Cancel = true;
                        }
                    }),
                }),
        });
    }

    public void Init()
    {
        List.Do(entry => entry.Generate());
    }

    public override BuffEntry Default() => this["不存在的Buff"];
}
