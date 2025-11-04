
using System;
using System.Collections.Generic;

public class BarterItem : Addressable
{
    public SkillGhost FromSkill;
    public SkillGhost ToSkill;
    private Action<BarterItem> ExchangeFunc;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "FromSkill",                  thisObject => ((BarterItem)thisObject).FromSkill },
        { "ToSkill",                    thisObject => ((BarterItem)thisObject).ToSkill },
    };
    public object Get(string s) => Accessor[s](this);
    public BarterItem(SkillGhost fromSkill, SkillGhost toSkill, Action<BarterItem> exchangeFunc)
    {
        FromSkill = fromSkill;
        ToSkill = toSkill;
        ExchangeFunc = exchangeFunc;
    }

    public void Exchange()
        => ExchangeFunc.Invoke(this);

    public bool Affordable()
        => RunManager.Instance.Environment.DeckIndexFromQuery(out DeckIndex _, RunSkillQuery.FromSkillGhost(FromSkill));
}
