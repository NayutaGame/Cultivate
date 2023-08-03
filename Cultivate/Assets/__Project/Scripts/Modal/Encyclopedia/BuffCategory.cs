using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class BuffCategory : Category<BuffEntry>
{
    public BuffCategory()
    {
        AddRange(new List<BuffEntry>()
        {
            new("不存在的Buff", "不存在的Buff", BuffStackRule.Add, true, false),
            new("灵气", "可以消耗灵气使用技能", BuffStackRule.Add, true, false),
            new("跳回合", "跳过回合", BuffStackRule.Add, false, false),
            new("跳卡牌", "行动时跳过下张卡牌", BuffStackRule.Add, false, false),
            new("双发", "下一张牌使用两次", BuffStackRule.Wasted, true, false),
            new("二动", "下一张牌二动", BuffStackRule.Add, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("StartTurn", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        d.Owner.Swift = true;
                        b.Stack -= 1;
                    }),
                }),
            new("免费", "下一次耗蓝时无需灵气", BuffStackRule.Add, true, false),
            new("集中", "下一次使用牌时，条件算作激活", BuffStackRule.Add, true, false),
            new("永久集中", "使用牌时，条件算作激活", BuffStackRule.Add, true, false),
            new("六爻化劫", "第二轮开始时，双方重置生命上限，回[层数]%血", BuffStackRule.Max, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("StartRound", async (listener, stageEventDetails) =>
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
                eventCaptures: new StageEventCapture[]
                {
                    new("DidDispel", async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DispelDetails d = (DispelDetails)eventDetails;
                        if (b.Owner != d.Tgt) return;
                        if (d._buffEntry.Name != "灵气") return;

                        await d.Tgt.BuffSelfProcedure("灵气", d._stack);
                        b.Stack -= 1;
                    }),
                }),

            new("护甲回收", "下一次护甲减少时，加回", BuffStackRule.Add, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("ArmorDidLose", async (listener, eventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ArmorLoseDetails d = (ArmorLoseDetails)eventDetails;
                        if (b.Owner == d.Tgt)
                        {
                            await b.Owner.ArmorGainSelfProcedure(d.Value);
                            b.Stack -= 1;
                        }
                    }),
                }),

            /***
             * 禁止治疗
             * 永久双发
             * 长明灯
             * 永久免费
             * 永久穿透
             * 尖刺陷阱
             * 回合力量
             * 浮空艇
             * 外骨骼
             * 永动机
             * 火箭靴
             * 定龙桩
             * 飞行器
             * 时光机
             */

            new("延迟护甲", "下回合护甲+[层数]", BuffStackRule.Add, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("StartTurn", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        await b.Owner.ArmorGainSelfProcedure(b.Stack);
                        await b.Owner.RemoveBuff(b);
                    }),
                }),

            new("无常已至", "造成伤害：施加[伤害值，最多Stack]减甲", BuffStackRule.Add, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("DidDamage", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;
                        if (!(b.Owner == d.Src && d.Src != d.Tgt))
                            return;
                        await b.Owner.ArmorLoseOppoProcedure(Mathf.Min(d.Value, b.Stack));
                    }),
                }),

            new("锋锐", "每回合：[层数]攻\n受到伤害后层数-1", BuffStackRule.Add, true, true,
                eventCaptures: new StageEventCapture[]
                {
                    new("EndTurn", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        await b.Owner.AttackProcedure(b.Stack, wuXing: WuXing.Jin);
                    }),
                    new("DidDamage", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            b.Stack -= 1;
                    }),
                }),
            new("森罗万象", "奇偶同时激活两个效果", BuffStackRule.Wasted, true, false),

            new("自动灵气", "每回合：灵气+[层数]", BuffStackRule.Add, true, true,
                eventCaptures: new StageEventCapture[]
                {
                    new("StartTurn", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        await b.Owner.BuffSelfProcedure("灵气", b.Stack);
                    }),
                }),
            new("敛息", "造成伤害时：取消伤害，施加减甲", BuffStackRule.Add, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("WillDamage", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        DamageDetails d = (DamageDetails)stageEventDetails;

                        if (b.Owner == d.Src && d.Src != d.Tgt)
                        {
                            d.Cancel = true;
                            await b.Owner.ArmorLoseOppoProcedure(d.Value);
                            b.Stack -= 1;
                        }
                    }),
                }),

            new("吸血", "下一次攻击造成伤害时，回复生命", BuffStackRule.Add, true, true,
                eventCaptures: new StageEventCapture[]
                {
                    new("WillAttack", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src)
                        {
                            d.LifeSteal = true;
                            b.Stack -= 1;
                        }
                    }),
                }),
            new("凝神", "下一次受到治疗：护甲+治疗量", BuffStackRule.Add, true, true,
                eventCaptures: new StageEventCapture[]
                {
                    new("DidHeal", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        HealDetails d = (HealDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                        {
                            await b.Owner.ArmorGainSelfProcedure(d.Value);
                            b.Stack -= 1;
                        }
                    }),
                }),
            new("永久吸血", "攻击一直具有吸血，直到使用非攻击牌", BuffStackRule.Wasted, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("StartStep", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StepDetails d = (StepDetails)stageEventDetails;
                        if (b.Owner == d.Owner)
                        {
                            if (!d.Skill.GetSkillType().Contains(SkillTypeCollection.Attack))
                                b.Stack -= 1;
                        }
                    }),
                    new("WillAttack", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src)
                        {
                            d.LifeSteal = true;
                        }
                    }),
                }),

            // new("治疗转灵气", "受到治疗时：灵气+[Stack]", BuffStackRule.Add, true, false,
            //     healed: async (buff, d) => await d.Tgt.BuffSelfProcedure("灵气", buff.Stack)),
            // new("治疗转二动", "被治疗时，如果实际治疗>=20，二动", BuffStackRule.Wasted, true, false,
            //     healed: (buff, d) =>
            //     {
            //         int actualHealed = Mathf.Min(d.Tgt.MaxHp - d.Tgt.Hp, d.Value);
            //         d.Tgt.Swift |= actualHealed >= 20;
            //     }),
            new("不动明王咒", "无法二动/三动", BuffStackRule.Wasted, false, false),
            new("玄武吐息法", "治疗可以穿上限", BuffStackRule.Add, true, true,
                eventCaptures: new StageEventCapture[]
                {
                    new("WillHeal", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        HealDetails d = (HealDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            d.Penetrate = true;
                    }),
                }),
            new("格挡", "受到攻击：攻击力-[层数]", BuffStackRule.Add, true, true,
                eventCaptures: new StageEventCapture[]
                {
                    new("WillAttack", async (listener, stageEventDetails) =>
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
                eventCaptures: new StageEventCapture[]
                {
                    new("StartRound", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner == d.Owner)
                            await b.Owner.BuffSelfProcedure("格挡", b.Stack);
                    }),
                }),
            new("缠绕", "无法二动/三动\n每回合：层数-1", BuffStackRule.Add, false, true,
                eventCaptures: new StageEventCapture[]
                {
                    new("EndTurn", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        b.Stack -= 1;
                    }),
                }),

            new("闪避", "受到攻击时，减少1层，忽略此次攻击", BuffStackRule.Add, true, true,
                eventCaptures: new StageEventCapture[]
                {
                    new("WillAttack", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Tgt && d.Src != d.Tgt)
                        {
                            d.Evade = true;
                            b.Stack -= 1;
                        }
                    }),
                }),
            new("自动闪避", "每轮：闪避补至[层数]", BuffStackRule.Add, true, true,
                eventCaptures: new StageEventCapture[]
                {
                    new("StartRound", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner == d.Owner)
                            await b.Owner.BuffSelfProcedure("闪避", b.Stack - b.Owner.GetStackOfBuff("闪避"));
                    }),
                }),
            new("穿透", "下一次攻击时，忽略对方护甲/闪避/格挡", BuffStackRule.Add, true, true,
                eventCaptures: new StageEventCapture[]
                {
                    new("WillAttack", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                        {
                            d.Pierce = true;
                            b.Stack -= 1;
                        }
                    }),
                }),
            new("力量", "攻击时，多[层数]攻", BuffStackRule.Add, true, true,
                eventCaptures: new StageEventCapture[]
                {
                    new("WillAttack", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                            d.Value += b.Stack;
                    }),
                }),
            new("回马枪", "下次受攻击后：[层数]攻 穿透", BuffStackRule.Max, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("DidAttack", async (listener, stageEventDetails) =>
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
                eventCaptures: new StageEventCapture[]
                {
                    new("StartTurn", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await d.Owner.AttackProcedure(b.Stack, wuXing: WuXing.Huo);
                    }),
                }),
            new("业火", "消耗牌时：使用2次", BuffStackRule.Wasted, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("DidExhaust", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ExhaustDetails d = (ExhaustDetails)stageEventDetails;
                        if (b.Owner == d.Owner)
                            await d.Skill.Execute(d.Owner);
                    }),
                }),
            new("淬体", "消耗生命时：灼烧+[层数]", BuffStackRule.Add, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("WillDamage", async (listener, stageEventDetails) =>
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
                eventCaptures: new StageEventCapture[]
                {
                    new("StartStep", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StepDetails d = (StepDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        if (d.Skill.GetSkillType().Contains(SkillTypeCollection.Attack))
                            return;

                        await d.Skill.ExhaustProcedure();
                        bool noBuff = b.Owner.GetStackOfBuff("免费") == 0;
                        if (noBuff)
                            await b.Owner.BuffSelfProcedure("免费");

                        b.Stack -= 1;
                    }),
                }),

            new("心斋", "所有耗蓝-[层数]", BuffStackRule.Add, true, false),

            new("盛开", "收到治疗时：力量+[层数]", BuffStackRule.Add, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("DidHeal", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        HealDetails d = (HealDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            await b.Owner.BuffSelfProcedure("力量", b.Stack);
                    }),
                }),

            new("通透世界", "攻击具有穿透", BuffStackRule.Wasted, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("WillAttack", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        AttackDetails d = (AttackDetails)stageEventDetails;
                        if (b.Owner == d.Src && d.Src != d.Tgt)
                            d.Pierce = true;
                    }),
                }),
            new("鹤回翔", "反转出牌顺序", BuffStackRule.Wasted, true, false),

            new("待激活的凤凰涅槃", "累计20灼烧后激活", BuffStackRule.Wasted, true, false,
                buffed: new Tuple<int, Func<Buff, BuffDetails, Task<BuffDetails>>>(0, async (buff, d) =>
                {
                    if (buff.Owner.GainedBurningRecord >= 20 && buff.Owner.GetStackOfBuff("涅槃") == 0)
                        await buff.Owner.BuffSelfProcedure("涅槃");
                    return d;
                })),
            new("涅槃", "每轮以及强制结算前：生命恢复至上限", BuffStackRule.Wasted, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("EndStage", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        StageDetails d = (StageDetails)stageEventDetails;

                        if (b.Owner != d.Owner) return;
                        await b.Owner.HealProcedure(b.Owner.MaxHp - b.Owner.Hp);
                    }),
                    new("StartRound", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        RoundDetails d = (RoundDetails)stageEventDetails;

                        if (b.Owner == d.Owner)
                            await b.Owner.HealProcedure(b.Owner.MaxHp - b.Owner.Hp);
                    }),
                }),
            new("抱元守一", "每回合：消耗[层数]生命，护甲+[层数]", BuffStackRule.Wasted, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("StartTurn", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await b.Owner.DamageSelfProcedure(b.Stack);
                        await b.Owner.ArmorGainSelfProcedure(b.Stack);
                    }),
                }),

            new("灼烧", "受到敌方攻击后：造成[层数]伤害", BuffStackRule.Add, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("DidAttack", async (listener, stageEventDetails) =>
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
                eventCaptures: new StageEventCapture[]
                {
                    new("EndTurn", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        TurnDetails d = (TurnDetails)stageEventDetails;
                        if (b.Owner != d.Owner) return;
                        await d.Owner.ArmorGainSelfProcedure(b.Stack);
                    }),
                }),
            new("少阳", "获得护甲：额外+[层数]", BuffStackRule.Add, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("ArmorWillGain", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ArmorGainDetails d = (ArmorGainDetails)stageEventDetails;
                        if (b.Owner == d.Tgt)
                            d.Value += b.Stack;
                    }),
                }),
            new("少阴", "施加减甲：额外+[层数]", BuffStackRule.Add, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("ArmorWillLose", async (listener, stageEventDetails) =>
                    {
                        Buff b = (Buff)listener;
                        ArmorLoseDetails d = (ArmorLoseDetails)stageEventDetails;
                        if (b.Owner == d.Src && b.Owner != d.Tgt)
                            d.Value += b.Stack;
                    }),
                }),
            new("永久暴击", "攻击附带暴击", BuffStackRule.Wasted, true, false),
            new("天人合一", "激活所有架势", BuffStackRule.Wasted, true, false),

            new("看破", "无效化敌人下一次攻击，并且反击", BuffStackRule.Add, true, false,
                eventCaptures: new StageEventCapture[]
                {
                    new("WillAttack", async (listener, stageEventDetails) =>
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
