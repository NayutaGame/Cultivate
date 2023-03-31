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
            new ("跳回合", "回合中无法行动", BuffStackRule.Add, false, false),
            new ("跳卡牌", "行动时跳过下账卡牌", BuffStackRule.Add, false, false),
            new ("双发", "下一张牌使用两次", BuffStackRule.Wasted, true, false),

            new ("临金", "临金", BuffStackRule.Add, true, false),
            new ("临水", "临水", BuffStackRule.Add, true, false),
            new ("临木", "临木", BuffStackRule.Add, true, false),
            new ("临火", "临火", BuffStackRule.Add, true, false),
            new ("临土", "临土", BuffStackRule.Add, true, false),

            new ("再生", "回合开始:回复[层数]生命, 受到攻击:层数-1", BuffStackRule.Add, true, true,
                startTurn: (buff, owner) =>
                {
                    StageManager.Instance.HealProcedure(owner, owner, buff.Stack);
                },
                attacked: (buff, d) =>
                {
                    buff.Stack -= 1;
                }),

            new ("内伤", "回合开始:失去[层数]生命, 攻击:层数-1", BuffStackRule.Add, false, true,
                startTurn: (buff, owner) =>
                {
                    StageManager.Instance.DamageProcedure(owner, owner, buff.Stack);
                },
                attack: (buff, d) =>
                {
                    buff.Stack -= 1;
                }),

            new ("格挡", "受到攻击:攻击力-1", BuffStackRule.Add, true, true,
                attacked: (buff, d) =>
                {
                    d.Value -= buff.Stack;
                }),
            new ("吸血", "下一次攻击具有吸血", BuffStackRule.Add, true, true,
                attack: (buff, d) =>
                {
                    d.LifeSteal = true;
                    buff.Stack -= 1;
                }),
            new ("闪避", "下一次受攻击时具有闪避", BuffStackRule.Add, true, true,
                attacked: (buff, d) =>
                {
                    d.Evade = true;
                    buff.Stack -= 1;
                }),
            new ("穿透", "下一次攻击时具有穿透", BuffStackRule.Add, true, true,
                attack: (buff, d) =>
                {
                    d.Pierce = true;
                    buff.Stack -= 1;
                }),
            new ("自动护甲", "回合开始:护甲+【stack】", BuffStackRule.Add, true, true),
            new ("力量", "攻击时:数值+【stack】", BuffStackRule.Add, true, true,
                attack: (buff, d) =>
                {
                    d.Value += buff.Stack;
                }),

            new ("不屈", "无法二动，死亡时:直到下个自己回合开始前，生命值最少为1，堆叠规则为取最大", BuffStackRule.Max, true, false),
            new ("激活的不屈", "直到下个自己回合开始前，生命值最少为1", BuffStackRule.Wasted, true, false),

            new ("铃鹿御前", "造成伤害：施加【造成伤害，最多Stack】减甲", BuffStackRule.Add, true, false,
                damage: (buff, d) =>
                {
                    if (buff.Owner == d.Tgt)
                        return;
                    StageManager.Instance.ArmorLoseProcedure(d.Tgt, Mathf.Min(d.Value, buff.Stack));
                }),
            new ("木21", "攻击一直具有吸血，直到使用一张非攻击牌", BuffStackRule.Wasted, true, false,
                attack: (buff, d) => d.LifeSteal = true,
                startStep: (buff, d) =>
                {
                    if (d.WaiGong.GetWaiGongType() == WaiGongEntry.WaiGongType.NONATTACK)
                        buff.Stack -= 1;
                }),
            new ("火22", "本场战斗中，每次受到不少于10点伤害的时候，力量+1", BuffStackRule.Wasted, true, false,
                damaged: (buff, d) =>
                {
                    if (d.Value >= 10)
                        StageManager.Instance.BuffProcedure(buff.Owner, buff.Owner, "力量");
                }),
            new ("天衣无缝", "本场战斗中，Step开始：【stack】攻", BuffStackRule.Max, true, false,
                startStep: (buff, d) => StageManager.Instance.AttackProcedure(d.Caster, d.Caster.Opponent(), buff.Stack)),

            new ("通透世界", "本场战斗中，自己的所有攻击具有穿透", BuffStackRule.Wasted, true, false,
                attack: (buff, d) => d.Pierce = true),

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
