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
            /**********************************************************************************************************/
            /*********************************************** 百花缭乱 **************************************************/
            /**********************************************************************************************************/

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
                    StageManager.Instance.ArmorGainProcedure(buff.Owner, buff.Owner, buff.Stack);
                    buff.Owner.RemoveBuff(buff);
                }),

            new ("临金", "临金", BuffStackRule.Add, true, false),
            new ("临水", "临水", BuffStackRule.Add, true, false),
            new ("临木", "临木", BuffStackRule.Add, true, false),
            new ("临火", "临火", BuffStackRule.Add, true, false),
            new ("临土", "临土", BuffStackRule.Add, true, false),

            new ("自动护甲", "回合开始:护甲+[层数]", BuffStackRule.Add, true, true,
                startTurn: (buff, d) => StageManager.Instance.ArmorGainProcedure(d.Owner, d.Owner, buff.Stack)),
            new ("永久暴击", "本场战斗中，攻击附带暴击", BuffStackRule.Wasted, true, false),
            new ("天人合一", "激活所有架势", BuffStackRule.Wasted, true, false),

            new ("铃鹿御前", "造成伤害：施加【造成伤害，最多Stack】减甲", BuffStackRule.Add, true, false,
                damage: (buff, d) =>
                {
                    if (buff.Owner == d.Tgt)
                        return;
                    StageManager.Instance.ArmorLoseProcedure(d.Tgt, Mathf.Min(d.Value, buff.Stack));
                }),

            new ("水势", "奇数/偶数回合：[层数]攻/治疗\n受到伤害后层数-1", BuffStackRule.Add, true, true,
                endTurn: (buff, d) =>
                {
                    if (d.SlotIndex % 2 == 0)
                        StageManager.Instance.AttackProcedure(d.Owner, d.Owner.Opponent(), buff.Stack);
                    else
                        StageManager.Instance.HealProcedure(d.Owner, d.Owner, buff.Stack);
                },
                damaged: (buff, d) => buff.Stack -= 1),
            new ("隐如云", "造成伤害时，取消伤害，施加减甲", BuffStackRule.Add, true, false,
                damage: (buff, d) =>
                {
                    d.Cancel = true;
                    StageManager.Instance.ArmorLoseProcedure(buff.Owner.Opponent(), d.Value);
                    buff.Stack -= 1;
                }),

            new ("吸血", "下一次攻击具有吸血", BuffStackRule.Add, true, true,
                attack: (buff, d) =>
                {
                    d.LifeSteal = true;
                    buff.Stack -= 1;
                }),
            new ("永久吸血", "攻击一直具有吸血，直到使用非攻击牌", BuffStackRule.Wasted, true, false,
                attack: (buff, d) => d.LifeSteal = true,
                startStep: (buff, d) =>
                {
                    if (!d.WaiGong.GetWaiGongType().Contains(WaiGongType.Attack))
                        buff.Stack -= 1;
                }),
            new ("治疗转二动", "本场战斗中，被治疗时，如果实际治疗>=20，二动", BuffStackRule.Wasted, true, false,
                healed: (buff, d) =>
                {
                    int actualHealed = Mathf.Min(d.Tgt.MaxHp - d.Tgt.Hp, d.Value);
                    d.Tgt.Swift |= actualHealed >= 20;
                }),
            new ("格挡", "受到攻击:攻击力-1", BuffStackRule.Add, true, true,
                attacked: (buff, d) =>
                {
                    bool flag = buff.Owner.GetStackOfBuff("强化格挡") > 0;
                    if (!flag)
                        d.Value -= buff.Stack;
                    else
                        d.Value -= buff.Stack * 2;
                }),
            new ("自动格挡", "Round开始:格挡+[层数]", BuffStackRule.Add, true, true,
                startRound: (buff, owner) => StageManager.Instance.BuffProcedure(owner, owner, "格挡", buff.Stack)),
            new ("强化格挡", "本场战斗中，无法治疗，每次治疗时，每有10点，格挡+1\n每1格挡可以抵挡2攻", BuffStackRule.Wasted, true, false,
                healed: (buff, d) =>
                {
                    int actualHealed = Mathf.Min(d.Tgt.MaxHp - d.Tgt.Hp, d.Value);
                    int value = actualHealed / 10;
                    StageManager.Instance.BuffProcedure(d.Tgt, d.Tgt, "格挡", value);
                    d.Cancel = true;
                }),
            new ("缠绕", "无法二动\n每回合：层数-1", BuffStackRule.Add, false, true,
                startTurn: (buff, d) => buff.Stack -= 1),

            new ("闪避", "下一次受攻击时具有闪避", BuffStackRule.Add, true, true,
                attacked: (buff, d) =>
                {
                    d.Evade = true;
                    buff.Stack -= 1;
                }),
            new ("自动闪避", "每轮：闪避补至[层数]", BuffStackRule.Add, true, true,
                startRound: (buff, owner) => StageManager.Instance.BuffProcedure(buff.Owner, buff.Owner, "闪避", buff.Stack - owner.GetStackOfBuff("闪避"))),
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

            new ("不屈", "无法二动，死亡时:直到下个自己回合开始前，生命值最少为1，堆叠规则为取最大", BuffStackRule.Max, true, false),
            new ("激活的不屈", "直到下个自己回合开始前，生命值最少为1", BuffStackRule.Wasted, true, false),
            new ("强化不屈", "本场战斗中，对手回合中，自己的不屈被触发时，对手跳过下一回合", BuffStackRule.Wasted, true, false),
            new ("天衣无缝", "每回合：[层数]攻", BuffStackRule.Max, true, false,
                startTurn: (buff, d) => StageManager.Instance.AttackProcedure(d.Owner, d.Owner.Opponent(), buff.Stack)),
            new ("追击", "持续[层数]次，下次攻击时，次数+1", BuffStackRule.Add, true, true),

            new ("火22", "本场战斗中，每次受到不少于10点伤害的时候，力量+1", BuffStackRule.Wasted, true, false,
                damaged: (buff, d) =>
                {
                    if (d.Value >= 10)
                        StageManager.Instance.BuffProcedure(buff.Owner, buff.Owner, "力量");
                }),

            new ("待激活的涅槃", "累计20闪避后激活", BuffStackRule.Add, true, false,
                buffed: new Tuple<int, Func<Buff, BuffDetails, BuffDetails>>(0, (buff, d) =>
                {
                    if (d._buffEntry.Name == "闪避")
                        buff.Stack += d._stack;
                    if (buff.Stack >= 20)
                    {
                        StageManager.Instance.BuffProcedure(buff.Owner, buff.Owner, "涅槃");
                        buff.Owner.RemoveBuff(buff);
                    }
                    return d;
                })),
            new ("涅槃", "每轮：生命恢复至上限", BuffStackRule.Wasted, true, false,
                startRound: (buff, entity) =>
                {
                    StageManager.Instance.HealProcedure(entity, entity, entity.MaxHp - entity.Hp);
                }),
            new ("夜凯", "每回合：消耗10生命，力量+1", BuffStackRule.Wasted, true, false,
                startTurn: (buff, d) =>
                {
                    StageManager.Instance.DamageProcedure(buff.Owner, buff.Owner, 10);
                    StageManager.Instance.BuffProcedure(buff.Owner, buff.Owner, "力量", 1);
                }),

            new ("通透世界", "本场战斗中，自己的所有攻击具有穿透", BuffStackRule.Wasted, true, false,
                attack: (buff, d) => d.Pierce = true),

            new ("看破", "无效化敌人下一次攻击，并且反击", BuffStackRule.Add, true, false,
                attacked: (buff, d) =>
                {
                    if (!d.Recursive)
                        return;

                    StageManager.Instance.AttackProcedure(d.Tgt, d.Src, d.Value, 1, d.LifeSteal, d.Pierce, d.Crit, false, d.Damaged);
                    d.Cancel = true;
                }),

            /**********************************************************************************************************/
            /*********************************************** 大剑哥 ****************************************************/
            /**********************************************************************************************************/

            /**********************************************************************************************************/
            /*********************************************** 石丸 ******************************************************/
            /**********************************************************************************************************/

            /**********************************************************************************************************/
            /*********************************************** Summer68 *************************************************/
            /**********************************************************************************************************/

            /**********************************************************************************************************/
            /*********************************************** End ******************************************************/
            /**********************************************************************************************************/
        };
    }
}
