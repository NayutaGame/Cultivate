using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCategory : Category<BuffEntry>
{
    public BuffCategory()
    {
        List = new()
        {
            new ("灵气", "可以消耗灵气使用技能", BuffStackRule.Add, true),
            new ("免伤", "可以免疫下一次伤害", BuffStackRule.Add, true,
                damaged: (buff, d) =>
                {
                    d.Cancel = true;
                    buff.Stack -= 1;
                }),
            new ("无敌", "免疫伤害，Debuff，每回合Stack-1", BuffStackRule.Add, true,
                startTurn: (b, owner) =>
                {
                    b.Stack -= 1;
                },
                damaged: (buff, d) =>
                {
                    d.Cancel = true;
                },
                buffed: new Tuple<int, Func<Buff, BuffDetails, BuffDetails>>(0,
                    (buff, d) =>
                    {
                        if(!d._buffEntry.Friendly)
                            d.Cancel = true;
                        return d;
                    })),
            new ("格挡", "受到攻击时，减少受攻击值", BuffStackRule.Add, true,
                attacked: (buff, d) =>
                {
                    d.Value -= buff.Stack;
                }),
            new ("无法攻击", "本回合无法攻击", BuffStackRule.Add, false,
                attack: (buff, d) =>
                {
                    d.Cancel = true;
                },
                endTurn: (buff, caster) =>
                {
                    buff.Stack -= 1;
                }),
            new ("晕眩", "本回合无法行动", BuffStackRule.Add, false,
                endTurn: (buff, caster) =>
                {
                    buff.Stack -= 1;
                }),
            new ("不动", "本次战斗获得的护甲翻倍", BuffStackRule.Wasted, true,
                armored: (buff, d) =>
                {
                    if (d.Value >= 0)
                        d.Value *= 2;
                }),
            new ("明", "本轮所有的防御卡打出两次", BuffStackRule.Add, true),
            new ("觉有情", "获得20点【减伤】，回合开始护甲+20", BuffStackRule.Add, true),
        };
    }
}
