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

            new ("可被驱散", "可被驱散", BuffStackRule.Add, false, true),
            new ("治疗同时加护甲", "治疗同时加护甲", BuffStackRule.Add, true, false,
                healed: (buff, d) =>
                {
                    StageManager.Instance.ArmorGainProcedure(new StringBuilder(), buff.Owner, d.Tgt, d.Value);
                }),

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
            /**********************************************************************************************************/

            /**********************************************************************************************************/
            /*********************************************** Summer68 *************************************************/
            /**********************************************************************************************************/
            new ("蓄力", "下次攻击时攻击力乘二，受到攻击后移除。", BuffStackRule.Add, true, false,
                damaged: (buff, d) =>
                {
                        buff.Stack = 0;
                },
                attack: (buff, d) =>
                {
                    while(buff.Stack > 0)
                    {
                        d.Value*=2;
                        buff.Stack-=1;
                    }
                }
                ),

            new ("磨刀", "所有牌的金相邻数+1", BuffStackRule.Add, true, false),
            new ("疲劳", "本回合无法攻击", BuffStackRule.Add, false, false,
                attack: (buff, d) =>
                {
                    d.Cancel = true;
                },
                endTurn: (buff, caster) =>
                {
                    buff.Stack -= 1;
                }),
            new ("邪灵剑", "当敌人生命值低于10+2*【金】时直接斩杀", BuffStackRule.Add, true, false,
                startTurn: (b, owner) =>
                {
                    if(StageManager.Instance._enemy.Hp<b.Stack)
                    {
                        StageManager.Instance._enemy.Hp=0;
                    }
                },
                endTurn: (buff, caster) =>
                {
                    if(StageManager.Instance._enemy.Hp<buff.Stack)
                    {
                        StageManager.Instance._enemy.Hp=0;
                    }
                }),
            new ("丛云剑", "下次遭遇致死伤害时仍会保留1血", BuffStackRule.Add, true, false,
                damaged: (buff, d) =>
                {
                    if(StageManager.Instance._hero.Hp<d.Value)
                    {
                        d.Value=StageManager.Instance._hero.Hp-1;
                        buff.Stack=0;
                        //移除卡牌丛云剑，增加卡牌破碎剑刃
                    }
                }
                ),
            new ("金刚之躯", "攻击额外提高3+1*【金】点攻击，防御额外提高3+1*【金】点防御。", BuffStackRule.Add, true, false,
                armored: (buff, d) =>
                {
                    d.Value+=buff.Stack;
                },
                attack: (buff, d) =>
                {
                    d.Value+=buff.Stack;
                }
                ),
            new ("中毒", "每回合结束后失去等量生命值", BuffStackRule.Add, false, false,

                endTurn: (buff, owner) =>
                {
                    StageManager.Instance.DamageProcedure(new StringBuilder(), owner.Opponent(), owner, buff.Stack, false);

                }),
            new ("治疗计数器", "计算总治疗值", BuffStackRule.Add, true, false

                ),
            new ("生生不息", "每回合结束后恢复3+1*【木】", BuffStackRule.Add, true, false,

                endTurn: (buff, owner) =>
                {
                    StageManager.Instance.BuffProcedure(new StringBuilder(), owner, owner, "治疗计数器",buff.Stack);
                    StageManager.Instance.HealProcedure(new StringBuilder(), owner, owner, buff.Stack);
                }),

            new ("蜜糖砒霜", "每次自身受到治疗给对方施加1中毒，相邻2木以上时施加2中毒", BuffStackRule.Add, true, false,
                    healed:(buff,d) =>
                    {
                        StageManager.Instance.BuffProcedure(new StringBuilder(), buff.Owner, buff.Owner.Opponent(), "中毒", buff.Stack);
                    }
                ),
            new ("移花接木", "每回合结束后随机获得生生不息，蜜糖砒霜或移花接木", BuffStackRule.Add, true, false,

                endTurn: (buff, owner) =>
                {
                    int rs=RandomManager.Range(0,2);
                    if(rs==0)
                    {
                        StageManager.Instance.BuffProcedure(new StringBuilder(), owner, owner, "生生不息",buff.Stack);
                    }
                    if(rs==1)
                    {
                        //StageManager.Instance.BuffProcedure(new StringBuilder(), owner, owner, "蜜糖砒霜",buff.Stack);
                    }
                    if(rs==2)
                    {
                        StageManager.Instance.BuffProcedure(new StringBuilder(), owner, owner, "移花接木",buff.Stack);
                    }
                }),

            new ("寄生", "每次获得灵力时被偷取1点灵力，1+【木】生命值", BuffStackRule.Add, false, false,
                buffed: new Tuple<int, Func<Buff, BuffDetails, BuffDetails>>(0,
                    (buff, d) =>
                    {
                        Buff same1 = d.Tgt.FindBuff(d._buffEntry);

                        if(same1.GetName()=="灵气")
                        {
                            buff.Owner.Hp-=buff.Stack;
                            StageManager.Instance.BuffProcedure(new StringBuilder(), buff.Owner.Opponent(), buff.Owner.Opponent(), "治疗计数器",buff.Stack);
                            StageManager.Instance.HealProcedure(new StringBuilder(), buff.Owner, buff.Owner.Opponent(), buff.Stack);
                            same1.Stack-=1;
                            StageManager.Instance.BuffProcedure(new StringBuilder(),buff.Owner, buff.Owner.Opponent(), "灵气",1);
                        }
                        return d;
                    })
                ),
            /***new ("预兆之水", "会根据敌方的行为来驱散或防御", BuffStackRule.Add, false,
                buffed: new Tuple<int, Func<Buff, BuffDetails, BuffDetails>>(0,
                    (buff, d) =>
                    {
                        Buff same1 = d.Tgt.FindBuff(d._buffEntry);

                        if(d._buffEntry.Friendly )
                            d.Cancel = true;
                        return d;
                    }),
                attack: (buff, d) =>
                {
                    d.Value-=buff.Stack;
                    if(d.Value<0)
                        d.Value =0;
                    buff.Stack=0;
                }//
                ), ***/
            new ("水之守护", "受到攻击时获得当时灵力值的防御", BuffStackRule.Add, true, false,
                    attacked: (buff, d) =>
                {
                    Buff lq = d.Tgt.FindBuff("灵气");
                    StageManager.Instance.ArmorGainProcedure(new StringBuilder(), d.Tgt, d.Tgt, lq.Stack);
                    buff.Stack = 0;
                }
                ),
            new ("水之守护", "受到攻击时获得当时灵力值的防御", BuffStackRule.Add, true, false,
                    attacked: (buff, d) =>
                {
                    Buff lq = d.Tgt.FindBuff("灵气");
                    StageManager.Instance.ArmorGainProcedure(new StringBuilder(), d.Tgt, d.Tgt, 2*lq.Stack);
                    buff.Stack -=1;
                }
                ),
            new ("力量", "攻击时攻击提高", BuffStackRule.Add, true, false,
                attack: (buff, d) =>
                {
                    d.Value+=buff.Stack;
                }
                ),

            new ("混乱", "攻击防御时数值随机减少", BuffStackRule.Add, false, false,
                attack: (buff, d) =>
                {
                    d.Value=RandomManager.Range(0,d.Value);
                },
                armored: (buff, d) =>
                {
                    d.Value=RandomManager.Range(0,d.Value);
                }
                ),
            new ("浩瀚无穷", "每回合开始获得2灵力", BuffStackRule.Add, true, false,

                startTurn: (buff, owner) =>
                {
                    StageManager.Instance.BuffProcedure(new StringBuilder(), owner, owner, "灵力",2);
                }),


            new ("献祭计数器", "计算总献祭值", BuffStackRule.Add, true, false

                ),

            new ("灵魂燃烧", "无法获得灵力", BuffStackRule.Add, false, false,
                buffed: new Tuple<int, Func<Buff, BuffDetails, BuffDetails>>(0,
                    (buff, d) =>
                    {
                        Buff same1 = d.Tgt.FindBuff(d._buffEntry);

                        if(same1.GetName()=="灵气")
                        {
                            d.Cancel = true ;
                        }
                        return d;
                    })
                ),
            new("浴火凤凰","死亡后复活，生命值重置为献祭值，移除所有buff", BuffStackRule.Add, true, false,
                damaged: (buff, d) =>
                {
                    if(StageManager.Instance._hero.Hp<d.Value)
                    {
                        d.Cancel=true;
                        int xianh = 0;
                        Buff xianji=d.Tgt.FindBuff("献祭计数器");
                        if(xianji!= null)
                            {
                                xianh=xianji.Stack;
                            }
                        d.Tgt.Hp=xianh;
                        d.Tgt.Buffs.RemoveAll(bf=>true);
                    }

                }
            ),
            new ("侵略如火", "获得3动，死亡倒计时", BuffStackRule.Add, true, false

                ),
            new ("撕裂大地", "你每获得30护甲，使用一次地震。", BuffStackRule.Add, true, false,
                    armored: (buff, d) =>
                {
                    buff.Stack+=d.Value;
                    while(buff.Stack>=31)
                    {
                        buff.Stack-=30;
                        Buff ddlz=d.Tgt.FindBuff("大地领主");
                        int damage=(int)((d.Tgt.Opponent().MaxHp-1)*(0.1)+1);
                        if(ddlz!=null)
                        {
                            damage*=2;
                        }
                        StageManager.Instance.AttackProcedure(new StringBuilder(), d.Tgt, d.Tgt.Opponent(), damage);
                    }
                }
                ),
            new ("大地领主", "地震造成双倍伤害", BuffStackRule.Wasted, true, false
                ),

            new ("重峦叠嶂", "每回合结束后获得5+【土】防御", BuffStackRule.Add, true, false,

                endTurn: (buff, owner) =>
                {
                    StageManager.Instance.ArmorGainProcedure(new StringBuilder(), owner, owner,buff.Stack);
                }),
            /**********************************************************************************************************/
            /*********************************************** End ******************************************************/
            /**********************************************************************************************************/
        };
    }
}
