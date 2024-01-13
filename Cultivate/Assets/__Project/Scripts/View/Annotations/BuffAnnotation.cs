
using TMPro;
using UnityEngine;

public class BuffAnnotation : AnnotationView
{
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private GameObject LowerSeparator;
    [SerializeField] private TMP_Text TriviaText;

    public override void Refresh()
    {
        base.Refresh();

        Buff buff = Get<Buff>();

        TitleText.text = $"{buff.Stack} {buff.GetName()}\n\n{buff.GetEntry().Description}";
        DescriptionText.text = buff.GetAnnotationText();

        string trivia = buff.GetTrivia();
        bool hasTrivia = trivia != null;

        LowerSeparator.SetActive(hasTrivia);
        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;
    }
}
