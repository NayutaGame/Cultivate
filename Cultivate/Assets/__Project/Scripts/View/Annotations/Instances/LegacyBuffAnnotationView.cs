
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LegacyBuffAnnotationView : XView
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

        object obj = Get<object>();
        if (InterpretAsBuff(obj as Buff)) return;
        if (InterpretAsBuffEntry(obj as BuffEntry)) return;
    }

    private bool InterpretAsBuff(Buff buff)
    {
        if (buff is null)
            return false;
        
        Icon.sprite = buff.GetEntry().GetSprite();
        TitleText.text = $"{buff.GetName()} {buff.Stack}";
        DescriptionText.text = buff.GetEntry().GetLiteralDescription().ToString();
        AnnotationText.text = buff.GetCascadeAnnotated();

        string trivia = buff.GetTrivia();
        bool hasTrivia = trivia != null;

        LowerSeparator.SetActive(hasTrivia);
        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;

        return true;
    }

    private bool InterpretAsBuffEntry(BuffEntry buff)
    {
        if (buff is null)
            return false;
        
        Icon.sprite = buff.GetSprite();
        TitleText.text = $"{buff.GetName()}";
        DescriptionText.text = buff.GetLiteralDescription().ToString();
        AnnotationText.text = buff.GetCascadeAnnotated();

        string trivia = buff.GetTrivia();
        bool hasTrivia = trivia != null;

        LowerSeparator.SetActive(hasTrivia);
        TriviaText.gameObject.SetActive(hasTrivia);

        if (hasTrivia)
            TriviaText.text = trivia;

        return true;
    }
}
