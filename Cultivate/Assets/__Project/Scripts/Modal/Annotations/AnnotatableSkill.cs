
using UnityEngine;

public interface AnnotatableSkill : AnnotatableCost
{
    JingJie GetLowestJingJie();
    JingJie GetHighestJingJie();
    Sprite GetSprite();
    string GetName();
    Description GetDescription(JingJie showingJingJie);
    TagComposite GetTagComposite();
    Sprite GetJingJieSprite(JingJie showingJingJie);
}