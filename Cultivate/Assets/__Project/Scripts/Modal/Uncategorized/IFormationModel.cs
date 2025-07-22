
using System;

public interface IFormationModel : IMarkedSliderModel
{
    string GetName();
    JingJie GetLowestJingJie();
    JingJie? GetActivatedJingJie();
    string GetConditionDescription();

    Description GetRewardDescription(JingJie jingJie);
    
    string GetTriviaFromJingJie(JingJie jingJie);
    JingJie GetIncrementedJingJie(JingJie jingJie);
    int GetRequirementFromJingJie(JingJie jingJie);
    Predicate<ISkill> GetContributorPred();
    SpriteEntry GetSprite();
}
