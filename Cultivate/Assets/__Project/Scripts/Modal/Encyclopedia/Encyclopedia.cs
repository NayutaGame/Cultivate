
using System;
using System.Collections.Generic;

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
    public static RunEntity EntityEditorHomeEntity;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new()
        {
            { "FormationCategory", () => FormationCategory },
            { "SkillCategory", () => SkillCategory },

            { "EntityEditableList", () => EntityEditableList },
            { "EntityEditorHomeEntity", () => EntityEditorHomeEntity }
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
        EntityEditorHomeEntity = RunEntity.Default;
    }

    public void CopyToTop(int currentIndex)
    {
        EntityEditableList.Replace(EntityEditableList[currentIndex], RunEntity.FromTemplate(EntityEditorHomeEntity));
    }

    public void SwapTopAndBottom(int currentIndex)
    {
        RunEntity temp = EntityEditorHomeEntity;
        EntityEditorHomeEntity = EntityEditableList[currentIndex];
        EntityEditableList.Replace(EntityEditableList[currentIndex], temp);
    }

    public void CopyToBottom(int currentIndex)
    {
        EntityEditorHomeEntity = RunEntity.FromTemplate(EntityEditableList[currentIndex]);
    }
}
