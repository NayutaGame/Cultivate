
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationAnnotationView : XView
{
    private JingJie _showingJingJie;
    private int _showingMark;

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

        _showingJingJie = formation.GetActivatedJingJie() ?? formation.GetLowestJingJie();
        _showingMark = formation.GetRequirementFromJingJie(_showingJingJie);

        MarkedSlider.SetAddress(GetAddress());
    }

    public override void Refresh()
    {
        base.Refresh();

        IFormationModel formation = Get<IFormationModel>();

        TitleText.text = $"{_showingJingJie} {formation.GetName()}";
        ConditionDescriptionText.text = formation.GetConditionDescription();

        MarkedSlider.Refresh();
        MarkedSlider.MarkList.TraversalActive().Do(itemBehaviour =>
        {
            MarkView markView = itemBehaviour.GetSimpleView() as MarkView;
            MarkModel markModel = markView.Get<MarkModel>();

            bool activated = IsActivatedFromFormation(formation, markModel._mark);
            bool showing = markModel._mark == _showingMark;

            markView.SetState(activated, showing);
        });

        RewardDescriptionText.text = formation.GetHighlightedRewardDescriptionFromJingJie(_showingJingJie);
        AnnotationText.text = formation.GetRewardDescriptionAnnotationFromJingJie(_showingJingJie);

        SetTrivia(formation.GetTriviaFromJingJie(_showingJingJie));
    }

    private bool IsActivatedFromFormation(IFormationModel formation, int mark)
    {
        JingJie? activatedJingJie = formation.GetActivatedJingJie();
        if (activatedJingJie.HasValue)
            return formation.GetRequirementFromJingJie(activatedJingJie.Value) == mark;

        return false;
    }

    private void SetTrivia(string trivia)
    {
        bool hasTrivia = trivia != null;

        LowerSeparator.SetActive(hasTrivia);
        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;
    }

    public void SwitchShowingJingJie(LegacyInteractBehaviour ib, PointerEventData d)
    {
        IFormationModel formation = Get<IFormationModel>();
        _showingJingJie = formation.GetIncrementedJingJie(_showingJingJie);
        _showingMark = formation.GetRequirementFromJingJie(_showingJingJie);
        Refresh();
    }
}
