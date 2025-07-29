
using TMPro;
using UnityEngine;

public class FormationAnnotationView : AnnotationView
{
    [SerializeField] private FormationIconView FormationIconView;
    [SerializeField] private TMP_Text Title;
    [SerializeField] private TMP_Text ConditionDescriptionText;
    [SerializeField] private RatingBarView RatingBarView;
    [SerializeField] private TMP_Text RewardDescriptionText;
    [SerializeField] private TMP_Text Trivia;

    private int _showingScore;
    
    public override Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d)
    {
        if (d is ImageAnnotationAlignmentDetails)
        {
            return FormationIconView.GetRect().position - GetRect().position;
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
        FormationIconView.SetAddress(d.Address);
        
        AnnotatableFormation formation = d.Address.Get<AnnotatableFormation>();
        int[] criticalValues = formation.GetCriticalProgresses();
        int maxValue = criticalValues[0];
        int value = formation.GetProgress();
        
        RatingBarModel ratingBarModel = new RatingBarModel(maxValue, value, criticalValues);
        RatingBarView.SetModel(ratingBarModel);
        RatingBarView.ValueChangedNeuron.Join(SetPreviewProgress);
    }

    public override void Refresh()
    {
        base.Refresh();
        
        AnnotationDetails d = Get<AnnotationDetails>();
        AnnotatableFormation formation = d.Address.Get<AnnotatableFormation>();
        Title.text = formation.GetName();
        ConditionDescriptionText.text = formation.GetConditionDescription();
        
        SetPreviewProgress(formation.GetProgress());
        
        FormationIconView.Refresh();
    }

    public void SetPreviewProgress(int progress)
    {
        AnnotationDetails d = Get<AnnotationDetails>();
        AnnotatableFormation formation = d.Address.Get<AnnotatableFormation>();
        
        RewardDescriptionText.text = formation.GetRewardDescription(progress).GetHighlightedString();
        Trivia.text = formation.GetTrivia(progress);
    }
}
