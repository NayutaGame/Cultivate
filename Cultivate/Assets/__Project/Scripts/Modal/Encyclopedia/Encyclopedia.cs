
using System;
using System.Collections.Generic;

public class Encyclopedia : Addressable
{
    public static SpriteCategory SpriteCategory;
    public static AudioCategory AudioCategory;

    public static KeywordCategory KeywordCategory;
    public static BuffCategory BuffCategory;
    public static SkillCategory SkillCategory;
    public static EntityCategory EntityCategory;
    public static TechCategory TechCategory;
    public static NodeCategory NodeCategory;
    public static FormationCategory FormationCategory;
    public static CharacterCategory CharacterCategory;
    public static DifficultyCategory DifficultyCategory;

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

        KeywordCategory = new();
        BuffCategory = new();
        SkillCategory = new();
        EntityCategory = new();
        TechCategory = new();
        NodeCategory = new();
        FormationCategory = new();
        CharacterCategory = new();
        DifficultyCategory = new();

        TechCategory.Init();

        KeywordCategory.Init();
        BuffCategory.Init();
        SkillCategory.Init();
        DifficultyCategory.Init();

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
