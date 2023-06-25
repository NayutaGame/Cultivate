using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Encyclopedia : CLLibrary.Singleton<Encyclopedia>
{
    public static SpriteCategory SpriteCategory;
    public static SoundCategory SoundCategory;

    public static KeywordCategory KeywordCategory;
    public static BuffCategory BuffCategory;
    public static SkillCategory SkillCategory;
    public static EntityCategory EntityCategory;
    public static TechCategory TechCategory;
    public static NodeCategory NodeCategory;

    public override void DidAwake()
    {
        base.DidAwake();

        SpriteCategory = new();
        SoundCategory = new();

        KeywordCategory = new();
        BuffCategory = new();
        SkillCategory = new();
        EntityCategory = new();
        TechCategory = new();
        NodeCategory = new();

        TechCategory.Init();

        KeywordCategory.Init();
        BuffCategory.Init();
        SkillCategory.Init();
    }
}
