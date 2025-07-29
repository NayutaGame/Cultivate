
using TMPro;
using UnityEngine;

public class AchievementAnnotationView : AnnotationView
{
    [SerializeField] private UnlockIcon UnlockIcon;
    [SerializeField] private TMP_Text Title;
    [SerializeField] private TMP_Text ConditionDescription;
    [SerializeField] private TMP_Text RewardDescription;
    
    public override Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d)
    {
        if (d is ImageAnnotationAlignmentDetails)
        {
            return UnlockIcon.GetRect().position - GetRect().position;
        }

        int characterIndex = 0;
        if (d is CharacterAnnotationAlignmentDetails characterD)
        {
            Refresh();
            characterIndex = characterD.CharacterIndex;
        }

        if (Title.textInfo != null && Title.textInfo.characterInfo.Length > characterIndex)
        {
            var charInfo = Title.textInfo.characterInfo[characterIndex];
            Vector3 charCenter = (charInfo.bottomLeft + charInfo.topRight) * 0.5f;
            Vector3 charWorldCenter = Title.transform.TransformPoint(charCenter);

            return charWorldCenter - GetRect().position;
        }

        return Title.rectTransform.position - GetRect().position;
    }
    
    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        AnnotationDetails d = Get<AnnotationDetails>();
        UnlockIcon.SetAddress(d.Address);
    }

    public override void Refresh()
    {
        base.Refresh();
        
        AnnotationDetails d = Get<AnnotationDetails>();
        AnnotatableAchievement achievement = d.Address.Get<AnnotatableAchievement>();
        Title.text = achievement.GetName();
        ConditionDescription.text = achievement.GetConditionDescription().GetHighlightedString();
        RewardDescription.text = achievement.GetRewardDescription().GetHighlightedString();

        UnlockIcon.Refresh();
    }
}
