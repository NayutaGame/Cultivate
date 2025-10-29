
using System;
using System.Collections.Generic;

public class BarterItem : Addressable
{
    public SkillReference FromSkill;
    public SkillReference ToSkill;
    private Action<BarterItem> ExchangeFunc;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "FromSkill",                  thisObject => ((BarterItem)thisObject).FromSkill },
        { "ToSkill",                    thisObject => ((BarterItem)thisObject).ToSkill },
    };
    public object Get(string s) => Accessor[s](this);
    public BarterItem(SkillReference fromSkill, SkillReference toSkill, Action<BarterItem> exchangeFunc)
    {
        FromSkill = fromSkill;
        ToSkill = toSkill;
        ExchangeFunc = exchangeFunc;
    }

    public void Exchange()
        => ExchangeFunc.Invoke(this);

    public bool Affordable()
        => RunManager.Instance.Environment.DeckIndexFromQuery(out DeckIndex _, RunSkillQuery.FromSkillReference(FromSkill));
}
