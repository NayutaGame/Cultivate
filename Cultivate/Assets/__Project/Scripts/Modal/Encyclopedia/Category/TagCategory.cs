
using System.Collections.Generic;
using CLLibrary;

public class TagCategory : Category<TagEntry>
{
    public TagCategory()
    {
        AddRange(new List<TagEntry>()
        {
            new("金", 0, 1 << 0, ""),
            new("水", 1, 1 << 1, ""),
            new("木", 2, 1 << 2, ""),
            new("火", 3, 1 << 3, ""),
            new("土", 4, 1 << 4, ""),
            new("攻击", 5, 1 << 5, ""),
            new("防御", 6, 1 << 6, ""),
            new("灵气", 7, 1 << 7, ""),
            new("气血", 8, 1 << 8, ""),
            new("二动", 9, 1 << 9, ""),
            new("升华", 10, 1 << 10, ""),
            new("一次性", 11, 1 << 11, ""),
            new("自指", 12, 1 << 12, ""),
        });
    }

    public void Init()
    {
        List.Do(entry =>
        {
            entry.GenerateDescription();
        });
    }
    
    public static TagEntry Jin      => Encyclopedia.TagCategory.List[0];
    public static TagEntry Shui     => Encyclopedia.TagCategory.List[1];
    public static TagEntry Mu       => Encyclopedia.TagCategory.List[2];
    public static TagEntry Huo      => Encyclopedia.TagCategory.List[3];
    public static TagEntry Tu       => Encyclopedia.TagCategory.List[4];
    public static TagEntry Attack   => Encyclopedia.TagCategory.List[5];
    public static TagEntry Defend   => Encyclopedia.TagCategory.List[6];
    public static TagEntry Mana     => Encyclopedia.TagCategory.List[7];
    public static TagEntry Health   => Encyclopedia.TagCategory.List[8];
    public static TagEntry Swift    => Encyclopedia.TagCategory.List[9];
    public static TagEntry Exhaust  => Encyclopedia.TagCategory.List[10];
    public static TagEntry Deplete  => Encyclopedia.TagCategory.List[11];
    public static TagEntry ZiZhi    => Encyclopedia.TagCategory.List[12];

    public static int Length => Encyclopedia.TagCategory.Count();
}
