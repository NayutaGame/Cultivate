
using System;
using System.Collections.Generic;

public class Encyclopedia : Addressable
{
    public static SpriteCategory SpriteCategory;
    public static AudioCategory AudioCategory;
    public static PrefabCategory PrefabCategory;

    public static KeywordCategory KeywordCategory;
    public static BuffCategory BuffCategory;
    public static SkillCategory SkillCategory;
    public static EntityCategory EntityCategory;
    public static RoomCategory RoomCategory;
    public static FormationCategory FormationCategory;
    public static CharacterCategory CharacterCategory;
    public static DifficultyCategory DifficultyCategory;
    public static MapCategory MapCategory;

    public static PackCategory PackCategory;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Encyclopedia()
    {
        _accessors = new()
        {
            { "FormationCategory", () => FormationCategory },
            { "SkillCategory", () => SkillCategory },
        };

        Fib.Init();
        SkillType.Init();

        SpriteCategory = new();
        AudioCategory = new();
        PrefabCategory = new();

        KeywordCategory = new();
        BuffCategory = new();
        SkillCategory = new();
        EntityCategory = new();
        RoomCategory = new();
        FormationCategory = new();
        CharacterCategory = new();
        DifficultyCategory = new();
        MapCategory = new();
        
        KeywordCategory.Init();
        BuffCategory.Init();
        SkillCategory.Init();
        DifficultyCategory.Init();
        FormationCategory.Init();
        
        PackCategory = new();

        JingJieToAudio = new()
        {
            { JingJie.LianQi, "BGMLianQi" },
            { JingJie.ZhuJi, "BGMZhuJi" },
            { JingJie.JinDan, "BGMJinDan" },
            { JingJie.YuanYing, "BGMYuanYing" },
            { JingJie.HuaShen, "BGMHuaShen" },
        };
    }

    private static Dictionary<JingJie, AudioEntry> JingJieToAudio;

    public static AudioEntry AudioFromJingJie(JingJie jingJie)
        => JingJieToAudio[jingJie];
}
