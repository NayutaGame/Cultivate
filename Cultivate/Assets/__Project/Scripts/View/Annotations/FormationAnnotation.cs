using TMPro;
using UnityEngine;

public class FormationAnnotation : AnnotationView
{
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private GameObject LowerSeparator;
    [SerializeField] private TMP_Text TriviaText;

    public override void Refresh()
    {
        base.Refresh();

        if (GetAddress() == null)
        {
            gameObject.SetActive(false);
            return;
        }

        object obj = Get<object>();

        FormationEntry formationEntry = obj as FormationEntry ?? (obj as Formation)?.Entry;

        if (formationEntry == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        TitleText.text = $"{formationEntry.GetJingJie()} {formationEntry.GetName()}\n\n{formationEntry.GetConditionDescription()}\n\n{formationEntry.GetRewardDescription()}";
        // DescriptionText.text = formation.GetAnnotationText();

        string trivia = formationEntry.GetTrivia();
        bool hasTrivia = trivia != null;

        LowerSeparator.SetActive(hasTrivia);
        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;
    }
}
