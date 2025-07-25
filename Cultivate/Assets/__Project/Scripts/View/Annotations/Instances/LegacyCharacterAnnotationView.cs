
using TMPro;
using UnityEngine;

public class LegacyCharacterAnnotationView : XView
{
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text DescriptionText;
    // [SerializeField] private TMP_Text AnnotationText;
    // [SerializeField] private GameObject LowerSeparator;
    // [SerializeField] private TMP_Text TriviaText;

    public override void Refresh()
    {
        base.Refresh();

        CharacterProfile characterProfile = Get<CharacterProfile>();
        TitleText.text = characterProfile.GetEntry().GetName();
        DescriptionText.text = characterProfile.GetEntry().GetAbilityDescription().GetHighlightedString();
        // Debug.Log(characterProfile.GetEntry().GetName());

        // Buff buff = Get<Buff>();
        // TitleText.text = $"{buff.GetName()} {buff.Stack}";
        // DescriptionText.text = buff.GetEntry().GetDescription();
        // AnnotationText.text = buff.GetCascadeAnnotated();
        //
        // string trivia = buff.GetTrivia();
        // bool hasTrivia = trivia != null;
        //
        // LowerSeparator.SetActive(hasTrivia);
        // TriviaText.gameObject.SetActive(hasTrivia);
        //
        // if (hasTrivia)
        //     TriviaText.text = trivia;
    }
}
