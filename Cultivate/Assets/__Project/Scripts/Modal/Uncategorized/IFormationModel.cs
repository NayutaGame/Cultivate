
public interface IFormationModel : IMarkedSliderModel
{
    string GetName();
    JingJie GetJingJie();
    string GetConditionDescription();
    string GetRewardDescriptionFromJingJie(JingJie jingJie);
    int? GetProgress();
    string GetTriviaFromJingJie(JingJie jingJie);
}
