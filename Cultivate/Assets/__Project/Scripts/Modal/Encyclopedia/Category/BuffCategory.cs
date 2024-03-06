
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class BuffCategory : Category<BuffEntry>
{
    public BuffCategory()
    {
        AddRange(new List<BuffEntry>()
        {
            new(name:                       "不存在的Buff",
                description:                "不存在的Buff",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                eventDescriptors:           null),
            
            new(name:                       "灵气",
                description:                "可以消耗灵气使用技能",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                eventDescriptors:           null),

            new("齐物论",     "奇偶同时激活两个效果",                    BuffStackRule.One, true, false),
            new("追击",      "持续[层数]次，下次攻击时，次数+1",            BuffStackRule.Add, true, true),
            new("心斋",      "所有耗蓝-[层数]",                     BuffStackRule.Add, true, false),
            new("鹤回翔",     "反转出牌顺序",                        BuffStackRule.One, true, false),
            new("永久暴击",    "攻击附带暴击",                        BuffStackRule.One, true, false),
            new("天人合一",    "激活所有架势",                        BuffStackRule.One, true, false),
            new("跳回合",     "跳过回合",                          BuffStackRule.Add, false, false),
            new("跳卡牌",     "行动时跳过下张卡牌",                     BuffStackRule.Add, false, false),
            new("双发",      "下一张牌使用两次",                      BuffStackRule.Add, true, false),
            new("永久双发",    "所有牌使用两次",                       BuffStackRule.One, true, false),
            new("免费",      "下一次耗蓝时无需灵气",                    BuffStackRule.Add, true, false),
            new("永久免费",    "所有牌无需灵气",                       BuffStackRule.One, true, false),
            new("集中",      "下一次使用牌时，条件算作激活",                BuffStackRule.Add, true, false),
            new("永久集中",    "所有牌，条件算作激活",                    BuffStackRule.Add, true, false),
            new("浮空艇",     "回合被跳过时：生命及上线无法下降",              BuffStackRule.Add, true, false),
            
            new(name:                       "滞气",
                description:                "每回合：失去[层数]灵气，层数-1",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await d.Owner.RemoveBuffProcedure("灵气", b.Stack);
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(name:                       "缠绕",
                description:                "无法二动/三动\n回合结束/二动时：-1层",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_SWIFT, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        SwiftDetails d = (SwiftDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        d.Cancel = true;
                        await b.SetDStack(-1);
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.END_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(name:                       "软弱",
                description:                "攻击时：少[层数]攻\n回合结束/攻击时：-1层",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner != d.Src) return;
                        d.Value -= b.Stack;
                        await b.SetDStack(-1);
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.END_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(name:                       "内伤",
                description:                "每回合：失去[层数]生命，层数-1",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await d.Owner.DamageSelfProcedure(b.Stack);
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(name:                       "腐朽",
                description:                "每回合：失去[层数]护甲，层数-1",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.END_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.Owner.LoseArmorProcedure(b.Stack);
                        await b.SetDStack(-1);
                    }),
                }),
            
            new("二动", "下一张牌二动", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        d.Owner.Swift = true;
                        await b.SetDStack(-1);
                    }),
                }),
            
            new("六爻化劫", "第二轮开始时，双方重置生命上限，回[层数]%血", BuffStackRule.Max, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_ROUND, 0, async (listener, stageEventDetails) =>
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
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.BUFF_DID_LOSE, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        LoseBuffDetails d = (LoseBuffDetails)eventDetails;
                        if (b.Owner != d.Tgt) return;
                        if (d._buffEntry.GetName() != "灵气") return;

                        await d.Tgt.GainBuffProcedure("灵气", d._stack);
                        await b.SetDStack(-1);
                    }),
                }),

            new("护甲回收", "下一次护甲减少时，加回", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.ARMOR_DID_LOSE, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        LoseArmorDetails d = (LoseArmorDetails)eventDetails;
                        if (b.Owner == d.Tgt)
                        {
                            await b.Owner.GainArmorProcedure(d.Value);
                            await b.SetDStack(-1);
                        }
                    }),
                }),

            new("长明灯", "获得灵气时：每1，生命+3", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.BUFF_DID_GAIN, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        GainBuffDetails d = (GainBuffDetails)eventDetails;
                        if (b.Owner != d.Tgt) return;
                        if (d._buffEntry.GetName() != "灵气") return;
                        await b.Owner.HealProcedure(d._stack * 3);
                    }),
                }),

            new("尖刺陷阱", "下次受到攻击时，对对方施加等量减甲", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_ATTACK, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)eventDetails;
                        if (b.Owner != d.Tgt || d.Src == d.Tgt) return;
                        await b.Owner.RemoveArmorProcedure(d.Value);
                    }),
                }),

            new("回合力量", "回合开始时：力量+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_TURN, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)eventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.Owner.GainBuffProcedure("力量", b.Stack);
                    }),
                }),

            new("回合免疫", "此回合无法收到伤害", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_TURN, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)eventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.Owner.RemoveBuff(b);
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_DAMAGE, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)eventDetails;
                        if (b.Owner == d.Tgt)
                            d.Cancel = true;
                    }),
                }),

            new("外骨骼", "每次攻击前，护甲+3", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_ATTACK, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)eventDetails;
                        if (b.Owner != d.Src) return;
                        await b.Owner.GainArmorProcedure(3 * b.Stack);
                    }),
                }),

            new("永动机", "[层数]回合后死亡", BuffStackRule.Min, false, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_TURN, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)eventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.SetDStack(-1);
                        if (b.Owner.GetStackOfBuff("永动机") == 0)
                            await b.Owner.LoseHealthProcedure(b.Owner.Hp);
                    }),
                }),

            new("火箭靴", "使用灵气牌时：获得二动", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.END_STEP, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StepDetails d = (StepDetails)eventDetails;
                        if (b.Owner != d.Owner) return;
                        if (d.Skill != null && d.Skill.GetSkillType().Contains(SkillType.LingQi))
                            b.Owner.Swift = true;
                    }),
                }),

            new("定龙桩", "对方二动时：暴击补至1", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_SWIFT, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        SwiftDetails d = (SwiftDetails)eventDetails;
                        if (b.Owner == d.Owner) return;
                        if (b.Owner.GetStackOfBuff("暴击") == 0)
                            await b.Owner.GainBuffProcedure("暴击");
                    }),
                }),

            new("飞行器", "成功闪避时，对方跳回合补至1", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_EVADE, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        EvadeDetails d = (EvadeDetails)eventDetails;
                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.GetStackOfBuff("跳回合") == 0)
                            await b.Owner.GainBuffProcedure("跳回合");
                    }),
                }),

            new("时光机", "使用一张牌前，升级", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_EXECUTE, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ExecuteDetails d = (ExecuteDetails)eventDetails;
                        if (b.Owner != d.Caster || d.Caster != d.Skill.Owner) return;
                        await d.Skill.TryUpgradeJingJie();
                    }),
                }),

            new("延迟护甲", "下回合护甲+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        await b.Owner.GainArmorProcedure(b.Stack);
                        await b.Owner.RemoveBuff(b);
                    }),
                }),

            new("诸行无常", "造成伤害：施加[伤害值，最多Stack]减甲", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;
                        if (!(b.Owner == d.Src && d.Src != d.Tgt))
                            return;
                        await b.Owner.RemoveArmorProcedure(Mathf.Min(d.Value, b.Stack));
                    }),
                }),

            new("锋锐", "回合结束时：[层数]间接攻击\n受到伤害后层数-1", BuffStackRule.Add, true, true,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.END_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        await b.Owner.IndirectProcedure(b.Stack, wuXing: WuXing.Jin);
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            await b.SetDStack(-1);
                    }),
                }),

            new("一切皆苦", "每回合：灵气+[层数]", BuffStackRule.Add, true, true,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        await b.Owner.GainBuffProcedure("灵气", b.Stack);
                    }),
                }),
            
            new("敛息", "击伤时：伤害替换为减甲", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;

                        if (b.Owner == d.Src && d.Src != d.Tgt)
                        {
                            d.Cancel = true;
                            await b.Owner.RemoveArmorProcedure(d.Value);
                            await b.SetDStack(-1);
                        }
                    }),
                }),

            new("吸血", "下一次攻击造成伤害时，回复生命", BuffStackRule.Add, true, true,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
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
            
            new("幻月狂乱", "攻击一直具有吸血，使用非攻击牌时：遭受1跳回合", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STEP, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StepDetails d = (StepDetails)stageEventDetails;
                        if (b.Owner == d.Owner)
                        {
                            if (!d.Skill.GetSkillType().Contains(SkillType.Attack))
                                await d.Owner.GainBuffProcedure("跳回合");
                        }
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src)
                        {
                            d.LifeSteal = true;
                        }
                    }),
                }),
            
            new("玄武吐息法", "治疗可以穿上限", BuffStackRule.Add, true, true,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_HEAL, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        HealDetails d = (HealDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            d.Penetrate = true;
                    }),
                }),
            
            new("格挡", "受到攻击：攻击力-[层数]", BuffStackRule.Add, true, true,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
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
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_ROUND, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner == d.Owner)
                            await b.Owner.GainBuffProcedure("格挡", b.Stack);
                    }),
                }),

            new("闪避", "受到攻击时，减少1层，忽略此次攻击", BuffStackRule.Add, true, true,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
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
            
            new("飞龙在天", "每轮：闪避补至[层数]", BuffStackRule.Add, true, true,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_ROUND, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner == d.Owner)
                            await b.Owner.GainBuffProcedure("闪避", b.Stack - b.Owner.GetStackOfBuff("闪避"));
                    }),
                }),
            
            new("穿透", "下一次攻击时，忽略对方护甲/闪避/格挡", BuffStackRule.Add, true, true,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_ATTACK, -1, async (listener, stageEventDetails) =>
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
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                            d.Pierce = true;
                    }),
                }),
            
            new("力量", "攻击时，多[层数]攻", BuffStackRule.Add, true, true,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                            d.Value += b.Stack;
                    }),
                }),
            
            new("回马枪", "下次受攻击后：[层数]攻 穿透", BuffStackRule.Max, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_ATTACK, 0, async (listener, stageEventDetails) =>
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
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await d.Owner.AttackProcedure(b.Stack, wuXing: WuXing.Huo);
                    }),
                }),
            
            new("业火", "消耗牌时：使用2次", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_EXHAUST, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ExhaustDetails d = (ExhaustDetails)stageEventDetails;
                        if (b.Owner == d.Owner)
                            await d.Skill.Execute(d.Owner);
                    }),
                }),
            
            new("淬体", "受伤时：灼烧+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            await b.Owner.GainBuffProcedure("灼烧", b.Stack);
                    }),
                }),
            
            new("净天地", "使用非攻击卡不消耗灵气，使用之后消耗", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_STEP, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StepDetails d = (StepDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        if (d.Skill.GetSkillType().Contains(SkillType.Attack))
                            return;

                        await d.Skill.ExhaustProcedure();
                        bool noBuff = b.Owner.GetStackOfBuff("免费") == 0;
                        if (noBuff)
                            await b.Owner.GainBuffProcedure("免费");

                        await b.SetDStack(-1);
                    }),
                }),

            new("盛开", "受到治疗时：力量+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_HEAL, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        HealDetails d = (HealDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            await b.Owner.GainBuffProcedure("力量", b.Stack);
                    }),
                }),

            new("通透世界", "攻击具有穿透", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                            d.Pierce = true;
                    }),
                }),

            new("待激活的凤凰涅槃", "20灼烧激活", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.BUFF_DID_GAIN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        GainBuffDetails d = (GainBuffDetails)stageEventDetails;

                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.GainedBurningRecord < 20) return;

                        await b.Owner.GainBuffProcedure("涅槃");
                        await b.Owner.RemoveBuff(b);
                    }),
                }),
            
            new("涅槃", "每轮以及强制结算前：生命恢复至上限", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.END_STAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StageDetails d = (StageDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        await b.Owner.HealProcedure(b.Owner.MaxHp - b.Owner.Hp);
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.START_ROUND, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner == d.Owner)
                            await b.Owner.HealProcedure(b.Owner.MaxHp - b.Owner.Hp);
                    }),
                }),

            new("灼烧", "受到敌方攻击后：[层数]间接攻击", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (!d.Recursive) return;
                        if (d.Src != b.Owner && d.Tgt == b.Owner)
                        {
                            await b.Owner.IndirectProcedure(b.Stack, recursive: false);
                        }
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_INDIRECT, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        IndirectDetails d = (IndirectDetails)stageEventDetails;
                        if (!d.Recursive) return;
                        if (d.Src != b.Owner && d.Tgt == b.Owner)
                        {
                            await b.Owner.IndirectProcedure(b.Stack, recursive: false);
                        }
                    }),
                }),
            
            new("两仪", "获得护甲时/施加减甲时：额外+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.ARMOR_WILL_GAIN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        GainArmorDetails d = (GainArmorDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            d.Value += b.Stack;
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.ARMOR_WILL_LOSE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        LoseArmorDetails d = (LoseArmorDetails)stageEventDetails;
                        if (b.Owner == d.Src && b.Owner != d.Tgt)
                            d.Value += b.Stack;
                    }),
                }),

            new("看破", "无效化敌人下一次攻击，并且反击", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WILL_ATTACK, 0, async (listener, stageEventDetails) =>
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
            
            new(name:                       "枯木",
                description:                "回合结束时：受到[层数]减甲",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                false,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.END_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner == d.Owner)
                            await b.Owner.LoseArmorProcedure(b.Stack);
                    }),
                }),
        });
    }

    public void Init()
    {
        List.Do(entry => entry.Generate());
    }

    public override BuffEntry DefaultEntry() => this["不存在的Buff"];
}
