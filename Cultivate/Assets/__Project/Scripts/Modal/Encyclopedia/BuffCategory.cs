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

            new ("下回合开始三动", "下回合开始三动", BuffStackRule.Add, true, false,
                startTurn: (buff, owner) =>
                {
                    owner.UltraSwift = true;
                    buff.Stack -= 1;
                }),




            // new ("免攻", "可以免疫下一次伤害", BuffStackRule.Add, true, false,
            //     attacked: (buff, d) =>
            //     {
            //         d.Cancel = true;
            //         buff.Stack -= 1;
            //     }),
            // new ("无敌", "免疫伤害，Debuff，每回合Stack-1", BuffStackRule.Add, true, false,
            //     startTurn: (b, owner) =>
            //     {
            //         b.Stack -= 1;
            //     },
            //     damaged: (buff, d) =>
            //     {
            //         d.Cancel = true;
            //     },
            //     buffed: new Tuple<int, Func<Buff, BuffDetails, BuffDetails>>(0,
            //         (buff, d) =>
            //         {
            //             if(!d._buffEntry.Friendly)
            //                 d.Cancel = true;
            //             return d;
            //         })),
            // new ("格挡", "受到攻击时，减少受攻击值", BuffStackRule.Add, true, false,
            //     attacked: (buff, d) =>
            //     {
            //         d.Value -= buff.Stack;
            //     }),
            // new ("无法攻击", "本回合无法攻击", BuffStackRule.Add, false, false,
            //     attack: (buff, d) =>
            //     {
            //         d.Cancel = true;
            //     },
            //     endTurn: (buff, caster) =>
            //     {
            //         buff.Stack -= 1;
            //     }),
            // new ("晕眩", "本回合无法行动", BuffStackRule.Add, false, false,
            //     endTurn: (buff, caster) =>
            //     {
            //         buff.Stack -= 1;
            //     }),
            // new ("不动", "本次战斗获得的护甲翻倍", BuffStackRule.Wasted, true, false,
            //     armored: (buff, d) =>
            //     {
            //         if (d.Value >= 0)
            //             d.Value *= 2;
            //     }),
            // new ("明", "本轮所有的防御卡打出两次", BuffStackRule.Add, true, false),
            // new ("觉有情", "获得20点【减伤】，回合开始护甲+20", BuffStackRule.Add, true, false),
            //
            // //
            //
            // new ("受伤反击", "下一回合受到伤害时，对敌方造成伤害", BuffStackRule.Add, true, false,
            //     damaged: (buff, d) =>
            //     {
            //         if (!d.Recursive) return;
            //         StageManager.Instance.DamageProcedure(new StringBuilder(), d.Tgt, d.Src, buff.Stack, false);
            //     },
            //     startTurn: (buff, owner) =>
            //     {
            //         owner.RemoveBuff(buff);
            //     }),

            /**********************************************************************************************************/
            /*********************************************** 大剑哥 ****************************************************/
            /**********************************************************************************************************/

            /**********************************************************************************************************/
            /*********************************************** 石丸 ******************************************************/
            /**********************************************************************************************************/            /**********************************************************************************************************/
            /*********************************************** 石丸 ******************************************************/
            /**********************************************************************************************************/

            new("强化", "下次攻击造成伤害当前层数的数值，每次触发后层数减1。", BuffStackRule.Add, true, false,
            attack: (buff, d) =>
            {
                d.Value += buff.Stack;
                buff.Stack -= 1;
            }),
            //new("力量", "造成的所有攻击增加当前层数的数值，不衰减。", BuffStackRule.Add, true, false,//已有，68
            new("毁灭", "下一张火属性攻击卡牌伤害翻倍。", BuffStackRule.Add, true, false,
                damaged: (buff, d) =>
                {
                    buff.Stack = 0;
                },
                attack: (buff, d) =>
                {
                    while (buff.Stack > 0)//&&判断这张卡是否火属性
                    {
                        d.Value *= 2;
                        buff.Stack -= 1;
                    }
                }
                ),
            new("坚韧", "受到的伤害减半，对手回合结束后层数减1。", BuffStackRule.Add, true, false,
                attacked: (buff, d) =>
                {
                    d.Value *= (1 / 2);
                },
                endTurn: (buff, caster) =>
                {
                    buff.Stack -= 1;
                }),

            /*
            new("反震", "下一次被攻击后，造成我方受到该攻击前护甲数伤害。", BuffStackRule.Add, true, false,

                attacked: (buff, d) =>
                {
                    int damege = buff.owner.Armor;//怎么记录被攻击前
                    StageManager.Instance.AttackProcedure(new StringBuilder(), buff.owner, buff.owner.Opponent, damege);
                }),
            */

            //new("停止", "本回合不行动，不跳过本回合的卡牌", BuffStackRule.Add, false, false,     //效果未做
            //    endTurn: (buff, caster) =>
            //    {
            //        buff.Stack -= 1;
            //    }),
            //new("跳过", "本回合不行动，且跳过本回合的卡牌", BuffStackRule.Add, false, false,

            new("持续伤害", "回合结束时受到一定伤害，层数减1", BuffStackRule.Add, true, false,  //伤害值动态变化未有
                endTurn: (buff, owner) =>
                {
                    StageManager.Instance.DamageProcedure(owner.Opponent(), owner, 3);
                    buff.Stack -= 1;
                }),

            new("固化","护甲不会在回合结束后消失",BuffStackRule.Add, true, false),

            new("灰烬", "万物燃烧殆尽后的产物", BuffStackRule.Add, true, false),
            new("木桩", "催生而起的坚硬木块", BuffStackRule.Add, true, false),
            new("水涡", "水分聚拢形成的漩涡", BuffStackRule.Add, true, false),
            new("利刃", "临时塑造成型的锋利飞刃", BuffStackRule.Add, true, false),
            new("砂尘", "随风扬起的黄色砂尘", BuffStackRule.Add, true, false),

            new("铁卫", "由兵器聚集而成的机关兽，每次攻击会额外造成铁卫数×3点伤害", BuffStackRule.Add, true, false,
                attack: (buff, d) =>
                {
                    StageManager.Instance.DamageProcedure(d.Tgt, d.Tgt.Opponent(), 3 * buff.Stack,false);
                }),

            new("土墙", "用于掩护的砂土墙，每回合开始时获得土墙数×3护甲", BuffStackRule.Add, true, false,
                startTurn: (buff, owner) =>
                {
                    StageManager.Instance.ArmorGainProcedure(owner, owner, 3 * buff.Stack);
                }),


            /**********************************************************************************************************/
            /*********************************************** Summer68 *************************************************/
            /**********************************************************************************************************/

            /**********************************************************************************************************/
            /*********************************************** End ******************************************************/
            /**********************************************************************************************************/
        };
    }
}
