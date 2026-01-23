
using System.Collections.Generic;

public class EntityCategory : Category<EntityEntry>
{
    public EntityCategory()
    {
        AddRange(new List<EntityEntry>()
        {
            new("Entity0001", "鶸", "除了聚气什么都不会的废物"),
            new("Entity0002", "噬金甲", "普通金系"),
            new("Entity0003", "墨蛟", "普通水系"),
            new("Entity0004", "渊虾", "普通木系"),
            new("Entity0005", "九尾", "普通火系"),
            new("Entity0006", "推山兽", "普通土系"),
            new("Entity0007", "白泽", "精英金系"),
            new("Entity0008", "鲲", "精英水系"),
            new("Entity0009", "毕方", "精英木系"),
            new("Entity0010", "火蟾", "精英火系"),
            new("Entity0011", "麒麟", "精英土系"),
            new("Entity0012", "醉良", "Boss金系"),
            new("Entity0013", "童游", "Boss水系"),
            new("Entity0014", "眠谷", "Boss木系"),
            new("Entity0015", "常夏", "Boss火系"),
            new("Entity0016", "司方", "Boss土系"),
            new("Entity0017", "置入教学怪物", "置入教学怪物"),
            new("Entity0018", "灵气教学怪物", "灵气教学怪物"),
            new("Entity0019", "合成教学怪物", "合成教学怪物"),
            new("Entity0020", "战败教学怪物", "战败教学怪物"),
            new("Entity0021", "教学怪物1", "教学怪物1", modelName: "噬金甲"),
            new("Entity0022", "教学怪物2", "教学怪物2", modelName: "噬金甲"),
            new("Entity0023", "教学怪物3", "教学怪物3", modelName: "噬金甲"),
            new("Entity0024", "教学怪物4", "神秘人", modelName: "眠谷"),
            new("Entity0025", "教学怪物5", "神秘人", modelName: "眠谷"),
            new("Entity0026", "教学怪物6", "教学怪物6", modelName: "噬金甲"),
            new("Entity0027", "教学怪物7", "教学怪物7", modelName: "噬金甲"),
            new("Entity0028", "教学怪物8", "教学怪物8", modelName: "噬金甲"),
            new("Entity0029", "教学怪物9", "教学怪物9", modelName: "噬金甲"),
            new("Entity0030", "教学怪物10", "教学怪物10", modelName: "眠谷"),
            new("Entity0031", "教学怪物11", "教学怪物11", modelName: "眠谷"),
            new("Entity0032", "教学怪物12", "教学怪物12", modelName: "渊虾"),
            new("Entity0033", "教学怪物13", "教学怪物13", modelName: "渊虾"),
            new("Entity0034", "玩家手牌1", "玩家手牌1"),
            new("Entity0035", "玩家手牌2", "玩家手牌2"),
            new("Entity0036", "玩家手牌3", "玩家手牌3"),
            new("Entity0037", "玩家手牌4", "玩家手牌4"),
            new("Entity0038", "玩家手牌5", "玩家手牌5"),
            new("Entity0039", "玩家手牌6", "玩家手牌6"),
            new("Entity0040", "玩家手牌7", "玩家手牌7"),
            new("Entity0041", "玩家手牌8", "玩家手牌8"),
            new("Entity0042", "玩家手牌9", "玩家手牌9"),
            new("Entity0043", "玩家手牌10", "玩家手牌10"),
            new("Entity0044", "玩家手牌11", "玩家手牌11"),
            new("Entity0045", "玩家手牌12", "玩家手牌12"),
            new("Entity0046", "多段HomeA", "多段HomeA", modelName: "风雨晴"),
            new("Entity0047", "多段AwayA", "多段AwayA", modelName: "风雨晴"),
            new("Entity0048", "多段HomeB", "多段HomeB", modelName: "风雨晴"),
            new("Entity0049", "多段AwayB", "多段AwayB", modelName: "风雨晴"),
            new("Entity0050", "多段HomeC", "多段HomeC", modelName: "风雨晴"),
            new("Entity0051", "多段AwayC", "多段AwayC", modelName: "风雨晴"),
            new("Entity0052", "多段HomeD", "多段HomeD", modelName: "风雨晴"),
            new("Entity0053", "多段AwayD", "多段AwayD", modelName: "风雨晴"),
            new("Entity0054", "排局1", "排局1", modelName: "噬金甲"),
            new("Entity0055", "排局2", "排局2", modelName: "噬金甲"),
            new("Entity0056", "排局3", "排局3", modelName: "噬金甲"),
        });
    }

    public EntityEntry Default() => FromName("鶸");
}
