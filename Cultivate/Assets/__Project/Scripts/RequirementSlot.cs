
using System;
using System.Collections.Generic;

public class RequirementSlot : Addressable
{
    private RunSkillDescriptor _descriptor;
    private RunSkill _skill;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
        { "Skill",                      thisObject => ((RequirementSlot)thisObject)._skill },
    };
    public object Get(string s) => Accessor[s](this);
    public RequirementSlot(RunSkillDescriptor descriptor)
    {
        _descriptor = descriptor;
        _skill = null;
    }

    public bool IsOccupied()
        => _skill != null;
}