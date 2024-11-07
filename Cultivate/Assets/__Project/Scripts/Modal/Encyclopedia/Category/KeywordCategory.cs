
using System.Collections.Generic;
using CLLibrary;

public class KeywordCategory : Category<KeywordEntry>
{
    public KeywordCategory()
    {
        AddRange(new List<KeywordEntry>()
        {
            new("金流转", "将所有坚毅转化成锋锐"),
            new("水流转", "将所有锋锐转化成格挡"),
            new("木流转", "将所有格挡转化成力量"),
            new("火流转", "将所有力量转化成灼烧"),
            new("土流转", "将所有灼烧转化成坚毅"),
            new("护甲", "下次受攻击时，攻击将被抵挡一部分"),
            new("破甲", "下次受攻击时，额外受到的攻击，会与护甲抵消"),
            new("成长", "每使用一次这张牌，效果便会激活一次"),
            new("奇偶", "此牌位置是奇数时触发前一个效果，是偶数时触发后一个效果"),
            new("架势", "消耗架势激活效果，没有架势时获得架势"),
            new("暴击", "造成伤害时，对方受到的伤害翻倍"),
            new("初次", "第一次使用时触发效果"),
            new("击伤", "攻击造成伤害时触发效果"),
            new("终结", "放置在最后一张时触发效果"),
            new("二动", "可以额外行动一次，一回合只能触发一次二动/三动"),
            new("三动", "可以额外行动两次，一回合只能触发一次二动/三动"),
            new("疲劳", "将这张牌移出本场战斗，结束后归还"),
            new("枯竭", "对战结束后，将此卡永久移出玩家卡组"),
            new("每轮", "每轮触发一次效果"),
            new("每回合", "每回合触发一次效果"),
            new("吟唱", "需要准备x回合才可以使用"),
            new("燃命", "受到来源自身的伤害时触发效果"),
            new("残血", "气血不到上限的25%"),
            new("开局", "在战斗开始前，一张牌都未打出时，触发效果"),
        });
    }

    public void Init()
    {
        List.Do(entry => entry.GenerateAnnotations());
    }
}
