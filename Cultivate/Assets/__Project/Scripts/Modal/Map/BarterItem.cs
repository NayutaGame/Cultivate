
using System;
using System.Collections.Generic;

public class BarterItem : Addressable
{
    public SkillEntryDescriptor FromSkill;
    public SkillEntryDescriptor ToSkill;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public BarterItem(SkillEntryDescriptor fromSkill, SkillEntryDescriptor toSkill)
    {
        _accessors = new()
        {
            { "FromSkill",         () => FromSkill },
            { "ToSkill",           () => ToSkill },
        };
        FromSkill = fromSkill;
        ToSkill = toSkill;
    }

    public bool Affordable()
        => RunManager.Instance.Environment.FindDeckIndex(out DeckIndex _, FromSkill);
}
