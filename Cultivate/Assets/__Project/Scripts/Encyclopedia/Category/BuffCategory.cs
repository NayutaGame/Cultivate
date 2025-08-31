
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
            #region 00特殊
            
            new(id:                         "Buff00_001",
                name:                       "不存在的Buff",
                rawDescription:             "不存在的Buff",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   null),

            #endregion
            
            #region 01金
            
            new(id:                         "Buff01_001",
                name:                       "暴击",
                rawDescription:             "下一次攻击造成的伤害翻倍",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff01_002",
                name:                       "轮暴击",
                rawDescription:             "每轮：获得[层数]暴击",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff01_003",
                name:                       "诸行无常",
                rawDescription:             "击伤时：施加[层数]减甲",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_DAMAGE, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        DamageDetails d = (DamageDetails)closureDetails;
                        if (!(b.Owner == d.Src && d.Src != d.Tgt))
                            return;
                        b.Emphasize();
                        await b.Owner.RemoveArmorProcedure(b.Stack, induced: true);
                    }),
                }),
            
            new(id:                         "Buff01_004",
                name:                       "敛息",
                rawDescription:             "击伤时：伤害替换为减甲",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff01_005",
                name:                       "锋锐",
                rawDescription:             "回合结束时：[层数]间接攻击",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff01_006",
                name:                       "凛冽",
                rawDescription:             "锋锐具有吸血",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false),
            
            new(id:                         "Buff01_007",
                name:                       "摇曳",
                rawDescription:             "锋锐变为施加破甲",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false),
            
            new(id:                         "Buff01_008",
                name:                       "人间无戈",
                rawDescription:             "死亡不会停止战斗",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_COMMIT, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        StageCommitDetails d = (StageCommitDetails)closureDetails;

                        d.Cancel = true;
                    }),
                }),

            #endregion
            
            #region 02水

            new(id:                         "Buff02_001",
                name:                       "吸血",
                rawDescription:             "下[层数]次攻击时，根据造成伤害值，回复气血",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff02_002",
                name:                       "轮吸血",
                rawDescription:             "每轮：获得[层数]吸血",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff02_003",
                name:                       "格挡",
                rawDescription:             "受攻击时：攻击力-[层数]",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff02_004",
                name:                       "瑞雪",
                rawDescription:             "格挡变为治疗",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false),
            
            new(id:                         "Buff02_005",
                name:                       "飞鸿踏雪",
                rawDescription:             "二动时：获得1格挡",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff02_006",
                name:                       "二动",
                rawDescription:             "下一回合二动",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff02_007",
                name:                       "玄武吐息法",
                rawDescription:             "治疗可以穿上限",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff02_008",
                name:                       "一梦如是",
                rawDescription:             "下[层数]次，受伤害时，变为治疗",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff02_009",
                name:                       "摩诃钵特摩",
                rawDescription:             "已经触发过摩诃钵特摩",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false),

            #endregion
            
            #region 03木
            
            new(id:                         "Buff03_001",
                name:                       "二重",
                rawDescription:             "下[层数]张牌使用两次",
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
            
            new(id:                         "Buff03_002",
                name:                       "永久二重",
                rawDescription:             "所有牌使用两次",
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

            new(id:                         "Buff03_003",
                name:                       "钟声",
                rawDescription:             "使用一张牌前：升级",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff03_004",
                name:                       "多重",
                rawDescription:             "下一张牌额外使用[层数]次，最高20层",
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

            new(id:                         "Buff03_005",
                name:                       "闪避",
                rawDescription:             "下[层数]次受攻击时：忽略攻击",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff03_006",
                name:                       "轮闪避",
                rawDescription:             "每轮：闪避补至[层数]",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff03_007",
                name:                       "穿透",
                rawDescription:             "下[层数]次攻击时，忽略对方护甲/闪避/格挡",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff03_008",
                name:                       "轮穿透",
                rawDescription:             "每轮：获得[层数]穿透",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff03_009",
                name:                       "力量",
                rawDescription:             "攻击时：多[层数]攻",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff03_010",
                name:                       "盛开",
                rawDescription:             "受到治疗时：获得[层数]力量（不会引起流转）",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff03_011",
                name:                       "花海",
                rawDescription:             "每回合：力量+1",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff03_012",
                name:                       "飞龙在天",
                rawDescription:             $"跳过下[层数]张牌，跳过时成长计数+1",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false),
            
            new(id:                         "Buff03_013",
                name:                       "鹤回翔",
                rawDescription:             "反转出牌顺序",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false),

            new(id:                         "Buff03_014",
                name:                       "通透世界",
                rawDescription:             "己方所有攻击具有穿透",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            #endregion
            
            #region 04火
            
            new(id:                         "Buff04_001",
                name:                       "剑意",
                rawDescription:             "攻击时：多[层数]攻，之后失去所有层数",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff04_002",
                name:                       "保留剑意",
                rawDescription:             "下1次攻击保留剑意",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
                        await b.LoseStackProcedure();
                    }),
                }),
            
            new(id:                         "Buff04_003",
                name:                       "剑心",
                rawDescription:             "每回合：获得[层数]剑意",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff04_004",
                name:                       "天衣无缝",
                rawDescription:             "每回合：[层数]攻，不消耗剑意",
                buffStackRule:              BuffStackRule.Max,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
                        if (d.Skill.GetTagComposite().Contains(TagCategory.Attack))
                        {
                            b.Emphasize();
                            await b.Owner.LoseBuffProcedure(b.GetEntry(), b.Stack);
                        }
                    }),
                }),

            new(id:                         "Buff04_005",
                name:                       "灼烧",
                rawDescription:             "受到敌方攻击后：[层数]间接攻击",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff04_006",
                name:                       "淬体",
                rawDescription:             "燃命时：获得[层数]灼烧（不会引起流转）",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff04_007",
                name:                       "浴火",
                rawDescription:             "燃命时：根据灼烧造成伤害",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff04_008",
                name:                       "升华",
                rawDescription:             "下一张牌在使用后，暂时移出卡组，战斗结束返还",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff04_009",
                name:                       "火升华",
                rawDescription:             "下一张火属性牌在使用后，暂时移出卡组，战斗结束返还",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff04_010",
                name:                       "不屈",
                rawDescription:             "持续[层数]回合，气血无法降低至0",
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
            
            new(id:                         "Buff04_011",
                name:                       "凤凰涅槃",
                rawDescription:             "每轮以及强制结算前：气血恢复至上限",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            #endregion
            
            #region 05土
            
            new(id:                         "Buff05_001",
                name:                       "终结",
                rawDescription:             "激活下一个终结效果",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false),
            
            new(id:                         "Buff05_002",
                name:                       "连岳",
                rawDescription:             "最后两张牌都可以触发终结",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false),
            
            new(id:                         "Buff05_003",
                name:                       "锻体",
                rawDescription:             "残血所需的阈值提升，满血所需的阈值降低，50层之后，可化身天人形态",
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
            
            new(id:                         "Buff05_004",
                name:                       "天人形态",
                rawDescription:             "造成伤害翻倍，受伤减半，触发满血/残血",
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

            new(id:                         "Buff05_005",
                name:                       "坚毅",
                rawDescription:             "对方回合开始时：护甲+[层数]",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff05_006",
                name:                       "磐石",
                rawDescription:             "吟唱时：坚毅+1",
                buffStackRule:              BuffStackRule.Max,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff05_007",
                name:                       "净体",
                rawDescription:             "每轮：净化2",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff05_008",
                name:                       "天人合一",
                rawDescription:             "已经触发过天人合一",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false),
            
            new(id:                         "Buff05_009",
                name:                       "那由他",
                rawDescription:             "灵气/吟唱消耗为零，Step阶段无法受影响，所有Buff层数不会再变化",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            #endregion
            
            #region 06无色
            
            new(id:                         "Buff06_001",
                name:                       "灵气",
                rawDescription:             "可以消耗灵气使用技能",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   null),
            
            new(id:                         "Buff06_002",
                name:                       "免费",
                rawDescription:             "持续[层数]次，使用牌时：无需消耗灵气",
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
            
            new(id:                         "Buff06_003",
                name:                       "永久免费",
                rawDescription:             "使用牌时：无需消耗灵气",
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

            new(id:                         "Buff06_004",
                name:                       "灵气返还",
                rawDescription:             "下一次灵气减少时，加回",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff06_005",
                name:                       "抱朴",
                rawDescription:             "每回合：灵气+[层数]",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff06_006",
                name:                       "清心",
                rawDescription:             "获得灵气时：每1，回复[层数]气血",
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
                        if (d.BuffEntry.GetName() != "灵气") return;
                        
                        b.Emphasize();
                        await b.Owner.HealProcedure(d.Stack * b.Stack, induced: true);
                    }),
                }),
            
            new(id:                         "Buff06_007",
                name:                       "同心蛊",
                rawDescription:             "下[层数]次受治疗时，对敌方造成等量伤害",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_HEAL, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        HealDetails d = (HealDetails)closureDetails;
                        if (b.Owner != d.Tgt) return;
                        
                        b.Emphasize();
                        await b.LoseStackProcedure();
                        await b.Owner.IndirectProcedure(d.Value, induced: true);
                    }),
                }),
            
            new(id:                         "Buff06_008",
                name:                       "太虚",
                rawDescription:             "第二轮：气血回满",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff06_009",
                name:                       "延迟攻",
                rawDescription:             "下回合，[层数]攻",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff06_010",
                name:                       "延迟护甲",
                rawDescription:             "下回合，护甲+[层数]",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff06_011",
                name:                       "护甲返还",
                rawDescription:             "下[层数]次，失去护甲时，返还",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff06_012",
                name:                       "常仪",
                rawDescription:             "流转时：造成伤害",
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

                        int stack = d.Owner.GetStackOfBuff(d.WuXing.GetElementaryBuff());
                        await d.Owner.IndirectProcedure(stack, induced: true);
                    }),
                }),
            
            new(id:                         "Buff06_013",
                name:                       "羲和",
                rawDescription:             "使用牌时：流转对应五行",
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

                        WuXing wuXing = d.Skill.Entry.GetWuXing();
                        if (!wuXing.IsBasic()) return;

                        await d.Caster.CycleProcedure(wuXing);
                    }),
                }),
            
            new(id:                         "Buff06_014",
                name:                       "万剑归宗",
                rawDescription:             "无法使用攻击牌，每轮：使用所有攻击牌",
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

                        if (!d.Skill.GetTagComposite().Contains(TagCategory.Attack)) return;
                        d.Skill = d.Caster.EmptyAction;
                        b.Emphasize();
                    }),
                    new(StageClosureDict.WIL_ROUND, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        RoundDetails d = (RoundDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        
                        b.Emphasize();
                        foreach (StageSkill skill in d.Owner.Skills)
                        {
                            if (!skill.GetTagComposite().Contains(TagCategory.Attack)) continue;
                            await b.Owner.CastProcedure(skill, fromWanJian: true);
                        }
                    }),
                }),
            
            #endregion
            
            #region 07Debuff
            
            new(id:                         "Buff07_001",
                name:                       "跳走步",
                rawDescription:             "跳过走步阶段",
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
            
            new(id:                         "Buff07_002",
                name:                       "跳卡牌",
                rawDescription:             "行动时跳过下张卡牌",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                false),
            
            new(id:                         "Buff07_003",
                name:                       "跳行动",
                rawDescription:             "跳过[层数]次行动",
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
            
            new(id:                         "Buff07_004",
                name:                       "滞气",
                rawDescription:             "每回合：失去[层数]灵气，层数-1",
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
            
            new(id:                         "Buff07_005",
                name:                       "缠绕",
                rawDescription:             "无法二动/三动\n回合结束/二动时：-1层",
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
            
            new(id:                         "Buff07_006",
                name:                       "软弱",
                rawDescription:             "攻击时：少[层数]攻\n回合结束/攻击时：-1层",
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
            
            new(id:                         "Buff07_007",
                name:                       "腐朽",
                rawDescription:             "每回合：失去[层数]护甲，层数-1",
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
            
            new(id:                         "Buff07_008",
                name:                       "内伤",
                rawDescription:             "每回合：失去[层数]气血，层数-1",
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
            
            new(id:                         "Buff07_009",
                name:                       "脆弱",
                rawDescription:             "受攻击时：多[层数]攻\n回合结束/受攻击时：-1层",
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

            #endregion
            
            #region 08禁忌

            new(id:                         "Buff08_001",
                name:                       "禁止治疗",
                rawDescription:             "无法受到治疗",
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
            
            new(id:                         "Buff08_002",
                name:                       "禁止护甲",
                rawDescription:             "无法获得护甲",
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
            
            new(id:                         "Buff08_003",
                name:                       "禁止二动",
                rawDescription:             "无法二动",
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
            
            new(id:                         "Buff08_004",
                name:                       "禁止行动",
                rawDescription:             "无法行动",
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
            
            new(id:                         "Buff08_005",
                name:                       "禁止灵气",
                rawDescription:             "无法获得灵气",
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
            
            new(id:                         "Buff08_006",
                name:                       "禁止攻击",
                rawDescription:             "无法攻击",
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
            
            new(id:                         "Buff08_007",
                name:                       "不堪一击",
                rawDescription:             "受击伤：气血降至0",
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
            
            #endregion
            
            new(id:                         "Buff99_001",
                name:                       "心斋",
                rawDescription:             "所有耗蓝-[层数]",
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
            
            new(id:                         "Buff99_002",
                name:                       "高速吟唱",
                rawDescription:             "吟唱时，额外推进[层数]进度",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_003",
                name:                       "碎防",
                rawDescription:             "下一次攻击时，1点伤害抵消2点护甲",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_004",
                name:                       "击伤赋予护甲",
                rawDescription:             "下[层数]次，攻击时，护甲+击伤值",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_005",
                name:                       "六爻化劫",
                rawDescription:             "第二轮开始时，双方重置气血上限，回[层数]%血",
                buffStackRule:              BuffStackRule.Max,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_006",
                name:                       "空明",
                rawDescription:             "下一次获得五行Buff时，额外[层数]点",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.WIL_GAIN_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        GainBuffDetails d = (GainBuffDetails)closureDetails;
                        
                        if (b.Owner != d.Tgt) return;  // 不是buff持有者获得的buff则不处理
                        
                        if (!WuXing.TraversalBasic.Map(wuXing => wuXing.GetElementaryBuff()).Contains(d.BuffEntry))
                            return;
                        
                        b.Emphasize();
                        d.Stack += b.Stack;
                        await b.Owner.LoseBuffProcedure(b.GetEntry(), b.Stack);  // 消耗空明
                    }),
                }),
            
            new(id:                         "Buff99_007",
                name:                       "童趣",
                rawDescription:             "复制对手下一次获得的增益",
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
            
            new(id:                         "Buff99_008",
                name:                       "一心",
                rawDescription:             "下一次吟唱无需消耗",
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
            
            new(id:                         "Buff99_009",
                name:                       "塑魂",
                rawDescription:             "灵气不足时，可消耗[层数]锻体代替1灵气",
                buffStackRule:              BuffStackRule.Min,
                friendly:                   true,
                dispellable:                false),
            
            new(id:                         "Buff99_010",
                name:                       "灵虚步",
                rawDescription:             "成功闪避后，双发+1",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_011",
                name:                       "灵敏",
                rawDescription:             "使用二动牌时，获得[层数]闪避",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_012",
                name:                       "两仪",
                rawDescription:             "获得护甲时/施加减甲时：额外+[层数]",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_013",
                name:                       "幻月狂乱",
                rawDescription:             "攻击一直具有吸血，使用非攻击牌时：遭受1跳行动",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_STEP, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        EndStepDetails d = (EndStepDetails)closureDetails;
                        if (b.Owner == d.Owner)
                        {
                            if (!d.Skill.GetTagComposite().Contains(TagCategory.Attack))
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
            
            new(id:                         "Buff99_014",
                name:                       "仙人抚顶",
                rawDescription:             "使用12次后：将对方气血变为0",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false),
            
            new(id:                         "Buff99_015",
                name:                       "集中",
                rawDescription:             "下一次使用牌时，条件算作激活",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false),
            
            new(id:                         "Buff99_016",
                name:                       "浮空艇",
                rawDescription:             "回合被跳过时：气血及上线无法下降",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false),

            new(id:                         "Buff99_017",
                name:                       "长明灯",
                rawDescription:             "获得灵气时：每1，气血+3",
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
                        if (d.BuffEntry.GetName() != "灵气") return;
                        await b.Owner.HealProcedure(d.Stack * 3, induced: true);
                        b.Emphasize();
                    }),
                }),

            new(id:                         "Buff99_018",
                name:                       "尖刺陷阱",
                rawDescription:             "下次受到攻击时，对对方施加等量减甲",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff99_019",
                name:                       "回合力量",
                rawDescription:             "回合开始时：力量+[层数]",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff99_020",
                name:                       "回合免疫",
                rawDescription:             "此回合无法收到伤害",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff99_021",
                name:                       "外骨骼",
                rawDescription:             "每次攻击前，护甲+3",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff99_022",
                name:                       "永动机",
                rawDescription:             "[层数]回合后死亡",
                buffStackRule:              BuffStackRule.Min,
                friendly:                   false,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff99_023",
                name:                       "火箭靴",
                rawDescription:             "使用灵气牌时：获得二动",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_STEP, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        EndStepDetails d = (EndStepDetails)closureDetails;
                        if (b.Owner != d.Owner) return;
                        if (d.Skill != null && d.Skill.GetTagComposite().Contains(TagCategory.Mana))
                        {
                            b.Emphasize();
                            b.Owner.SetActionPoint(2);
                        }
                    }),
                }),

            new(id:                         "Buff99_024",
                name:                       "定龙桩",
                rawDescription:             "对方二动时：暴击补至1",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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

            new(id:                         "Buff99_025",
                name:                       "飞行器",
                rawDescription:             "成功闪避时，对方跳行动补至1",
                buffStackRule:              BuffStackRule.One,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_026",
                name:                       "素弦",
                rawDescription:             "下[层数]次，攻击时，灵气+3",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_027",
                name:                       "苦寒",
                rawDescription:             "下[层数]次，攻击后，下一回合具有二动",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_028",
                name:                       "弱昙",
                rawDescription:             "下[层数]次，攻击时，力量+1",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_029",
                name:                       "狂焰",
                rawDescription:             "下[层数]次，攻击时，多8攻",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_030",
                name:                       "恶意",
                rawDescription:             "下1次施加破甲时将会流失气血",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   false,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_031",
                name:                       "火墙",
                rawDescription:             "若在下次使用前，没有遭受[stack]次伤害，则卡牌激活",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
            
            new(id:                         "Buff99_032",
                name:                       "他心通",
                rawDescription:             "下次敌方获得增益时，自己也获得",
                buffStackRule:              BuffStackRule.Add,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
                {
                    new(StageClosureDict.DID_GAIN_BUFF, 0, async (owner, closure, closureDetails) =>
                    {
                        Buff b = (Buff)owner;
                        GainBuffDetails d = (GainBuffDetails)closureDetails;
                        if (!d.Recursive) return;
                        if (d.BuffEntry == FromName("他心通")) return;
                        if (b.Owner.Opponent() != d.Tgt) return;
                        if (!d.BuffEntry.Friendly) return;
                        b.Emphasize();
                        await b.LoseStackProcedure();
                        await b.Owner.GainBuffProcedure(d.BuffEntry, d.Stack, recursive: false);
                    }),
                }),
            
            new(id:                         "Buff99_033",
                name:                       "伤害上限",
                rawDescription:             "受到伤害时，不超过[层数]点",
                buffStackRule:              BuffStackRule.Min,
                friendly:                   true,
                dispellable:                false,
                closures:                   new StageClosure[]
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
        });
    }

    // public override BuffEntry DefaultEntry() => this["不存在的Buff"];

    private BuffEntry[] _debuffs;
    public BuffEntry[] GetDebuffs()
    {
        if (_debuffs != null)
            return _debuffs;

        _debuffs = new BuffEntry[] { FromName("滞气"), FromName("缠绕"), FromName("软弱"), FromName("腐朽"), FromName("内伤"), };
        return _debuffs;
    }
}
