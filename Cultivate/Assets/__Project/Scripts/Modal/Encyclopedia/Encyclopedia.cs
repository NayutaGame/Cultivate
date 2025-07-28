
using System;
using System.Collections.Generic;
using CLLibrary;

public class Encyclopedia : Addressable
{
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
    };
    public object Get(string s) => Accessor[s](this);
    public Encyclopedia()
    {
        Fib.Init();

        SpriteCategory = new();
        AudioCategory = new();
        PrefabCategory = new();

        TagCategory = new();

        KeywordCategory = new();
        BuffCategory = new();
        SkillCategory = new();

        PackCategory = new();
        
        EntityCategory = new();
        RoomCategory = new();
        FormationCategory = new();
        CharacterCategory = new();
        DifficultyCategory = new();
        MapCategory = new();
        
        AchievementCategory = new();
        
        Description.BuildSymbolizeRegex();
        SkillCategory.RefreshNameDict();
        JingJie.Traversal.Do(j => j.Init());
        
        TagCategory.Init();
        KeywordCategory.Init();
        BuffCategory.Init();
        SkillCategory.Init();
        PackCategory.Init();
        FormationCategory.Init();
        CharacterCategory.Init();
        DifficultyCategory.Init();
        AchievementCategory.Init();

        JingJieToAudio = new()
        {
            { JingJie.LianQi, "BGMLianQi" },
            { JingJie.ZhuJi, "BGMZhuJi" },
            { JingJie.JinDan, "BGMJinDan" },
            { JingJie.YuanYing, "BGMYuanYing" },
            { JingJie.HuaShen, "BGMHuaShen" },
        };
        
        SpriteCategory.RefreshDict();
    }

    private static Dictionary<JingJie, AudioEntry> JingJieToAudio;

    public static AudioEntry AudioFromJingJie(JingJie jingJie)
        => JingJieToAudio[jingJie];
}
