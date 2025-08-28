
using System.Collections.Generic;
using CLLibrary;

public class TagCategory : Category<TagEntry>
{
    public TagCategory()
    {
        AddRange(new List<TagEntry>()
        {
            new("Tag0001", "金",      0, 1 << 0, "金属性卡牌，擅长制造破甲，之后用暴击造成大量伤害"),
            new("Tag0002", "水",      1, 1 << 1, "水属性卡牌，擅长驱使灵气，技能有着幻术般的效果"),
            new("Tag0003", "木",      2, 1 << 2, "木属性卡牌，擅长使用治疗延长对局，搭配成长效果在后期威力越来越强"),
            new("Tag0004", "火",      3, 1 << 3, "火属性卡牌，擅长造成多段伤害，搭配力量之后可以造成恐怖的伤害"),
            new("Tag0005", "土",      4, 1 << 4, "土属性卡牌，擅长提供护甲，然后稳步造成伤害"),
            new("Tag0006", "无色",    5, 1 << 5, "无属性卡牌，不受属性限制，具有通用性和灵活性，可以提供护甲，也可以造成伤害"),
            new("Tag0007", "攻击",    6, 1 << 6, "攻击类卡牌，可以对敌方造成伤害"),
            new("Tag0008", "防御",    7, 1 << 7, "防御类卡牌，提供防御，使对局变长以迎来优势"),
            new("Tag0009", "灵气",    8, 1 << 8, "灵气类卡牌，可以提供灵气，强大的法术往往需要消耗大量灵气"),
            new("Tag0010", "气血",    9, 1 << 9, "气血类卡牌，治疗损失的气血"),
            new("Tag0011", "二动",    10, 1 << 10, "二动类卡牌，在一回合中可以行动两次，一回合只能触发一次"),
            new("Tag0012", "自指",    11, 1 << 11, "自指类卡牌，效果具有成长性，随着使用次数增加，效果会越来越强"),
            new("Tag0013", "升华",    12, 1 << 12, "升华类卡牌，使用一次时候在对局中移出，对局结束后返还"),
            new("Tag0014", "一次性",  13, 1 << 13, "一次性卡牌，在对局中可使用多次，但是对局结束后消失，徐福可以将第一张一次性卡牌的一次性免除"),
        });
    }
    
    public static TagEntry Jin      => Encyclopedia.TagCategory.List[0];
    public static TagEntry Shui     => Encyclopedia.TagCategory.List[1];
    public static TagEntry Mu       => Encyclopedia.TagCategory.List[2];
    public static TagEntry Huo      => Encyclopedia.TagCategory.List[3];
    public static TagEntry Tu       => Encyclopedia.TagCategory.List[4];
    public static TagEntry Wu       => Encyclopedia.TagCategory.List[5];
    public static TagEntry Attack   => Encyclopedia.TagCategory.List[6];
    public static TagEntry Defend   => Encyclopedia.TagCategory.List[7];
    public static TagEntry Mana     => Encyclopedia.TagCategory.List[8];
    public static TagEntry Health   => Encyclopedia.TagCategory.List[9];
    public static TagEntry Swift    => Encyclopedia.TagCategory.List[10];
    public static TagEntry ZiZhi    => Encyclopedia.TagCategory.List[11];
    public static TagEntry Exhaust  => Encyclopedia.TagCategory.List[12];
    public static TagEntry Deplete  => Encyclopedia.TagCategory.List[13];

    public static int Length => Encyclopedia.TagCategory.Count();
}
