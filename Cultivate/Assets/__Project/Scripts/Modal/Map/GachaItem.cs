
using System;
using System.Collections.Generic;
using UnityEngine;

public class GachaItem : Addressable, AnnotatableSkill
{
    public SkillGhost Skill;
    public Action<GachaItem> GachaFunc;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
        { "PackEntry",                  thisObject => ((AnnotatableSkill)thisObject).GetPackEntry() },
        // { "Skill",                      thisObject => ((GachaItem)thisObject).Skill },
    };
    public object Get(string s) => Accessor[s](this);

    public GachaItem(SkillGhost skill, Action<GachaItem> gachaFunc)
    {
        Skill = skill;
        GachaFunc = gachaFunc;
    }

    public void GachaProcedure()
        => GachaFunc(this);
    
    public bool CanShowAnnotation()
        => Skill?.CanShowAnnotation() ?? false;

    public JingJie GetJingJie()
        => Skill?.GetJingJie();

    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
        => Skill?.GetLiteralCostDescription(showingJingJie) ?? CostDescription.Empty;

    public JingJie GetLowestJingJie()
        => Skill?.GetLowestJingJie();

    public JingJie GetHighestJingJie()
        => Skill?.GetHighestJingJie();

    public Sprite GetCardIllustration()
        => Skill?.GetCardIllustration();

    public Sprite GetBarIllustration()
        => Skill?.GetBarIllustration();

    public string GetName()
        => Skill?.GetName();

    public Description GetDescription(JingJie showingJingJie)
        => Skill?.GetDescription(showingJingJie);

    public string GetTrivia()
        => Skill?.GetTrivia();

    public Sprite GetJingJieSprite(JingJie showingJingJie)
        => Skill?.GetJingJieSprite(showingJingJie);

    public TagComposite GetTagComposite()
        => Skill?.GetTagComposite();

    public PackEntry GetPackEntry()
        => Skill?.GetPackEntry();
}