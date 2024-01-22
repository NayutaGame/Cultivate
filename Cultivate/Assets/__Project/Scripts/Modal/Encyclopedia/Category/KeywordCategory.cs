
using System.Collections.Generic;
using CLLibrary;

public class KeywordCategory : Category<KeywordEntry>
{
    public KeywordCategory()
    {
        AddRange(new List<KeywordEntry>()
        {
            new("奇偶", "此牌位置是奇数时触发前一个效果，是偶数时触发后一个效果"),
            new("架势", "需要激活架势的牌以触发特殊效果，无法使用集中"),
            new("暴击", "造成伤害时，对方受到的伤害翻倍"),
            new("初次", "第一次使用时触发效果"),
            new("击伤", "攻击造成伤害时触发效果"),
            new("终结", "放置在最后一张时触发效果"),
            new("二动", "可以额外行动一次，一回合只能触发一次二动/三动"),
            new("三动", "可以额外行动两次，一回合只能触发一次二动/三动"),
            new("充沛", "再消耗1次灵气已触发效果"),
            new("移除", "将此卡移出本场战斗"),
            new("一次性", "本局对战后，将此卡移出玩家手牌"),
            new("每轮", "每轮触发一次效果"),
            new("每回合", "每回合触发一次效果"),
            new("吟唱", "需要准备x回合才可以使用"),
        });
    }

    public void Init()
    {
        List.Do(entry => entry.Generate());
    }
}
