
public interface IFormationModel : IMarkedSliderModel
{
    string GetName();
    JingJie GetLowestJingJie();
    JingJie? GetActivatedJingJie();
    string GetConditionDescription();
    string GetRewardDescriptionFromJingJie(JingJie jingJie);
    string GetTriviaFromJingJie(JingJie jingJie);
    JingJie GetIncrementedJingJie(JingJie jingJie);
    int GetRequirementFromJingJie(JingJie jingJie);
}
