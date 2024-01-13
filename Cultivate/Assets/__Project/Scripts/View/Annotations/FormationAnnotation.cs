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

        object obj = Get<object>();

        FormationEntry formationEntry = null;

        if (obj is FormationEntry e)
        {
            formationEntry = e;
        }
        else if (obj is Formation f)
        {
            formationEntry = f.Entry;
        }

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
