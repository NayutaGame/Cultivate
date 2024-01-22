
using TMPro;
using UnityEngine;

public class FormationAnnotationView : SimpleView
{
    private JingJie _jingJie;

    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text ConditionDescriptionText;
    [SerializeField] private MarkedSlider MarkedSlider;
    [SerializeField] private TMP_Text RewardDescriptionText;
    [SerializeField] private TMP_Text AnnotationText;
    [SerializeField] private GameObject LowerSeparator;
    [SerializeField] private TMP_Text TriviaText;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        IFormationModel formation = Get<IFormationModel>();
        _jingJie = formation.GetJingJie();
    }

    public override void Refresh()
    {
        base.Refresh();

        IFormationModel formation = Get<IFormationModel>();

        TitleText.text = $"{_jingJie} {formation.GetName()}";
        ConditionDescriptionText.text = formation.GetConditionDescription();

        MarkedSlider.SetAddress(GetAddress());

        RewardDescriptionText.text = formation.GetRewardDescriptionFromJingJie(_jingJie);
        // AnnotationText.text = formationEntry.GetAnnotationText();

        SetTrivia(formation.GetTriviaFromJingJie(_jingJie));
    }

    private void SetTrivia(string trivia)
    {
        bool hasTrivia = trivia != null;

        LowerSeparator.SetActive(hasTrivia);
        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;
    }
}
