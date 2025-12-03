
using System;
using System.Collections.Generic;

public class GachaItem : Addressable
{
    public SkillGhost Skill;
    public Action<GachaItem> GachaFunc;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Skill",         thisObject => ((GachaItem)thisObject).Skill },
    };
    public object Get(string s) => Accessor[s](this);
    public GachaItem(SkillGhost skill, Action<GachaItem> gachaFunc)
    {
        Skill = skill;
        GachaFunc = gachaFunc;
    }

    public void GachaProcedure()
        => GachaFunc(this);
}