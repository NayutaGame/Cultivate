
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffAnnotationView : LegacySimpleView
{
    [SerializeField] private Image Icon;
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private TMP_Text AnnotationText;
    [SerializeField] private GameObject LowerSeparator;
    [SerializeField] private TMP_Text TriviaText;

    public override void Refresh()
    {
        base.Refresh();

        Buff buff = Get<Buff>();
        Icon.sprite = buff.GetEntry().GetSprite();
        TitleText.text = $"{buff.GetName()} {buff.Stack}";
        DescriptionText.text = buff.GetEntry().GetDescription();
        AnnotationText.text = buff.GetExplanation();

        string trivia = buff.GetTrivia();
        bool hasTrivia = trivia != null;

        LowerSeparator.SetActive(hasTrivia);
        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;
    }
}
