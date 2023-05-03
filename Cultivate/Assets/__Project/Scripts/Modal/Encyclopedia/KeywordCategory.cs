using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class KeywordCategory : Category<KeywordEntry>
{
    public KeywordCategory()
    {
        List = new()
        {
            new("暴击", "造成伤害时，对方受到的伤害翻倍"),
            new("初次", "第一次使用时触发效果"),
            new("击伤", "攻击造成伤害时触发效果"),
            new("终结", "放置在最后一张时触发效果"),
            new("二动", "角色可以再行动一次，每回合只能触发一次，此回合无法触发三动"),
            new("三动", "角色可以再行动两次，每回合只能触发一次，此回合无法触发二动"),
            new("充沛", "再消耗1次灵气已触发效果"),
            new("移除", "将此卡移出本场战斗"),
            new("一次性", "本局对战后，将此卡移出玩家手牌"),
            new("每轮", "每轮触发一次效果"),
            new("每回合", "每回合触发一次效果"),
        };
    }

    public void Init()
    {
        List.Do(entry => entry.Generate());
    }
}
