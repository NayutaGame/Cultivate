
using System;

public interface IFormationModel : IMarkedSliderModel
{
    string GetName();
    JingJie GetLowestJingJie();
    JingJie? GetActivatedJingJie();
    string GetConditionDescription();
    string GetRewardDescriptionFromJingJie(JingJie jingJie);

    string GetHighlightedRewardDescriptionFromJingJie(JingJie jingJie);
    string GetRewardDescriptionAnnotationFromJingJie(JingJie jingJie);
    
    string GetTriviaFromJingJie(JingJie jingJie);
    JingJie GetIncrementedJingJie(JingJie jingJie);
    int GetRequirementFromJingJie(JingJie jingJie);
    Predicate<ISkill> GetContributorPred();
    SpriteEntry GetSprite();
}
