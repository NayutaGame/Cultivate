
using UnityEngine;

public interface AnnotatableSkill : Annotatable
{
    JingJie GetJingJie();
    JingJie GetLowestJingJie();
    JingJie GetHighestJingJie();
    Sprite GetSprite();
    CostDescription GetLiteralCostDescription(JingJie showingJingJie);
    string GetName();
    Description GetDescription(JingJie showingJingJie);
    TagComposite GetTagComposite();
    Sprite GetJingJieSprite(JingJie showingJingJie);
}