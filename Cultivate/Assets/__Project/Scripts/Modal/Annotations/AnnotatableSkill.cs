
using UnityEngine;

public interface AnnotatableSkill : AnnotatableCost
{
    JingJie GetLowestJingJie();
    JingJie GetHighestJingJie();
    Sprite GetSprite();
    string GetName();
    Description GetDescription(JingJie showingJingJie);
    Sprite GetJingJieSprite(JingJie showingJingJie);
    TagComposite GetTagComposite();
    PackEntry GetPackEntry();
}