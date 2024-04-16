using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BarterItem : Addressable
{
    public SkillDescriptor FromSkill;
    public SkillDescriptor ToSkill;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public BarterItem(SkillDescriptor fromSkill, SkillDescriptor toSkill)
    {
        _accessors = new()
        {
            { "FromSkill",         () => FromSkill },
            { "ToSkill",               () => ToSkill },
        };
        FromSkill = fromSkill;
        ToSkill = toSkill;
    }

    public bool Affordable()
    {
        DeckIndex? deckIndex = RunManager.Instance.Environment.FindDeckIndex(FromSkill);
        return deckIndex != null;
    }
}
