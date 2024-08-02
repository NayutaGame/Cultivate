
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class BuffCategory : Category<BuffEntry>
{
    public BuffCategory()
    {
        AddRange(new List<BuffEntry>()
        {
            new(id:                         "不存在的Buff",
                description:                "不存在的Buff",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                eventDescriptors:           null),
            
            new(id:                         "灵气",
                description:                "可以消耗灵气使用技能",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                eventDescriptors:           null),

            new("齐物论",     "奇偶同时激活两个效果",                    BuffStackRule.One, true, false),
            new("追击",      "持续[层数]次，下次攻击时，次数+1",            BuffStackRule.Add, true, false),
            new("鹤回翔",     "反转出牌顺序",                        BuffStackRule.One, true, false),
            new("永久暴击",    "攻击附带暴击",                        BuffStackRule.One, true, false),
            new("天人合一",    "激活所有架势",                        BuffStackRule.One, true, false),
            new("跳卡牌",     "行动时跳过下张卡牌",                     BuffStackRule.Add, false, false),
            new("集中",      "下一次使用牌时，条件算作激活",                BuffStackRule.Add, true, false),
            new("永久集中",    "所有牌，条件算作激活",                    BuffStackRule.One, true, false),
            new("浮空艇",     "回合被跳过时：生命及上线无法下降",              BuffStackRule.Add, true, false),
            new("架势",     "消耗架势激活效果，没有架势时获得架势",              BuffStackRule.One, true, false),
            new("一梦如是已触发",     "一梦如是已触发",              BuffStackRule.One, true, false),
            
            new(id:                         "跳走步",
                description:                "跳过走步阶段",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                false,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STEP, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StartStepDetails d = (StartStepDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        d.Cancel = true;
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(id:                         "滞气",
                description:                "每回合：失去[层数]灵气，层数-1",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.Owner.LoseBuffProcedure("灵气", b.Stack);
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(id:                         "缠绕",
                description:                "无法二动/三动\n回合结束/二动时：-1层",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ACTION, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ActionDetails d = (ActionDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        if (!d.IsSwift) return;
                        d.Cancel = true;
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(id:                         "软弱",
                description:                "攻击时：少[层数]攻\n回合结束/攻击时：-1层",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ATTACK, 1, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner != d.Src) return;
                        d.Value -= b.Stack;
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(id:                         "腐朽",
                description:                "每回合：失去[层数]护甲，层数-1",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.Owner.LoseArmorProcedure(b.Stack);
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(id:                         "内伤",
                description:                "每回合：失去[层数]生命，层数-1",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await d.Owner.DamageSelfProcedure(b.Stack);
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(id:                         "无法攻击",
                description:                "无法攻击",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                false,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ATTACK, -3, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner != d.Src) return;
                        b.PlayPingAnimation();
                        d.Cancel = true;
                        await b.SetDStack(d.Value);
                    }),
                }),
            
            new(id:                         "永久二重",
                description:                "所有牌使用两次",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_EXECUTE, -1, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ExecuteDetails d = (ExecuteDetails)stageEventDetails;
                        if (b.Owner != d.Caster) return;
                        b.PlayPingAnimation();
                        d.CastTimes = 2;
                    }),
                }),
            
            new(id:                         "二重",
                description:                "下[层数]张牌使用两次",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_EXECUTE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ExecuteDetails d = (ExecuteDetails)stageEventDetails;
                        if (b.Owner != d.Caster) return;
                        if (d.CastTimes > 1) return;
                        d.CastTimes = 2;
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(id:                         "多重",
                description:                "下一张牌额外使用[层数]次",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_EXECUTE, 1, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ExecuteDetails d = (ExecuteDetails)stageEventDetails;
                        if (b.Owner != d.Caster) return;
                        d.CastTimes += b.Stack;
                        await b.Owner.RemoveBuff(b);
                    }),
                }),
            
            new(id:                         "跳回合",
                description:                "跳过[层数]次回合",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                false,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 100, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        d.Cancel = true;
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(id:                         "禁止治疗",
                description:                "无法受到治疗",
                buffStackRule:              BuffStackRule.One,
                friendly:                   false,
                dispellable:                false,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_HEAL, -3, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        HealDetails d = (HealDetails)stageEventDetails;

                        if (b.Owner != d.Tgt) return;
                        d.Cancel = true;
                    }),
                }),
            
            new(id:                         "心斋",
                description:                "所有耗蓝-[层数]",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_MANA_COST, -3, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ManaCostResult d = (ManaCostResult)stageEventDetails;

                        if (b.Owner != d.Entity) return;
                        b.PlayPingAnimation();
                        d.Value = (d.Value - b.Stack).ClampLower(0);
                    }),
                }),
            
            new(id:                         "永久免费",
                description:                "使用牌时：无需消耗灵气",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_MANA_COST, -2, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ManaCostResult d = (ManaCostResult)stageEventDetails;

                        if (b.Owner != d.Entity) return;
                        b.PlayPingAnimation();
                        d.Value = 0;
                    }),
                }),
            
            new(id:                         "免费",
                description:                "持续[层数]次，使用牌时：无需消耗灵气",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_MANA_COST, -1, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ManaCostResult d = (ManaCostResult)stageEventDetails;

                        if (b.Owner != d.Entity) return;
                        if (d.Value <= 0) return;
                        
                        d.Value = 0;
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                }),
            
            new(id:                         "不堪一击",
                description:                "受击伤：生命降至0",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                eventDescriptors:           new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;
                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.Hp > 0)
                        {
                            b.PlayPingAnimation();
                            await b.Owner.LoseHealthProcedure(b.Owner.Hp);
                        }
                    }),
                }),
            
            new("暴击", "下一次攻击具有暴击", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;

                        if (b.Owner != d.Src || b.Owner == d.Tgt || d.Crit) return;
                        d.Crit = true;
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                }),
            
            new("二动", "下一张牌二动", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        b.Owner.SetActionPoint(2);
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                }),
            
            new("六爻化劫", "第二轮开始时，双方重置生命上限，回[层数]%血", BuffStackRule.Max, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ROUND, 0, async (listener, stageEventDetails) =>
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

                        await self.HealProcedure(selfHpGap, induced: true);
                        await oppo.HealProcedure(oppoHpGap, induced: true);

                        b.PlayPingAnimation();
                        await self.RemoveBuff(b);
                    }),
                }),

            new("灵气回收", "下一次灵气减少时，加回", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_LOSE_BUFF, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        LoseBuffDetails d = (LoseBuffDetails)eventDetails;
                        if (b.Owner != d.Tgt) return;
                        if (d._buffEntry.GetName() != "灵气") return;

                        await d.Tgt.GainBuffProcedure("灵气", d._stack);
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                    }),
                }),

            new("护甲回收", "下一次护甲减少时，加回", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_LOSE_ARMOR, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        LoseArmorDetails d = (LoseArmorDetails)eventDetails;
                        if (b.Owner == d.Tgt)
                        {
                            await b.Owner.GainArmorProcedure(d.Value, induced: true);
                            b.PlayPingAnimation();
                            await b.SetDStack(-1);
                        }
                    }),
                }),

            new("长明灯", "获得灵气时：每1，生命+3", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_GAIN_BUFF, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        GainBuffDetails d = (GainBuffDetails)eventDetails;
                        if (b.Owner != d.Tgt) return;
                        if (d._buffEntry.GetName() != "灵气") return;
                        await b.Owner.HealProcedure(d._stack * 3, induced: true);
                        b.PlayPingAnimation();
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
                        b.PlayPingAnimation();
                        await b.Owner.RemoveArmorProcedure(d.Value);
                    }),
                }),

            new("回合力量", "回合开始时：力量+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)eventDetails;
                        if (b.Owner != d.Owner) return;
                        b.PlayPingAnimation();
                        await b.Owner.GainBuffProcedure("力量", b.Stack);
                    }),
                }),

            new("回合免疫", "此回合无法收到伤害", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)eventDetails;
                        if (b.Owner != d.Owner) return;
                        b.PlayPingAnimation();
                        await b.Owner.RemoveBuff(b);
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_DAMAGE, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)eventDetails;
                        if (b.Owner == d.Tgt)
                        {
                            b.PlayPingAnimation();
                            d.Cancel = true;
                        }
                    }),
                }),

            new("外骨骼", "每次攻击前，护甲+3", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ATTACK, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)eventDetails;
                        if (b.Owner != d.Src) return;
                        b.PlayPingAnimation();
                        await b.Owner.GainArmorProcedure(3 * b.Stack, induced: true);
                    }),
                }),

            new("永动机", "[层数]回合后死亡", BuffStackRule.Min, false, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)eventDetails;
                        if (b.Owner != d.Owner) return;
                        b.PlayPingAnimation();
                        await b.SetDStack(-1);
                        if (b.Owner.GetStackOfBuff("永动机") == 0)
                            await b.Owner.LoseHealthProcedure(b.Owner.Hp);
                    }),
                }),

            new("火箭靴", "使用灵气牌时：获得二动", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_STEP, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        EndStepDetails d = (EndStepDetails)eventDetails;
                        if (b.Owner != d.Owner) return;
                        if (d.Skill != null && d.Skill.GetSkillType().Contains(SkillType.Mana))
                        {
                            b.PlayPingAnimation();
                            b.Owner.SetActionPoint(2);
                        }
                    }),
                }),

            new("定龙桩", "对方二动时：暴击补至1", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_ACTION, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ActionDetails d = (ActionDetails)eventDetails;
                        if (b.Owner == d.Owner) return;
                        if (!d.IsSwift) return;
                        if (b.Owner.GetStackOfBuff("暴击") == 0)
                        {
                            b.PlayPingAnimation();
                            await b.Owner.GainBuffProcedure("暴击");
                        }
                    }),
                }),

            new("飞行器", "成功闪避时，对方跳回合补至1", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_EVADE, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        EvadedDetails d = (EvadedDetails)eventDetails;
                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.GetStackOfBuff("跳回合") == 0)
                        {
                            b.PlayPingAnimation();
                            await b.Owner.GainBuffProcedure("跳回合");
                        }
                    }),
                }),

            new("延迟攻", "下回合，[层数]攻", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        b.PlayPingAnimation();
                        await b.Owner.AttackProcedure(b.Stack);
                        await b.Owner.RemoveBuff(b);
                    }),
                }),

            new("延迟护甲", "下回合，护甲+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        b.PlayPingAnimation();
                        await b.Owner.GainArmorProcedure(b.Stack, induced: false);
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
                        b.PlayPingAnimation();
                        await b.Owner.RemoveArmorProcedure(Mathf.Min(d.Value, b.Stack));
                    }),
                }),

            new("锋锐", "回合结束时：[层数]间接攻击\n受到伤害后层数-1", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        b.PlayPingAnimation();
                        await b.Owner.IndirectProcedure(b.Stack, wuXing: WuXing.Jin);
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                        {
                            b.PlayPingAnimation();
                            await b.SetDStack(-1);
                        }
                    }),
                }),

            new("抱朴", "每回合：灵气+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        b.PlayPingAnimation();
                        await b.Owner.GainBuffProcedure("灵气", b.Stack);
                    }),
                }),
            
            new("敛息", "击伤时：伤害替换为减甲", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;

                        if (b.Owner == d.Src && d.Src != d.Tgt)
                        {
                            d.Cancel = true;
                            b.PlayPingAnimation();
                            await b.Owner.RemoveArmorProcedure(d.Value);
                            await b.SetDStack(-1);
                        }
                    }),
                }),

            new("吸血", "下一次攻击造成伤害时，回复生命", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src)
                        {
                            b.PlayPingAnimation();
                            d.LifeSteal = true;
                            await b.SetDStack(-1);
                        }
                    }),
                }),
            
            new("幻月狂乱", "攻击一直具有吸血，使用非攻击牌时：遭受1跳回合", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_STEP, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        EndStepDetails d = (EndStepDetails)stageEventDetails;
                        if (b.Owner == d.Owner)
                        {
                            if (!d.Skill.GetSkillType().Contains(SkillType.Attack))
                            {
                                b.PlayPingAnimation();
                                await d.Owner.GainBuffProcedure("跳回合");
                            }
                        }
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ATTACK, -1, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src)
                        {
                            d.LifeSteal = true;
                            b.PlayPingAnimation();
                        }
                    }),
                }),
            
            new("吐纳", "治疗可以穿上限", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_HEAL, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        HealDetails d = (HealDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                        {
                            b.PlayPingAnimation();
                            d.Penetrate = true;
                        }
                    }),
                }),
            
            new("格挡", "受到攻击：攻击力-[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ATTACK, 1, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (d.Penetrate) return;
                        if (b.Owner == d.Tgt && d.Src != d.Tgt)
                        {
                            b.PlayPingAnimation();
                            d.Value -= b.Stack;
                        }
                    }),
                }),

            new("闪避", "受到攻击时，减少1层，忽略此次攻击", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Tgt && d.Src != d.Tgt)
                        {
                            d.Evade = true;
                            b.PlayPingAnimation();
                            await b.SetDStack(-1);
                        }
                    }),
                }),
            
            new("轮闪避", "每轮：闪避补至[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ROUND, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner == d.Owner)
                        {
                            b.PlayPingAnimation();
                            await b.Owner.GainBuffProcedure("闪避", b.Stack - b.Owner.GetStackOfBuff("闪避"));
                        }
                    }),
                }),
            
            new("穿透", "下一次攻击时，忽略对方护甲/闪避/格挡", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ATTACK, -1, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt && !d.Penetrate)
                        {
                            d.Penetrate = true;
                            b.PlayPingAnimation();
                            await b.SetDStack(-1);
                        }
                    }),
                }),
            
            new("力量", "攻击时，多[层数]攻", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ATTACK, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                        {
                            b.PlayPingAnimation();
                            d.Value += b.Stack;
                        }
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
                            b.PlayPingAnimation();
                            await b.Owner.AttackProcedure(b.Stack, wuXing: WuXing.Mu, recursive: false, induced: true);
                            await b.Owner.RemoveBuff(b);
                        }
                    }),
                }),

            new("天衣无缝", "每回合：[层数]攻", BuffStackRule.Max, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        b.PlayPingAnimation();
                        await d.Owner.AttackProcedure(b.Stack, wuXing: WuXing.Huo, fromSeamless: true);
                    }),
                }),
            
            new("业火", "卡牌变成疲劳时：使用2次", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_EXHAUST, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ExhaustDetails d = (ExhaustDetails)stageEventDetails;
                        if (b.Owner == d.Owner)
                        {
                            b.PlayPingAnimation();
                            await d.Owner.CastProcedure(d.Skill);
                        }
                    }),
                }),
            
            new("淬体", "燃命时：灼烧+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;
                        if (b.Owner != d.Tgt)
                            return;

                        if (d.Src != d.Tgt)
                            return;
                        
                        b.PlayPingAnimation();
                        await b.Owner.GainBuffProcedure("灼烧", b.Stack);
                    }),
                }),
            
            new("观众生", "使用非攻击卡时，变得疲劳", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STEP, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StartStepDetails d = (StartStepDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        StageSkill skill = d.Owner._skills[d.P];
                        if (skill.GetSkillType().Contains(SkillType.Attack))
                            return;
                        
                        b.PlayPingAnimation();
                        await skill.ExhaustProcedure();
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
                        {
                            b.PlayPingAnimation();
                            await b.Owner.GainBuffProcedure("力量", b.Stack);
                        }
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
                            b.PlayPingAnimation();
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
                            b.PlayPingAnimation();
                            await b.Owner.IndirectProcedure(b.Stack, recursive: false);
                        }
                    }),
                }),

            new("柔韧", "对方回合开始时：护甲+[层数]，己方回合开始时：层数-1", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_TURN, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        b.PlayPingAnimation();
                        if (b.Owner == d.Owner)
                        {
                            await b.SetDStack(-1);
                        }
                        else
                        {
                            await b.Owner.GainArmorProcedure(b.Stack, induced: true);
                        }
                    }),
                }),
            
            new("两仪", "获得护甲时/施加减甲时：额外+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_GAIN_ARMOR, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        GainArmorDetails d = (GainArmorDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                        {
                            b.PlayPingAnimation();
                            d.Value += b.Stack;
                        }
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_LOSE_ARMOR, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        LoseArmorDetails d = (LoseArmorDetails)stageEventDetails;
                        if (b.Owner == d.Src && b.Owner != d.Tgt)
                        {
                            b.PlayPingAnimation();
                            d.Value += b.Stack;
                        }
                    }),
                }),

            new("看破", "无效化敌人下一次攻击，并且反击", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ATTACK, -2, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (!d.Recursive) return;
                        if (d.Src != b.Owner && d.Tgt == b.Owner)
                        {
                            b.PlayPingAnimation();
                            await b.Owner.AttackProcedure(d.Value, d.WuXing, 1, d.LifeSteal, d.Penetrate, d.Crit, false, d.DidDamage);
                            d.Cancel = true;
                        }
                    }),
                }),

            new("待激活的人间无戈", "20锋锐觉醒：死亡不会导致战斗结算", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_GAIN_BUFF, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        GainBuffDetails d = (GainBuffDetails)stageEventDetails;

                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.GainedFengRuiRecord < 20) return;
                        if (b.Owner.GetStackOfBuff("人间无戈") != 0) return;

                        b.PlayPingAnimation();
                        await b.Owner.GainBuffProcedure("人间无戈");
                        await b.Owner.RemoveBuff(b);
                    }),
                }),
            
            new("人间无戈", "死亡不会停止战斗", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_COMMIT, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        CommitDetails d = (CommitDetails)stageEventDetails;

                        d.Cancel = true;
                    }),
                }),

            new("待激活的摩诃钵特摩", "20格挡觉醒：八动，如果受伤则死亡", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_GAIN_BUFF, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        GainBuffDetails d = (GainBuffDetails)stageEventDetails;

                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.GainedGeDangRecord < 20) return;
                        if (b.Owner.GetStackOfBuff("摩诃钵特摩") != 0) return;

                        b.PlayPingAnimation();
                        await b.Owner.GainBuffProcedure("摩诃钵特摩");
                        await b.Owner.RemoveBuff(b);
                    }),
                }),
            
            new("摩诃钵特摩", "八动，如果受伤则死亡", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    // new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.BUFF_APPEAR, 0, async (listener, stageEventDetails) =>
                    // {
                    //     Buff b = (Buff)listener;
                    //     BuffAppearDetails d = (BuffAppearDetails)stageEventDetails;
                    //
                    //     if (b.Owner != d.Owner) return;
                    //     b.PlayPingAnimation();
                    //     d.Owner.SetActionPoint(d.Owner.GetActionPoint() + 8);
                    // }),
                    // new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_TURN, 0, async (listener, stageEventDetails) =>
                    // {
                    //     Buff b = (Buff)listener;
                    //     TurnDetails d = (TurnDetails)stageEventDetails;
                    //
                    //     if (b.Owner != d.Owner) return;
                    //     b.PlayPingAnimation();
                    //     await b.Owner.LoseHealthProcedure(b.Owner.Hp);
                    // }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_DAMAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;
                        if (b.Owner != d.Tgt) return;
                        if (d.Value == 0) return;
                        if (b.Owner.Hp > 0)
                        {
                            b.PlayPingAnimation();
                            await b.Owner.LoseHealthProcedure(b.Owner.Hp);
                        }
                    }),
                }),

            new("待激活的通透世界", "20力量觉醒：永久穿透和集中", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_GAIN_BUFF, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        GainBuffDetails d = (GainBuffDetails)stageEventDetails;

                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.GainedLiLiangRecord < 20) return;
                        if (b.Owner.GetStackOfBuff("通透世界") != 0) return;

                        b.PlayPingAnimation();
                        await b.Owner.GainBuffProcedure("通透世界");
                        await b.Owner.RemoveBuff(b);
                    }),
                }),

            new("通透世界", "永久穿透和集中", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ATTACK, -1, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                        {
                            b.PlayPingAnimation();
                            d.Penetrate = true;
                        }
                    }),
                }),
            
            new("待激活的凤凰涅槃", "20灼烧觉醒：每轮生命恢复至上限", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_GAIN_BUFF, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        GainBuffDetails d = (GainBuffDetails)stageEventDetails;

                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.GainedZhuoShaoRecord < 20) return;
                        if (b.Owner.GetStackOfBuff("凤凰涅槃") != 0) return;

                        b.PlayPingAnimation();
                        await b.Owner.GainBuffProcedure("凤凰涅槃");
                        await b.Owner.RemoveBuff(b);
                    }),
                }),
            
            new("凤凰涅槃", "每轮以及强制结算前：生命恢复至上限", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_STAGE, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StageDetails d = (StageDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        b.PlayPingAnimation();
                        await b.Owner.HealProcedure(b.Owner.MaxHp - b.Owner.Hp, induced: false);
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ROUND, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner == d.Owner)
                        {
                            b.PlayPingAnimation();
                            await b.Owner.HealProcedure(b.Owner.MaxHp - b.Owner.Hp, induced: false);
                        }
                    }),
                }),

            new("待激活的那由他", "20柔韧觉醒：灵气消耗为零，Step阶段无法受影响，所有Buff层数不会再变化", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_GAIN_BUFF, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        GainBuffDetails d = (GainBuffDetails)stageEventDetails;

                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.GainedRouRenRecord < 20) return;
                        if (b.Owner.GetStackOfBuff("那由他") != 0) return;

                        b.PlayPingAnimation();
                        await b.Owner.GainBuffProcedure("那由他");
                        await b.Owner.RemoveBuff(b);
                    }),
                }),
            
            new("那由他", "灵气/吟唱消耗为零，Step阶段无法受影响，所有Buff层数不会再变化", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_MANA_COST, -4, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ManaCostResult d = (ManaCostResult)stageEventDetails;

                        b.PlayPingAnimation();
                        d.Value = 0;
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_GAIN_BUFF, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        GainBuffDetails d = (GainBuffDetails)stageEventDetails;

                        b.PlayPingAnimation();
                        d._stack = 0;
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_LOSE_BUFF, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        LoseBuffDetails d = (LoseBuffDetails)stageEventDetails;

                        b.PlayPingAnimation();
                        d._stack = 0;
                    }),
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_TURN, 101, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        b.PlayPingAnimation();
                        d.Cancel = false;
                    }),
                }),

            new("钟声", "使用一张牌前：升级", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_CAST, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        CastDetails d = (CastDetails)eventDetails;
                        if (b.Owner != d.Caster || d.Caster != d.Skill.Owner) return;

                        if (await d.Skill.TryUpgradeJingJie())
                        {
                            b.PlayPingAnimation();
                            await b.SetDStack(-1);
                        }
                    }),
                }),

            new("轮生命上限", "每轮：生命上限+[层数]", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ROUND, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)eventDetails;
                        if (b.Owner != d.Owner) return;

                        b.PlayPingAnimation();
                        b.Owner.MaxHp += b.Stack;
                    }),
                }),

            new("轮暴击", "每轮：获得[层数]暴击", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ROUND, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)eventDetails;
                        if (b.Owner != d.Owner) return;

                        b.PlayPingAnimation();
                        await b.Owner.GainBuffProcedure("暴击", b.Stack);
                    }),
                }),

            new("轮吸血", "每轮：获得[层数]吸血", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ROUND, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)eventDetails;
                        if (b.Owner != d.Owner) return;

                        b.PlayPingAnimation();
                        await b.Owner.GainBuffProcedure("吸血", b.Stack);
                    }),
                }),

            new("轮穿透", "每轮：获得[层数]穿透", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ROUND, 0, async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)eventDetails;
                        if (b.Owner != d.Owner) return;

                        b.PlayPingAnimation();
                        await b.Owner.GainBuffProcedure("穿透", b.Stack);
                    }),
                }),
            
            new("疲劳", "使用下一张牌后变成疲劳", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_STEP, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StartStepDetails d = (StartStepDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        StageSkill skill = d.Owner._skills[d.P];
                        
                        b.PlayPingAnimation();
                        await skill.ExhaustProcedure();
                        
                        await b.SetDStack(-1);
                    }),
                }),
            
            new("清心", "获得灵气时：每1，回复[层数]生命", BuffStackRule.Add, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.DID_GAIN_BUFF, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        GainBuffDetails d = (GainBuffDetails)stageEventDetails;

                        if (b.Owner != d.Tgt) return;
                        if (d._buffEntry.GetName() != "灵气") return;
                        
                        b.PlayPingAnimation();
                        await b.Owner.HealProcedure(d._stack * b.Stack, induced: true);
                    }),
                }),
            
            new("太虚", "第二轮：生命回满", BuffStackRule.One, true, false,
                eventDescriptors: new StageEventDescriptor[]
                {
                    new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ROUND, 0, async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        
                        b.PlayPingAnimation();
                        int gap = b.Owner.MaxHp - b.Owner.Hp;
                        await b.Owner.HealProcedure(gap, induced: false);
                        await b.SetDStack(-1);
                    }),
                }),
        });
    }

    public void Init()
    {
        List.Do(entry => entry.GenerateAnnotations());
    }

    public override BuffEntry DefaultEntry() => this["不存在的Buff"];
}
