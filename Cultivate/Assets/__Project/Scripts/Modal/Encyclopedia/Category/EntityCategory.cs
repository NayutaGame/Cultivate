
using System.Collections.Generic;

public class EntityCategory : Category<EntityEntry>
{
    public EntityCategory()
    {
        AddRange(new List<EntityEntry>()
        {
            new("鶸", "除了聚气什么都不会的废物"),
            new("噬金甲", "普通金系减甲"),
            new("墨蛟", "普通水系吸血"),
            new("渊虾", "普通木系穿透"),
            new("九尾狐", "普通火系多段"),
            new("推山兽", "普通土系高伤"),
            new("白泽", "精英金系锋锐"),
            new("鲲", "精英水系格挡"),
            new("毕方", "精英木系闪避"),
            new("火蟾", "精英火系灼烧"),
            new("麒麟", "精英土系加甲"),
            new("凌霄大圣", "Boss金系"),
            new("龟仙人", "Boss水系"),
            new("鹤仙人", "Boss木系"),
            new("鹿仙人", "Boss火系"),
            new("土行大圣", "Boss土系"),
            new("置入教学怪物", "置入教学怪物"),
            new("灵气教学怪物", "灵气教学怪物"),
            new("合成教学怪物", "合成教学怪物"),
            new("战败教学怪物", "战败教学怪物"),
            new("教学怪物1", "教学怪物1", modelName: "噬金甲"),
            new("教学怪物2", "教学怪物2", modelName: "噬金甲"),
            new("教学怪物3", "教学怪物3", modelName: "噬金甲"),
            new("教学怪物4", "教学怪物4", modelName: "噬金甲"),
            new("教学怪物5", "教学怪物5", modelName: "噬金甲"),
            new("教学怪物6", "教学怪物6", modelName: "噬金甲"),
            new("教学怪物7", "教学怪物7", modelName: "噬金甲"),
            new("教学怪物8", "教学怪物8", modelName: "噬金甲"),
            new("教学怪物9", "教学怪物9", modelName: "噬金甲"),
            new("教学怪物10", "教学怪物10", modelName: "梦乃遥"),
            new("教学怪物11", "教学怪物11", modelName: "梦乃遥"),
            new("教学怪物12", "教学怪物12", modelName: "渊虾"),
            new("教学怪物13", "教学怪物13", modelName: "渊虾"),
            new("玩家手牌1", "玩家手牌1"),
            new("玩家手牌2", "玩家手牌2"),
            new("玩家手牌3", "玩家手牌3"),
            new("玩家手牌4", "玩家手牌4"),
            new("玩家手牌5", "玩家手牌5"),
            new("玩家手牌6", "玩家手牌6"),
            new("玩家手牌7", "玩家手牌7"),
            new("玩家手牌8", "玩家手牌8"),
            new("玩家手牌9", "玩家手牌9"),
            new("玩家手牌10", "玩家手牌10"),
            new("玩家手牌11", "玩家手牌11"),
            new("玩家手牌12", "玩家手牌12"),
        });
    }

    public override EntityEntry DefaultEntry() => this["鶸"];
}
