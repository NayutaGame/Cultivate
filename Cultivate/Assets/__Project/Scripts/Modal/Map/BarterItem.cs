
using System;
using System.Collections.Generic;

public class BarterItem : Addressable
{
    public SkillEntryDescriptor FromSkill;
    public SkillEntryDescriptor ToSkill;
    private Action<BarterItem> ExchangeFunc;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "FromSkill",                  thisObject => ((BarterItem)thisObject).FromSkill },
        { "ToSkill",                    thisObject => ((BarterItem)thisObject).ToSkill },
    };
    public object Get(string s) => Accessor[s](this);
    public BarterItem(SkillEntryDescriptor fromSkill, SkillEntryDescriptor toSkill, Action<BarterItem> exchangeFunc)
    {
        FromSkill = fromSkill;
        ToSkill = toSkill;
        ExchangeFunc = exchangeFunc;
    }

    public void Exchange()
        => ExchangeFunc.Invoke(this);

    public bool Affordable()
        => RunManager.Instance.Environment.DeckIndexFromDescriptor(out DeckIndex _, FromSkill);
}
