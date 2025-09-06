
using UnityEngine;

public interface AnnotatableSkill : AnnotatableCost
{
    JingJie GetLowestJingJie();
    JingJie GetHighestJingJie();
    Sprite GetCardIllustration();
    Sprite GetBarIllustration();
    string GetName();
    Description GetDescription(JingJie showingJingJie);
    string GetTrivia();
    Sprite GetJingJieSprite(JingJie showingJingJie);
    TagComposite GetTagComposite();
    PackEntry GetPackEntry();
}