
using System;
using System.Collections.Generic;

public class RequirementSlot : Addressable
{
    private int _index;
    private RunSkillDescriptor _descriptor;
    private RunSkill _skill;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
        { "Skill",                      thisObject => ((RequirementSlot)thisObject)._skill },
        { "Descriptor",                 thisObject => ((RequirementSlot)thisObject)._descriptor },
    };
    public object Get(string s) => Accessor[s](this);
    public RequirementSlot(int index, RunSkillDescriptor descriptor)
    {
        _index = index;
        _descriptor = descriptor;
        _skill = null;
    }

    public RunSkill Skill
    {
        get => _skill;
        set => _skill = value?.Clone();
    }

    public RunSkillDescriptor Descriptor()
        => _descriptor;

    public bool IsOccupied()
        => _skill != null;

    public DeckIndex ToDeckIndex()
        => DeckIndex.FromRequirement(_index);
}