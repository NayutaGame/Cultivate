
using System;
using System.Collections.Generic;
using UnityEngine;

public class RequirementSlot : AnnotatableSkill
{
    private int _index;
    private RunSkillQuery _query;
    private RunSkill _skill;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
        { "Skill",                      thisObject => ((RequirementSlot)thisObject)._skill },
        { "Descriptor",                 thisObject => ((RequirementSlot)thisObject)._query },
        { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
        { "PackEntry",                  thisObject => ((AnnotatableSkill)thisObject).GetPackEntry() },
    };
    public object Get(string s) => Accessor[s](this);
    public RequirementSlot(int index, RunSkillQuery query)
    {
        _index = index;
        _query = query;
        _skill = null;
    }

    public RunSkill Skill
    {
        get => _skill;
        set => _skill = value?.Clone();
    }

    public RunSkillQuery GetQuery()
        => _query;

    public bool IsOccupied()
        => _skill != null;

    public DeckIndex ToDeckIndex()
        => DeckIndex.FromRequirement(_index);

    public bool IsFulfilled()
        => _skill != null;
    
    public bool CanShowAnnotation()
        => _skill != null;
    
    public JingJie GetJingJie() => _skill.GetJingJie();
    public JingJie GetLowestJingJie() => _skill.GetLowestJingJie();
    public JingJie GetHighestJingJie() => _skill.GetHighestJingJie();
    public Sprite GetCardIllustration() => _skill.GetCardIllustration();
    public Sprite GetBarIllustration() => _skill.GetBarIllustration();
    public CostDescription GetLiteralCostDescription(JingJie showingJingJie) => _skill.GetLiteralCostDescription(showingJingJie);
    public string GetName() => _skill.GetName();
    public Description GetDescription(JingJie showingJingJie) => _skill.GetDescription(showingJingJie);
    public string GetTrivia() => _skill.GetTrivia();

    public TagComposite GetTagComposite() => _skill?.GetTagComposite();
    public PackEntry GetPackEntry() => _skill?.GetPackEntry();

    public Sprite GetJingJieSprite(JingJie showingJingJie) => _skill.GetJingJieSprite(showingJingJie);
}