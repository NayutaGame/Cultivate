
using System;
using System.Collections.Generic;
using UnityEngine;

public class BarterBoardSlot : AnnotatableSkill
{
    public SkillGhost SkillGhost;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Skill",                      thisObject => ((BarterBoardSlot)thisObject).SkillGhost },
        { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
        { "PackEntry",                  thisObject => ((AnnotatableSkill)thisObject).GetPackEntry() },
    };
    public object Get(string s) => Accessor[s](this);
    public BarterBoardSlot()
    {
        
    }
    
    public bool IsOccupied() => SkillGhost != null;

    public bool CanShowAnnotation() => SkillGhost != null;
    public JingJie GetJingJie() => SkillGhost.GetJingJie();
    public CostDescription GetLiteralCostDescription(JingJie showingJingJie) => SkillGhost.GetLiteralCostDescription(showingJingJie);
    public JingJie GetLowestJingJie() => SkillGhost.GetLowestJingJie();
    public JingJie GetHighestJingJie() => SkillGhost.GetHighestJingJie();
    public Sprite GetCardIllustration() => SkillGhost.GetCardIllustration();
    public Sprite GetBarIllustration() => SkillGhost.GetBarIllustration();
    public string GetName() => SkillGhost.GetName();
    public Description GetDescription(JingJie showingJingJie) => SkillGhost.GetDescription(showingJingJie);
    public string GetTrivia() => SkillGhost.GetTrivia();
    public Sprite GetJingJieSprite(JingJie showingJingJie) => SkillGhost.GetJingJieSprite(showingJingJie);
    public TagComposite GetTagComposite() => SkillGhost.GetTagComposite();
    public PackEntry GetPackEntry() => SkillGhost.GetPackEntry();
}