
using System;
using System.Collections.Generic;

public class BarterItem : Addressable
{
    public SkillEntryDescriptor FromSkill;
    public SkillEntryDescriptor ToSkill;
    private Action<BarterItem> ExchangeFunc;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public BarterItem(SkillEntryDescriptor fromSkill, SkillEntryDescriptor toSkill, Action<BarterItem> exchangeFunc)
    {
        _accessors = new()
        {
            { "FromSkill",         () => FromSkill },
            { "ToSkill",           () => ToSkill },
        };
        FromSkill = fromSkill;
        ToSkill = toSkill;
        ExchangeFunc = exchangeFunc;
    }

    public void Exchange()
        => ExchangeFunc.Invoke(this);

    public bool Affordable()
        => RunManager.Instance.Environment.DeckIndexFromDescriptor(out DeckIndex _, FromSkill);
}
