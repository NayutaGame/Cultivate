
public interface AnnotatableFormation : Annotatable
{
    string GetName();
    string GetConditionDescription();
    int[] GetCriticalProgresses();
    int GetProgress();
    Description GetRewardDescription(int progress);
    string GetTrivia(int progress);
    SpriteEntry GetBackgroundSprite();
    SpriteEntry GetIconSprite();
}