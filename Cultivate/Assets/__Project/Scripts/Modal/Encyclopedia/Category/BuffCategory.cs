
using System.Collections.Generic;
using System.Linq;
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
                closures:                   null),
            
            new(id:                         "灵气",
                description:                "可以消耗灵气使用技能",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   null),
            
            new(id:                         "跳走步",
                description:                "跳过走步阶段",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_STEP, 0, async (listener, closure, closureDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StartStepDetails d = (StartStepDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        d.Cancel = true;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new(id:                         "滞气",
                description:                "每回合：失去[层数]灵气，层数-1",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        await b.Owner.LoseBuffProcedure("灵气", b.Stack);
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new(id:                         "缠绕",
                description:                "无法二动/三动\n回合结束/二动时：-1层",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_ACTION, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        ActionDetails d = (ActionDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        if (!d.IsSwift) return;
                        d.Cancel = true;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                    new(StageClosureDict.DID_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new(id:                         "软弱",
                description:                "攻击时：少[层数]攻\n回合结束/攻击时：-1层",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, 1, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Src) return;
                        d.Value -= b.Stack;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                    new(StageClosureDict.DID_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new(id:                         "腐朽",
                description:                "每回合：失去[层数]护甲，层数-1",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        await b.Owner.LoseArmorProcedure(b.Stack, induced: false);
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new(id:                         "内伤",
                description:                "每回合：失去[层数]气血，层数-1",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        await d.Owner.DamageSelfProcedure(b.Stack);
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new(id:                         "脆弱",
                description:                "受攻击时：多[层数]攻\n回合结束/受攻击时：-1层",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, 1, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        bool cond = b.Owner == d.Tgt && b.Owner.Opponent() == d.Src;
                        if (!cond) return;
                        d.Value += b.Stack;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                    new(StageClosureDict.DID_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new(id:                         "无法攻击",
                description:                "无法攻击",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, -3, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Src) return;
                        b.Emphasize();
                        d.Cancel = true;
                        await b.GainStackProcedure(d.Value);
                    }),
                }),
            
            new(id:                         "永久二重",
                description:                "所有牌使用两次",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_EXECUTE, -1, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        ExecuteDetails d = (ExecuteDetails)closureDetails;
                        if (b.Owner != d.Caster) return;
                        b.Emphasize();
                        d.CastTimes = Mathf.Max(2, d.CastTimes);
                    }),
                }),
            
            new(id:                         "二重",
                description:                "下[层数]张牌使用两次",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_EXECUTE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        ExecuteDetails d = (ExecuteDetails)closureDetails;
                        if (b.Owner != d.Caster) return;
                        if (d.CastTimes > 1) return;
                        d.CastTimes = 2;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new(id:                         "多重",
                description:                "下一张牌额外使用[层数]次，最高20层",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_EXECUTE, 1, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        ExecuteDetails d = (ExecuteDetails)closureDetails;
                        if (b.Owner != d.Caster) return;
                        d.CastTimes += b.Stack;
                        await b.Owner.LoseBuffProcedure(b.GetEntry(), b.Stack);
                    }),
                }),
            
            new(id:                         "跳行动",
                description:                "跳过[层数]次行动",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_TURN, 100, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        d.Cancel = true;
                        b.Emphasize();
                        // await b.Owner.LoseBuffProcedure(b.GetEntry(), 1);
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new(id:                         "禁止治疗",
                description:                "无法受到治疗",
                buffStackRule:              BuffStackRule.One,
                friendly:                   false,
                dispellable:                false,
                isForbiddenDebuff:          true,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_HEAL, -3, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        HealDetails d = (HealDetails)closureDetails;

                        if (b.Owner != d.Tgt) return;
                        d.Cancel = true;
                    }),
                }),
            
            new(id:                         "禁止护甲",
                description:                "无法获得护甲",
                buffStackRule:              BuffStackRule.One,
                friendly:                   false,
                dispellable:                false,
                isForbiddenDebuff:          true,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_GAIN_ARMOR, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        GainArmorDetails d = (GainArmorDetails)closureDetails;

                        if (b.Owner != d.Tgt) return;
                        d.Cancel = true;
                        b.Emphasize();
                    }),
                }),
            
            new(id:                         "禁止二动",
                description:                "无法二动",
                buffStackRule:              BuffStackRule.One,
                friendly:                   false,
                dispellable:                false,
                isForbiddenDebuff:          true,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_ACTION, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        ActionDetails d = (ActionDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        if (!d.IsSwift) return;
                        d.Cancel = true;
                        b.Emphasize();
                    }),
                }),
            
            new(id:                         "禁止行动",
                description:                "无法行动",
                buffStackRule:              BuffStackRule.One,
                friendly:                   false,
                dispellable:                false,
                isForbiddenDebuff:          true,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_ACTION, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        ActionDetails d = (ActionDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        d.Cancel = true;
                        b.Emphasize();
                    }),
                }),
            
            new(id:                         "禁止灵气",
                description:                "无法获得灵气",
                buffStackRule:              BuffStackRule.One,
                friendly:                   false,
                dispellable:                false,
                isForbiddenDebuff:          true,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_GAIN_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        GainBuffDetails d = (GainBuffDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        if (d.BuffEntry.GetName() != "灵气") return;
                        d.Cancel = true;
                        b.Emphasize();
                    }),
                }),
            
            new(id:                         "心斋",
                description:                "所有耗蓝-[层数]",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_MANA_COST, -3, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CostDetails d = closureDetails as CostDetails;

                        if (b.Owner != d.Entity) return;
                        b.Emphasize();
                        d.Value = (d.Value - b.Stack).ClampLower(0);
                    }),
                }),
            
            new(id:                         "永久免费",
                description:                "使用牌时：无需消耗灵气",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_MANA_COST, -2, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CostDetails d = closureDetails as CostDetails;

                        if (b.Owner != d.Entity) return;
                        b.Emphasize();
                        d.Value = 0;
                    }),
                }),
            
            new(id:                         "免费",
                description:                "持续[层数]次，使用牌时：无需消耗灵气",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_MANA_COST, -1, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CostDetails d = closureDetails as CostDetails;

                        if (b.Owner != d.Entity) return;
                        if (d.Value <= 0) return;
                        
                        d.Value = 0;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new(id:                         "不堪一击",
                description:                "受击伤：气血降至0",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                true,
                isForbiddenDebuff:          true,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_DAMAGE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        DamageDetails d = (DamageDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.Hp > 0)
                        {
                            b.Emphasize();
                            await b.Owner.LoseHealthProcedure(b.Owner.Hp, d.CausedByAttack);
                        }
                    }),
                }),
            
            new("暴击", "下一次攻击造成的伤害翻倍", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Src || b.Owner == d.Tgt || d.Crit) return;
                        
                        d.Crit = true;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new("高速吟唱", "吟唱时，额外推进[层数]进度", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_CHANNEL, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        ChannelDetails d = (ChannelDetails)closureDetails;
                        if (b.Owner != d.Caster) return;

                        d.ProgressGain += b.Stack;
                        b.Emphasize();
                    }),
                }),
            
            new("碎防", "下一次攻击时，1点伤害抵消2点护甲", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Src || b.Owner == d.Tgt || d.Shatter) return;
                        
                        d.Shatter = true;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new("二动", "下一回合二动", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;

                        if (b.Owner != d.Owner) return;
                        
                        b.Owner.SetActionPoint(2);
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new("六爻化劫", "第二轮开始时，双方重置气血上限，回[层数]%血", BuffStackRule.Max, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ROUND, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        RoundDetails d = (RoundDetails)closureDetails;

                        if (b.Owner != d.Owner) return;

                        StageEntity self = b.Owner;
                        StageEntity oppo = b.Owner.Opponent();

                        int selfMaxHp = Mathf.Max(self.MaxHp, self.RunEntity.GetHealth());
                        int oppoMaxHp = Mathf.Max(oppo.MaxHp, oppo.RunEntity.GetHealth());
                        self.MaxHp = self.RunEntity.GetHealth();
                        oppo.MaxHp = oppo.RunEntity.GetHealth();

                        int selfHpGap = self.MaxHp - (int)((float)selfMaxHp * b.Stack / 100);
                        int oppoHpGap = oppo.MaxHp - (int)((float)oppoMaxHp * b.Stack / 100);

                        await self.HealProcedure(selfHpGap, induced: true);
                        await oppo.HealProcedure(oppoHpGap, induced: true);

                        b.Emphasize();
                        await b.Owner.LoseBuffProcedure(b.GetEntry(), b.Stack);
                    }),
                }),

            new("灵气回收", "下一次灵气减少时，加回", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_LOSE_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        LoseBuffDetails d = (LoseBuffDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        if (d.BuffEntry.GetName() != "灵气") return;

                        await d.Tgt.GainBuffProcedure("灵气", d.Stack);
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),

            new("护甲回收", "下一次护甲减少时，加回", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_LOSE_ARMOR, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        LoseArmorDetails d = (LoseArmorDetails)closureDetails;
                        if (b.Owner == d.Tgt)
                        {
                            await b.Owner.GainArmorProcedure(d.Value, induced: true);
                            b.Emphasize();
                            await b.LoseStackProcedure();
                        }
                    }),
                }),

            new("长明灯", "获得灵气时：每1，气血+3", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_GAIN_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        GainBuffDetails d = (GainBuffDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        if (d.BuffEntry.GetName() != "灵气") return;
                        await b.Owner.HealProcedure(d.Stack * 3, induced: true);
                        b.Emphasize();
                    }),
                }),

            new("尖刺陷阱", "下次受到攻击时，对对方施加等量减甲", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Tgt || d.Src == d.Tgt) return;
                        b.Emphasize();
                        await b.Owner.RemoveArmorProcedure(d.Value, induced: false);
                    }),
                }),

            new("回合力量", "回合开始时：力量+[层数]", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("力量", b.Stack);
                    }),
                }),

            new("回合免疫", "此回合无法收到伤害", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.Owner.LoseBuffProcedure(b.GetEntry(), b.Stack);
                    }),
                    new(StageClosureDict.WIL_DAMAGE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        DamageDetails d = (DamageDetails)closureDetails;
                        if (b.Owner == d.Tgt)
                        {
                            b.Emphasize();
                            d.Cancel = true;
                        }
                    }),
                }),

            new("外骨骼", "每次攻击前，护甲+3", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Src) return;
                        b.Emphasize();
                        await b.Owner.GainArmorProcedure(3 * b.Stack, induced: true);
                    }),
                }),

            new("永动机", "[层数]回合后死亡", BuffStackRule.Min, false, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                        if (b.Owner.GetStackOfBuff("永动机") == 0)
                            await b.Owner.LoseHealthProcedure(b.Owner.Hp, false);
                    }),
                }),

            new("火箭靴", "使用灵气牌时：获得二动", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_STEP, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        EndStepDetails d = (EndStepDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        if (d.Skill != null && d.Skill.GetSkillType().Contains(SkillType.Mana))
                        {
                            b.Emphasize();
                            b.Owner.SetActionPoint(2);
                        }
                    }),
                }),

            new("定龙桩", "对方二动时：暴击补至1", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_ACTION, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        ActionDetails d = (ActionDetails)closureDetails;
                        if (b.Owner == d.Owner) return;
                        if (!d.IsSwift) return;
                        if (b.Owner.GetStackOfBuff("暴击") == 0)
                        {
                            b.Emphasize();
                            await b.Owner.GainBuffProcedure("暴击");
                        }
                    }),
                }),

            new("飞行器", "成功闪避时，对方跳行动补至1", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_EVADE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        EvadedDetails d = (EvadedDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        if (b.Owner.GetStackOfBuff("跳行动") == 0)
                        {
                            b.Emphasize();
                            await b.Owner.GainBuffProcedure("跳行动");
                        }
                    }),
                }),

            new("延迟攻", "下回合，[层数]攻", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;

                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.Owner.AttackProcedure(b.Stack, initiator: owner);
                        await b.Owner.LoseBuffProcedure(b.GetEntry(), b.Stack);
                    }),
                }),

            new("延迟护甲", "下回合，护甲+[层数]", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;

                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.Owner.GainArmorProcedure(b.Stack);
                        await b.Owner.LoseBuffProcedure(b.GetEntry(), b.Stack);
                    }),
                }),

            new("诸行无常", "击伤时：施加[层数]减甲", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_DAMAGE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        DamageDetails d = (DamageDetails)closureDetails;
                        if (!(b.Owner == d.Src && d.Src != d.Tgt))
                            return;
                        b.Emphasize();
                        await b.Owner.RemoveArmorProcedure(b.Stack, induced: false);
                    }),
                }),

            new("锋锐", "回合结束时：[层数]间接攻击", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;

                        if (b.Owner != d.Owner) return;
                        
                        b.Emphasize();

                        if (b.Owner.GetStackOfBuff("摇曳") > 0)
                        {
                            await b.Owner.RemoveArmorProcedure(b.Stack);
                        }
                        else if (b.Owner.GetStackOfBuff("凛冽") > 0)
                        {
                            await b.Owner.IndirectProcedure(b.Stack, wuXing: WuXing.Jin, lifeSteal: true);
                        }
                        else
                        {
                            await b.Owner.IndirectProcedure(b.Stack, wuXing: WuXing.Jin);
                        }
                    }),
                }),

            new("抱朴", "每回合：灵气+[层数]", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;

                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("灵气", b.Stack);
                    }),
                }),
            
            new("敛息", "击伤时：伤害替换为减甲", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_DAMAGE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        DamageDetails d = (DamageDetails)closureDetails;

                        if (b.Owner == d.Src && d.Src != d.Tgt)
                        {
                            d.Cancel = true;
                            b.Emphasize();
                            await b.Owner.RemoveArmorProcedure(d.Value, induced: false);
                            await b.LoseStackProcedure();
                        }
                    }),
                }),

            new("吸血", "下[层数]次攻击时，根据造成伤害值，回复气血", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Src) return;
                        if (d.LifeSteal) return;
                        
                        b.Emphasize();
                        d.LifeSteal = true;
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new("幻月狂乱", "攻击一直具有吸血，使用非攻击牌时：遭受1跳行动", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_STEP, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        EndStepDetails d = (EndStepDetails)closureDetails;
                        if (b.Owner == d.Owner)
                        {
                            if (!d.Skill.GetSkillType().Contains(SkillType.Attack))
                            {
                                b.Emphasize();
                                await d.Owner.GainBuffProcedure("跳行动");
                            }
                        }
                    }),
                    new(StageClosureDict.WIL_ATTACK, -1, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner == d.Src)
                        {
                            d.LifeSteal = true;
                            b.Emphasize();
                        }
                    }),
                }),
            
            new("玄武吐息法", "治疗可以穿上限", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_HEAL, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        HealDetails d = (HealDetails)closureDetails;
                        if (b.Owner == d.Tgt)
                        {
                            b.Emphasize();
                            d.Penetrate = true;
                        }
                    }),
                }),
            
            new("格挡", "受攻击时：攻击力-[层数]", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, 1, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (d.Penetrate) return;
                        if (b.Owner.GetStackOfBuff("瑞雪") > 0) return;
                        if (b.Owner == d.Tgt && d.Src != d.Tgt)
                        {
                            b.Emphasize();
                            d.Value -= b.Stack;
                        }
                    }),
                    new(StageClosureDict.DID_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;

                        if (b.Owner != d.Owner) return;
                        if (b.Owner.GetStackOfBuff("瑞雪") <= 0) return;
                        
                        b.Emphasize();
                        await b.Owner.HealProcedure(b.Stack);
                    }),
                }),

            new("闪避", "下[层数]次受攻击时：忽略攻击", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, -1, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner == d.Tgt && d.Src != d.Tgt)
                        {
                            d.Evade = true;
                            b.Emphasize();
                            await b.LoseStackProcedure();
                        }
                    }),
                }),
            
            new("轮闪避", "每轮：闪避补至[层数]", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ROUND, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        RoundDetails d = (RoundDetails)closureDetails;

                        if (b.Owner == d.Owner)
                        {
                            b.Emphasize();
                            await b.Owner.GainBuffProcedure("闪避", b.Stack - b.Owner.GetStackOfBuff("闪避"));
                        }
                    }),
                }),
            
            new("穿透", "下[层数]次攻击时，忽略对方护甲/闪避/格挡", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, -1, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Src) return;
                        if (d.Src == d.Tgt) return;
                        if (d.Penetrate) return;
                        
                        d.Penetrate = true;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new("力量", "攻击时：多[层数]攻", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                        {
                            b.Emphasize();
                            d.Value += b.Stack;
                        }
                    }),
                }),
            
            new("剑意", "攻击时：多[层数]攻，之后失去所有层数", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;

                        if (b.Owner == d.Src && d.Src != d.Tgt)
                        {
                            b.Emphasize();
                            d.Value += b.Stack;
                        }
                    }),
                    new(StageClosureDict.DID_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;

                        string thisTurnAttackedKey = "thisTurnAttacked";
                        string thisTurnPreserveJianYiKey = "thisTurnPreserveJianYi";
                        bool thisTurnAttacked = d.Owner.Memory.TryGetVariable(thisTurnAttackedKey, false);
                        bool thisTurnPreserveJianYi = d.Owner.Memory.TryGetVariable(thisTurnPreserveJianYiKey, false);

                        bool preserveJianYi = !thisTurnAttacked || thisTurnPreserveJianYi;
                        if (preserveJianYi) return;

                        b.Emphasize();
                        await b.LoseStackProcedure(b.Stack);
                    }),
                }),
            
            new("剑心", "每回合：获得[层数]剑意", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("剑意", induced: true);
                    }),
                }),
            
            new("回马枪", "下次受攻击后：[层数]攻 穿透", BuffStackRule.Max, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (!d.Recursive) return;
                        if (b.Owner == d.Tgt && d.Src != d.Tgt)
                        {
                            b.Emphasize();
                            await b.Owner.AttackProcedure(b.Stack, wuXing: WuXing.Mu, initiator: owner, recursive: false, induced: true);
                            await b.Owner.LoseBuffProcedure(b.GetEntry(), b.Stack);
                        }
                    }),
                }),
            
            new("保留剑意", "下1次攻击保留剑意", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_FULL_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (!d.Recursive) return;
                        if (b.Owner != d.Src) return;
                        if (b.Owner == d.Tgt) return;
                        if (d.DoesntConsumeJianYi) return;
                        
                        b.Emphasize();
                        d.DoesntConsumeJianYi = true;
                    }),
                }),

            new("天衣无缝", "每回合：[层数]攻，不消耗剑意", BuffStackRule.Max, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        b.Emphasize();

                        StageClosure stageClosure = new(StageClosureDict.WIL_FULL_ATTACK, 0, async (owner, closure, closureDetails) =>
                        {
                            Buff buff = owner as Buff;
                            AttackDetails d = (AttackDetails)closureDetails;
                            
                            if (buff.Owner != d.Src) return;
                            d.DoesntConsumeJianYi = true;
                        });

                        await d.Owner.AttackProcedure(b.Stack, wuXing: WuXing.Huo, initiator: owner,
                            closures: new[] { stageClosure }, induced: false);
                    }),
                    new(StageClosureDict.WIL_CAST, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CastDetails d = (CastDetails)closureDetails;

                        if (d.Caster != b.Owner) return;
                        if (d.Skill.GetSkillType().Contains(SkillType.Attack))
                        {
                            b.Emphasize();
                            await b.Owner.LoseBuffProcedure(b.GetEntry(), b.Stack);
                        }
                    }),
                }),

            new("盛开", "受到治疗时：获得[层数]力量（不会引起流转）", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_HEAL, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        HealDetails d = (HealDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        
                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("力量", b.Stack);
                    }),
                }),

            new("灼烧", "受到敌方攻击后：[层数]间接攻击", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (!d.Recursive) return;
                        if (d.Src != b.Owner && d.Tgt == b.Owner)
                        {
                            b.Emphasize();
                            await b.Owner.IndirectProcedure(b.Stack, recursive: false);
                        }
                    }),
                    new(StageClosureDict.DID_INDIRECT, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        IndirectDetails d = (IndirectDetails)closureDetails;
                        if (!d.Recursive) return;
                        if (d.Src != b.Owner && d.Tgt == b.Owner)
                        {
                            b.Emphasize();
                            await b.Owner.IndirectProcedure(b.Stack, recursive: false);
                        }
                    }),
                }),

            new("坚毅", "对方回合开始时：护甲+[层数]", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;

                        if (b.Owner == d.Owner) return;
                        
                        b.Emphasize();
                        await b.Owner.GainArmorProcedure(b.Stack, induced: true);
                    }),
                }),
            
            new("两仪", "获得护甲时/施加减甲时：额外+[层数]", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_GAIN_ARMOR, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        GainArmorDetails d = (GainArmorDetails)closureDetails;
                        if (b.Owner == d.Tgt)
                        {
                            b.Emphasize();
                            d.Value += b.Stack;
                        }
                    }),
                    new(StageClosureDict.WIL_LOSE_ARMOR, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        LoseArmorDetails d = (LoseArmorDetails)closureDetails;
                        if (b.Owner == d.Src && b.Owner != d.Tgt)
                        {
                            b.Emphasize();
                            d.Value += b.Stack;
                        }
                    }),
                }),
            
            new("人间无戈", "死亡不会停止战斗", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        StageCommitDetails d = (StageCommitDetails)closureDetails;

                        d.Cancel = true;
                    }),
                }),

            new("通透世界", "己方所有攻击具有穿透", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, -1, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                        {
                            b.Emphasize();
                            d.Penetrate = true;
                        }
                    }),
                }),
            
            new("凤凰涅槃", "每轮以及强制结算前：气血恢复至上限", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_STAGE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        StageDetails d = (StageDetails)closureDetails;

                        if (b.Owner != d.Owner) return;

                        bool allowBelow0 = b.Owner.GetStackOfBuff("人间无戈") > 0 ||
                                           b.Owner.Opponent().GetStackOfBuff("人间无戈") > 0;
                        bool isBelow0 = b.Owner.Hp <= 0;

                        if (isBelow0 && !allowBelow0) return;
                        
                        b.Emphasize();
                        await b.Owner.HealProcedure(b.Owner.MaxHp - b.Owner.Hp);
                    }),
                    new(StageClosureDict.WIL_ROUND, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        RoundDetails d = (RoundDetails)closureDetails;

                        if (b.Owner != d.Owner) return;
                        
                        b.Emphasize();
                        await b.Owner.HealProcedure(b.Owner.MaxHp - b.Owner.Hp);
                    }),
                }),
            
            new("那由他", "灵气/吟唱消耗为零，Step阶段无法受影响，所有Buff层数不会再变化", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_MANA_COST, -4, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CostDetails d = closureDetails as CostDetails;

                        b.Emphasize();
                        d.Value = 0;
                    }),
                    new(StageClosureDict.WIL_GAIN_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        GainBuffDetails d = (GainBuffDetails)closureDetails;

                        b.Emphasize();
                        d.Stack = 0;
                    }),
                    new(StageClosureDict.WIL_LOSE_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        LoseBuffDetails d = (LoseBuffDetails)closureDetails;

                        b.Emphasize();
                        d.Stack = 0;
                    }),
                    new(StageClosureDict.WIL_TURN, 101, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;

                        b.Emphasize();
                        d.Cancel = false;
                    }),
                }),

            new("钟声", "使用一张牌前：升级", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_CAST, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CastDetails d = (CastDetails)closureDetails;
                        if (b.Owner != d.Caster || d.Caster != d.Skill.Owner) return;

                        if (await d.Skill.TryUpgradeJingJie())
                        {
                            b.Emphasize();
                            await b.LoseStackProcedure();
                        }
                    }),
                }),

            // new("轮气血上限", "每轮：气血上限+[层数]", BuffStackRule.Add, true, false,
            //     eventDescriptors: new StageEventDescriptor[]
            //     {
            //         new(StageEventDict.STAGE_ENVIRONMENT, StageEventDict.WIL_ROUND, 0, async (owner，,closure, closureDetails) =>
            //         {
            //             Buff b = (Buff)owner;
            //             RoundDetails d = (RoundDetails)closureDetails;
            //             if (b.Owner != d.Owner) return;
            //
            //             b.PlayPingAnimation();
            //             b.Owner.MaxHp += b.Stack;
            //         }),
            //     }),

            new("轮暴击", "每轮：获得[层数]暴击", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ROUND, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        RoundDetails d = (RoundDetails)closureDetails;
                        if (b.Owner != d.Owner) return;

                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("暴击", b.Stack);
                    }),
                }),

            new("轮吸血", "每轮：获得[层数]吸血", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ROUND, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        RoundDetails d = (RoundDetails)closureDetails;
                        if (b.Owner != d.Owner) return;

                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("吸血", b.Stack);
                    }),
                }),

            new("轮穿透", "每轮：获得[层数]穿透", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ROUND, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        RoundDetails d = (RoundDetails)closureDetails;
                        if (b.Owner != d.Owner) return;

                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("穿透", b.Stack);
                    }),
                }),
            
            new("升华", "下一张牌在使用后，暂时移出卡组，战斗结束返还", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_CAST, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CastDetails d = (CastDetails)closureDetails;

                        if (b.Owner != d.Caster) return;
                        if (d.Skill.Exhausted) return;
                        
                        b.Emphasize();
                        await d.Skill.ExhaustProcedure();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new("火升华", "下一张火属性牌在使用后，暂时移出卡组，战斗结束返还", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_CAST, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CastDetails d = (CastDetails)closureDetails;

                        if (b.Owner != d.Caster) return;
                        if (d.Skill.Exhausted) return;
                        if (d.Skill.Entry.GetWuXing() != WuXing.Huo) return;
                        
                        b.Emphasize();
                        await d.Skill.ExhaustProcedure();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new("清心", "获得灵气时：每1，回复[层数]气血", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_GAIN_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        GainBuffDetails d = (GainBuffDetails)closureDetails;

                        if (b.Owner != d.Tgt) return;
                        if (d.BuffEntry.GetName() != "灵气") return;
                        
                        b.Emphasize();
                        await b.Owner.HealProcedure(d.Stack * b.Stack, induced: true);
                    }),
                }),
            
            new("太虚", "第二轮：气血回满", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ROUND, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        RoundDetails d = (RoundDetails)closureDetails;

                        if (b.Owner != d.Owner) return;
                        
                        b.Emphasize();
                        int gap = b.Owner.MaxHp - b.Owner.Hp;
                        await b.Owner.HealProcedure(gap);
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new("灵气返还", "失去灵气时：返还[层数]点", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_LOSE_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        LoseBuffDetails d = (LoseBuffDetails)closureDetails;

                        if (b.Owner != d.Tgt) return;
                        if (d.BuffEntry.GetName() != "灵气") return;
                        
                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("灵气", b.Stack);
                    }),
                }),
            
            new("灵敏", "使用二动牌时，获得[层数]闪避", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ACTION, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        ActionDetails d = (ActionDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        if (!d.IsSwift) return;
                        
                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("闪避", b.Stack);
                    }),
                }),
            
            new("五行亲和", "使用五行卡牌后，发生对应的流转", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_EXECUTE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        ExecuteDetails d = (ExecuteDetails)closureDetails;
                        if (b.Owner != d.Caster) return;

                        if (!d.Skill.Entry.WuXing.HasValue) return;

                        WuXing wuXing = d.Skill.Entry.WuXing.Value;

                        b.Emphasize();
                        await b.Owner.CycleProcedure(wuXing);
                    }),
                }),
            
            new("相克流转", "流转步数为2", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_CYCLE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CycleDetails d = (CycleDetails)closureDetails;
                        if (b.Owner != d.Owner) return;

                        d.Step = 2;
                        b.Emphasize();
                    }),
                }),
            
            new("素弦", "下[层数]次，攻击时，灵气+3", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Src) return;
                        if (!d.Recursive) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                        await d.Src.GainBuffProcedure("灵气", 3, induced: true);
                    }),
                }),
            
            new("苦寒", "下[层数]次，攻击后，下一回合具有二动", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Src) return;
                        if (!d.Recursive) return;
                        
                        b.Emphasize();
                        await b.LoseStackProcedure();
                        await d.Src.GainBuffProcedure("二动", induced: true);
                    }),
                }),
            
            new("弱昙", "下[层数]次，攻击时，力量+1", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Src) return;
                        if (!d.Recursive) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                        await d.Src.GainBuffProcedure("力量", induced: true);
                    }),
                }),
            
            new("狂焰", "下[层数]次，攻击时，多8攻", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_FULL_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Src) return;
                        if (!d.Recursive) return;
                        
                        b.Emphasize();
                        d.Value += 8;
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new("击伤赋予护甲", "下[层数]次，攻击时，护甲+击伤值", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Src) return;
                        if (!d.Recursive) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                        await d.Src.GainArmorProcedure(d.Value, induced: true);
                    }),
                }),
            
            new("护甲返还", "下[层数]次，失去护甲时，返还", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_LOSE_ARMOR, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        LoseArmorDetails d = (LoseArmorDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                        await b.Owner.GainArmorProcedure(d.Value, induced: true);
                    }),
                }),
            
            new("一梦如是", "下[层数]次，受伤害时，变为治疗", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_DAMAGE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        DamageDetails d = (DamageDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                        d.Cancel = true;
                        await b.Owner.HealProcedure(d.Value, induced: true);
                    }),
                }),
            
            new("灵虚步", "成功闪避后，双发+1", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_EVADE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        EvadedDetails d = (EvadedDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("一念无量劫");
                    }),
                }),
            
            new("恶意", "下1次施加破甲时将会流失气血", BuffStackRule.Add, false, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_LOSE_ARMOR, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        LoseArmorDetails d = (LoseArmorDetails)closureDetails;
                        if (b.Owner.Opponent() != d.Tgt) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                        await b.Owner.Opponent().LoseHealthProcedure(d.Value, false);
                    }),
                }),
            
            new("花海", "每回合：力量+1", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("力量", b.Stack);
                    }),
                }),
            
            new("他心通", "下次敌方获得增益时，自己也获得", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_GAIN_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        GainBuffDetails d = (GainBuffDetails)closureDetails;
                        if (!d.Recursive) return;
                        if (d.BuffEntry == (BuffEntry)("他心通")) return;
                        if (b.Owner.Opponent() != d.Tgt) return;
                        if (!d.BuffEntry.Friendly) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                        await b.Owner.GainBuffProcedure(d.BuffEntry, d.Stack, recursive: false);
                    }),
                }),
            
            new("火墙", "若在下次使用前，没有遭受[stack]次伤害，则卡牌激活", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_ATTACK, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        AttackDetails d = (AttackDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new("伤害上限", "受到伤害时，不超过[层数]点", BuffStackRule.Min, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_DAMAGE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        DamageDetails d = (DamageDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        b.Emphasize();
                        d.Value = d.Value.ClampUpper(b.Stack);
                    }),
                }),
            
            new("净体", "每轮：净化2", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_ROUND, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        RoundDetails d = (RoundDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.Owner.DispelProcedure(2);
                    }),
                }),
            
            new("淬体", "燃命时：获得[层数]灼烧（不会引起流转）", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_BURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        BurnDetails d = (BurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("灼烧", b.Stack);
                    }),
                }),
            
            new("浴火", "燃命时：根据灼烧造成伤害", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_BURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        BurnDetails d = (BurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        int stack = b.Owner.GetStackOfBuff("灼烧");
                            
                        b.Emphasize();
                        await b.Owner.IndirectProcedure(stack, wuXing: WuXing.Huo);
                    }),
                }),
            
            new("飞鸿踏雪", "二动时：获得1格挡", BuffStackRule.One, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_ACTION, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        ActionDetails d = (ActionDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        if (!d.IsSwift) return;
                        b.Emphasize();
                        await b.Owner.CycleProcedure(WuXing.Shui, gain: 1, induced: true);
                    }),
                }),
            
            new("磐石", "吟唱时：坚毅+1", BuffStackRule.Max, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_CHANNEL, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        ChannelDetails d = (ChannelDetails)closureDetails;
                        if (b.Owner != d.Caster) return;
                        
                        b.Emphasize();
                        await b.Owner.GainBuffProcedure("坚毅", 1, induced: true);
                    }),
                }),
            
            new("同心蛊", "下[层数]次受治疗时，对敌方造成等量伤害", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.DID_HEAL, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        HealDetails d = (HealDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        
                        b.Emphasize();
                        await b.LoseStackProcedure();
                        await b.Owner.Opponent().IndirectProcedure(d.Value, induced: true);
                    }),
                }),
                
            new("空明", "下一次获得五行Buff时，额外[层数]点", BuffStackRule.Add, true, false,
                closures: new StageClosure[]
                {
                    new(StageClosureDict.WIL_GAIN_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        GainBuffDetails d = (GainBuffDetails)closureDetails;
                        
                        if (b.Owner != d.Tgt) return;  // 不是buff持有者获得的buff则不处理
                        
                        // 检查是否是五行buff之一
                        BuffEntry[] wuXingBuffs = { "锋锐", "格挡", "力量", "灼烧", "坚毅" };
                        if (!wuXingBuffs.Contains(d.BuffEntry.GetName()))
                            return;
                        
                        b.Emphasize();
                        d.Stack += b.Stack;
                        await b.Owner.LoseBuffProcedure(b.GetEntry(), b.Stack);  // 消耗空明
                    }),
                }),
            
            new(id:                         "天人形态",
                description:                "造成伤害翻倍，受伤减半，触发满血/残血",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_DAMAGE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        DamageDetails d = (DamageDetails)closureDetails;
                        bool cond = b.Owner == d.Src && b.Owner.Opponent() == d.Tgt;
                        if (!cond) return;
                        b.Emphasize();
                        d.Value = d.Value << b.Stack;
                    }),
                    new(StageClosureDict.WIL_DAMAGE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        DamageDetails d = (DamageDetails)closureDetails;
                        bool cond = b.Owner == d.Tgt;
                        if (!cond) return;
                        b.Emphasize();
                        d.Value = d.Value >> b.Stack;
                    }),
                }),
            
            new(id:                         "锻体",
                description:                "残血所需的阈值提升，满血所需的阈值降低，50层之后，可化身天人形态",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_GAIN_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        GainBuffDetails d = (GainBuffDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;

                        int duanTiStack = b.Stack;
                        int tianRenGain = duanTiStack / 50;
                        if (tianRenGain <= 0) return;
                        int consumption = tianRenGain * 50;
                        await b.Owner.LoseBuffProcedure(b.GetEntry(), consumption);
                        await b.Owner.GainBuffProcedure("天人形态", tianRenGain, induced: true);
                    }),
                }),
            
            new(id:                         "常仪",
                description:                "流转时：造成伤害",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_CYCLE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CycleDetails d = (CycleDetails)closureDetails;
                        if (b.Owner != d.Owner) return;

                        int stack = d.Owner.GetStackOfBuff(d.WuXing._elementaryBuff);
                        await d.Owner.IndirectProcedure(stack, induced: true);
                    }),
                }),
            
            new(id:                         "羲和",
                description:                "使用牌时：流转对应五行",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_CAST, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CastDetails d = (CastDetails)closureDetails;
                        if (b.Owner != d.Caster) return;

                        WuXing? wuXing = d.Skill.Entry.GetWuXing();
                        if (!wuXing.HasValue) return;

                        await d.Caster.CycleProcedure(wuXing.Value);
                    }),
                }),
            
            new(id:                         "塑魂",
                description:                "灵气不足时，可消耗[层数]锻体代替1灵气",
                buffStackRule:              BuffStackRule.Min,
                friendly:                   true,
                dispellable:                false),
            
            new(id:                         "不屈",
                description:                "持续[层数]回合，气血无法降低至0",
                buffStackRule:              BuffStackRule.Max,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_LOSE_HEALTH, 1, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        LoseHealthDetails d = (LoseHealthDetails)closureDetails;
                        if (b.Owner != d.Victim) return;

                        int upperBound = b.Owner.Hp - 1;
                        d.Value = d.Value.ClampUpper(upperBound);
                        b.Emphasize();
                    }),
                    new(StageClosureDict.WIL_TURN, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        TurnDetails d = (TurnDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new(id:                         "万剑归宗",
                description:                "无法使用攻击牌，每轮：使用所有攻击牌",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_CAST, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CastDetails d = (CastDetails)closureDetails;
                        if (b.Owner != d.Caster) return;
                        if (d.FromWanJian) return;

                        if (!d.Skill.GetSkillType().Contains(SkillType.Attack)) return;
                        d.Skill = d.Caster.EmptyAction;
                        b.Emphasize();
                    }),
                    new(StageClosureDict.WIL_ROUND, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        RoundDetails d = (RoundDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        
                        b.Emphasize();
                        foreach (StageSkill skill in d.Owner.TraversalSkills())
                        {
                            if (!skill.GetSkillType().Contains(SkillType.Attack)) continue;
                            await b.Owner.CastProcedure(skill, fromWanJian: true);
                        }
                    }),
                }),
            
            new(id:                         "童趣",
                description:                "复制对手下一次获得的增益",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_GAIN_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        GainBuffDetails d = closureDetails as GainBuffDetails;
                        if (!d.Recursive) return;
                        if (b.Owner.Opponent() != d.Tgt) return;
                        if (!d.BuffEntry.Friendly) return;

                        await b.Owner.GainBuffProcedure(d.BuffEntry, d.Stack, recursive: false, induced: true);
                        await b.LoseStackProcedure();
                        b.Emphasize();
                    }),
                }),
            
            new(id:                         "一心",
                description:                "下一次吟唱免费",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_CHANNEL_COST, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        CostDetails d = closureDetails as CostDetails;
                        if (b.Owner != d.Entity) return;
                        if (d.Value <= 0) return;

                        d.Value = 0;
                        await b.LoseStackProcedure();
                        b.Emphasize();
                    }),
                }),

            new("终结", "激活下一个终结效果",                         BuffStackRule.Add, true, false),
            new("摩诃钵特摩", "已经触发过摩诃钵特摩",                   BuffStackRule.One, true, false),
            new("天人合一", "已经触发过天人合一",                       BuffStackRule.One, true, false),
            new("连岳", "最后两张牌都可以触发终结",                     BuffStackRule.One, true, false),
            new("凛冽", "锋锐具有吸血",                                 BuffStackRule.One, true, false),
            new("摇曳", "锋锐变为施加破甲",                             BuffStackRule.One, true, false),
            new("瑞雪", "格挡变为治疗",                                 BuffStackRule.One, true, false),
            new("仙人抚顶", "使用12次后：将对方气血变为0",              BuffStackRule.Add, true, false),
            new("飞龙在天", $"跳过下[层数]张牌，跳过时成长计数+1",      BuffStackRule.Add, true, false),
            new("齐物论",     "奇偶同时激活两个效果",                   BuffStackRule.One, true, false),
            new("鹤回翔",     "反转出牌顺序",                           BuffStackRule.One, true, false),
            new("跳卡牌",     "行动时跳过下张卡牌",                     BuffStackRule.Add, false, false),
            new("集中",      "下一次使用牌时，条件算作激活",            BuffStackRule.Add, true, false),
            new("浮空艇",     "回合被跳过时：气血及上线无法下降",       BuffStackRule.Add, true, false),
        });
    }

    public void Init()
    {
        List.Do(entry => entry.GenerateCascade());
    }

    public override BuffEntry DefaultEntry() => this["不存在的Buff"];
}
