
using System;
using System.Collections.Generic;
using CLLibrary;
using Unity.VisualScripting;

public class Encyclopedia : Addressable
{
    public static ICategory<Entry>[] Categories;
    
    public static WuXingCategory WuXingCategory;
    public static JingJieCategory JingJieCategory;
    public static SpriteCategory SpriteCategory;
    public static AudioCategory AudioCategory;
    public static PrefabCategory PrefabCategory;
    public static TagCategory TagCategory;
    public static KeywordCategory KeywordCategory;
    public static BuffCategory BuffCategory;
    public static SkillCategory SkillCategory;
    public static PackCategory PackCategory;
    public static EntityCategory EntityCategory;
    public static RoomCategory RoomCategory;
    public static FormationCategory FormationCategory;
    public static CharacterCategory CharacterCategory;
    public static DifficultyCategory DifficultyCategory;
    public static MapCategory MapCategory;
    public static AchievementCategory AchievementCategory;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "TagCategory",                thisObject => TagCategory },
        { "KeywordCategory",            thisObject => KeywordCategory },
        { "BuffCategory",               thisObject => BuffCategory },
        { "SkillCategory",              thisObject => SkillCategory },
        { "FormationCategory",          thisObject => FormationCategory },
        { "CharacterCategory",          thisObject => CharacterCategory },
        { "JingJieCategory",            thisObject => JingJieCategory },
        { "PackCategory",               thisObject => PackCategory },
    };
    public object Get(string s) => Accessor[s](this);
    public Encyclopedia()
    {
        Fib.Init();
        
        Categories = new ICategory<Entry>[]
        {
            WuXingCategory      = new(),
            JingJieCategory     = new(),
            SpriteCategory      = new(),
            AudioCategory       = new(),
            PrefabCategory      = new(),
            TagCategory         = new(),
            KeywordCategory     = new(),
            BuffCategory        = new(),
            SkillCategory       = new(),
            PackCategory        = new(),
            EntityCategory      = new(),
            RoomCategory        = new(),
            FormationCategory   = new(),
            CharacterCategory   = new(),
            DifficultyCategory  = new(),
            MapCategory         = new(),
            AchievementCategory = new(),
        };
        
        Description.InitStaticValues();

        Categories.Do(c => c.Init());
        
        SpriteCategory.RefreshDict();
    }
}
