
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encyclopedia : CLLibrary.Singleton<Encyclopedia>, Addressable
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

    public static EntityEditableList EntityEditableList;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new()
        {
            { "FormationCategory", () => FormationCategory },
            { "SkillCategory", () => SkillCategory },
        };

        SkillType.Init();
        MechType.Init();

        SpriteCategory = new();
        AudioCategory = new();

        KeywordCategory = new();
        BuffCategory = new();
        SkillCategory = new();
        EntityCategory = new();
        TechCategory = new();
        NodeCategory = new();
        FormationCategory = new();

        TechCategory.Init();

        KeywordCategory.Init();
        BuffCategory.Init();
        SkillCategory.Init();

        EntityEditableList = EntityEditableList.ReadFromFile();
    }
}
