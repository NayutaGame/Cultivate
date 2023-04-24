using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class BuffCategory : Category<BuffEntry>
{
    public BuffCategory()
    {
        List = new()
        {
            new ("灵气", "可以消耗灵气使用技能", BuffStackRule.Add, true, false),
            new ("跳回合", "跳过回合", BuffStackRule.Add, false, false),
            new ("跳卡牌", "行动时跳过下账卡牌", BuffStackRule.Add, false, false),
            new ("双发", "下一张牌使用两次", BuffStackRule.Wasted, true, false),
            new ("二动", "下一张牌二动", BuffStackRule.Add, true, false,
                startTurn: (buff, d) =>
                {
                    d.Owner.Swift = true;
                    buff.Stack -= 1;
                }),
            new ("免费", "下一次无需灵气", BuffStackRule.Add, true, false),

            new ("延迟护甲", "下回合护甲+[层数]", BuffStackRule.Add, true, false,
                startTurn: (buff, entity) =>
                {
                    buff.Owner.ArmorGainSelfProcedure(buff.Stack);
                    buff.Owner.RemoveBuff(buff);
                }),

            new ("临金", "临金", BuffStackRule.Add, true, false),
            new ("临水", "临水", BuffStackRule.Add, true, false),
            new ("临木", "临木", BuffStackRule.Add, true, false),
            new ("临火", "临火", BuffStackRule.Add, true, false),
            new ("临土", "临土", BuffStackRule.Add, true, false),

            new ("无常已至", "造成伤害：施加[伤害值，最多Stack]减甲", BuffStackRule.Add, true, false,
                damage: (buff, d) =>
                {
                    if (buff.Owner == d.Tgt)
                        return;

                    buff.Owner.ArmorLoseOppoProcedure(Mathf.Min(d.Value, buff.Stack));
                }),

            new ("锋锐", "每回合，奇偶：[层数]攻/护甲+[层数]\n受到伤害后层数-1", BuffStackRule.Add, true, true,
                startTurn: (buff, d) =>
                {
                    bool both = buff.Owner.GetStackOfBuff("森罗万象") > 0;
                    if (d.SlotIndex % 2 == 0 || both)
                        buff.Owner.AttackProcedure(buff.Stack);
                    if (d.SlotIndex % 2 == 1 || both)
                        buff.Owner.ArmorGainSelfProcedure(buff.Stack);
                },
                damaged: (buff, d) => buff.Stack -= 1),
            new ("森罗万象", "奇偶同时激活两个效果", BuffStackRule.Wasted, true, false),

            new ("自动灵气", "每回合：灵气+[层数]", BuffStackRule.Add, true, true,
                startTurn: (buff, d) =>
                {
                    buff.Owner.BuffSelfProcedure("灵气", buff.Stack);
                }),
            new ("敛息", "造成伤害时：取消伤害，施加减甲", BuffStackRule.Add, true, false,
                damage: (buff, d) =>
                {
                    d.Cancel = true;
                    buff.Owner.ArmorLoseOppoProcedure(d.Value);
                    buff.Stack -= 1;
                }),

            new ("吸血", "下一次攻击具有吸血", BuffStackRule.Add, true, true,
                attack: (buff, d) =>
                {
                    d.LifeSteal = true;
                    buff.Stack -= 1;
                }),
            new ("凝神", "下一次受到治疗：护甲+治疗量", BuffStackRule.Add, true, true,
                healed: (buff, d) =>
                {
                    buff.Owner.ArmorGainSelfProcedure(d.Value);
                    buff.Stack -= 1;
                }),
            new ("永久吸血", "攻击一直具有吸血，直到使用非攻击牌", BuffStackRule.Wasted, true, false,
                attack: (buff, d) => d.LifeSteal = true,
                startStep: (buff, d) =>
                {
                    if (!d.WaiGong.GetWaiGongType().Contains(WaiGongType.Attack))
                        buff.Stack -= 1;
                }),
            // new ("治疗转二动", "本场战斗中，被治疗时，如果实际治疗>=20，二动", BuffStackRule.Wasted, true, false,
            //     healed: (buff, d) =>
            //     {
            //         int actualHealed = Mathf.Min(d.Tgt.MaxHp - d.Tgt.Hp, d.Value);
            //         d.Tgt.Swift |= actualHealed >= 20;
            //     }),
            new ("玄武吐息法", "治疗可以穿上限", BuffStackRule.Add, true, true,
                healed: (buff, d) =>
                {
                    d.Penetrate = true;
                }),
            new ("格挡", "受到攻击：攻击力-1", BuffStackRule.Add, true, true,
                attacked: (buff, d) =>
                {
                    if (d.Pierce)
                        return;

                    d.Value -= buff.Stack;
                }),
            new ("自动格挡", "每回合：格挡+[层数]", BuffStackRule.Add, true, true,
                startRound: (buff, owner) => owner.BuffSelfProcedure("格挡", buff.Stack)),
            // new ("强化格挡", "本场战斗中，无法治疗，每次治疗时，每有10点，格挡+1\n每1格挡可以抵挡2攻", BuffStackRule.Wasted, true, false,
            //     healed: (buff, d) =>
            //     {
            //         int actualHealed = Mathf.Min(d.Tgt.MaxHp - d.Tgt.Hp, d.Value);
            //         int value = actualHealed / 10;
            //         d.Tgt.BuffSelfProcedure("格挡", value);
            //         d.Cancel = true;
            //     }),
            // new ("缠绕", "无法二动\n每回合：层数-1", BuffStackRule.Add, false, true,
            //     endTurn: (buff, d) => buff.Stack -= 1),

            new ("闪避", "下一次受攻击时具有闪避", BuffStackRule.Add, true, true,
                attacked: (buff, d) =>
                {
                    d.Evade = true;
                    buff.Stack -= 1;
                }),
            new ("自动闪避", "每轮：闪避补至[层数]", BuffStackRule.Add, true, true,
                startRound: (buff, owner) => buff.Owner.BuffSelfProcedure("闪避", buff.Stack - owner.GetStackOfBuff("闪避"))),
            new ("穿透", "下一次攻击时具有穿透", BuffStackRule.Add, true, true,
                attack: (buff, d) =>
                {
                    d.Pierce = true;
                    buff.Stack -= 1;
                }),
            new ("力量", "攻击时，多[层数]攻", BuffStackRule.Add, true, true,
                attack: (buff, d) =>
                {
                    d.Value += buff.Stack;
                }),
            new ("回马枪", "下次受攻击时：[层数]攻 穿透", BuffStackRule.Max, true, false,
                attacked: (buff, d) =>
                {
                    if (!d.Recursive)
                        return;

                    buff.Owner.AttackProcedure(buff.Stack, recursive: false);
                    buff.Owner.RemoveBuff(buff);
                }),

            new ("天衣无缝", "每回合：[层数]攻", BuffStackRule.Max, true, false,
                startTurn: (buff, d) => d.Owner.AttackProcedure(buff.Stack)),
            new ("追击", "持续[层数]次，下次攻击时，次数+1", BuffStackRule.Add, true, true),
            new ("净天地", "使用非攻击卡不消耗灵气，使用之后消耗", BuffStackRule.Add, true, false,
                startStep: (buff, d) =>
                {
                    if (d.WaiGong.GetWaiGongType().Contains(WaiGongType.Attack))
                        return;
                    d.WaiGong.Consumed = true;
                    bool noBuff = buff.Owner.GetStackOfBuff("免费") == 0;
                    if(noBuff)
                        buff.Owner.BuffSelfProcedure("免费");
                }),

            new ("盛开", "受到治疗：力量+[层数]", BuffStackRule.Max, true, false,
                healed: (buff, d) =>
                {
                    buff.Owner.BuffSelfProcedure("力量");
                }),

            new ("通透世界", "本场战斗中：攻击具有穿透", BuffStackRule.Wasted, true, false,
                attack: (buff, d) => d.Pierce = true),
            new ("鹤回翔", "反转出牌顺序", BuffStackRule.Wasted, true, false),

            new ("待激活的凤凰涅槃", "累计20灼烧后激活", BuffStackRule.Wasted, true, false,
                buffed: new Tuple<int, Func<Buff, BuffDetails, BuffDetails>>(0, (buff, d) =>
                {
                    if(buff.Owner.GainedBurningRecord >= 20 && buff.Owner.GetStackOfBuff("涅槃") == 0)
                        buff.Owner.BuffSelfProcedure("涅槃");
                    return d;
                })),
            new ("涅槃", "每轮以及强制结算前：生命恢复至上限", BuffStackRule.Wasted, true, false,
                startRound: (buff, entity) =>
                {
                    entity.HealProcedure(entity.MaxHp - entity.Hp);
                },
                endStage: (buff, entity) =>
                {
                    entity.HealProcedure(entity.MaxHp - entity.Hp);
                }),
            new ("抱元守一", "每回合：消耗[层数]生命，护甲+[层数]", BuffStackRule.Wasted, true, false,
                startTurn: (buff, d) =>
                {
                    buff.Owner.DamageSelfProcedure(buff.Stack);
                    buff.Owner.ArmorGainSelfProcedure(buff.Stack);
                }),

            new ("灼烧", "受到敌方攻击时：造成[层数]伤害", BuffStackRule.Add, true, false,
                attacked: (buff, d) =>
                {
                    if (!d.Recursive || d.Src == buff.Owner)
                        return;

                    buff.Owner.DamageOppoProcedure(buff.Stack, recursive: false);
                }),

            new ("自动护甲", "每回合：护甲+[层数]", BuffStackRule.Add, true, false,
                startTurn: (buff, d) => d.Owner.ArmorGainSelfProcedure(buff.Stack)),
            new ("少阳", "获得护甲：额外+[层数]", BuffStackRule.Add, true, false,
                armorGained: (buff, d) => d.Value += buff.Stack),
            new ("少阴", "施加减甲：额外+[层数]", BuffStackRule.Add, true, false,
                armorLose: (buff, d) =>
                {
                    if (buff.Owner == d.Tgt)
                        return;
                    d.Value += buff.Stack;
                }),
            new ("永久暴击", "本场战斗中，攻击附带暴击", BuffStackRule.Wasted, true, false),
            new ("天人合一", "激活所有架势", BuffStackRule.Wasted, true, false),

            new ("看破", "无效化敌人下一次攻击，并且反击", BuffStackRule.Add, true, false,
                attacked: (buff, d) =>
                {
                    if (!d.Recursive || d.Src == buff.Owner)
                        return;

                    buff.Owner.AttackProcedure(d.Value, 1, d.LifeSteal, d.Pierce, d.Crit, false, d.Damaged);
                    d.Cancel = true;
                }),
        };
    }
}
