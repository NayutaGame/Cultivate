
using TMPro;
using UnityEngine;

public class BuffAnnotation : AnnotationBehaviour
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

        Buff buff = Get<Buff>();
        if (buff == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

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
