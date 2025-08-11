
using System.Collections.Generic;
using CLLibrary;

public class TagCategory : Category<TagEntry>
{
    public TagCategory()
    {
        AddRange(new List<TagEntry>()
        {
            new("Tag0001", "金",             0, 1 << 0, ""),
            new("Tag0002", "水",             1, 1 << 1, ""),
            new("Tag0003", "木",             2, 1 << 2, ""),
            new("Tag0004", "火",             3, 1 << 3, ""),
            new("Tag0005", "土",             4, 1 << 4, ""),
            new("Tag0006", "无色",           5, 1 << 5, ""),
            new("Tag0007", "攻击",           6, 1 << 6, ""),
            new("Tag0008", "防御",           7, 1 << 7, ""),
            new("Tag0009", "灵气",           8, 1 << 8, ""),
            new("Tag0010", "气血",           9, 1 << 9, ""),
            new("Tag0011", "二动",           10, 1 << 10, ""),
            new("Tag0012", "自指",           11, 1 << 11, ""),
            new("Tag0013", "升华",           12, 1 << 12, ""),
            new("Tag0014", "一次性",         13, 1 << 13, ""),
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
