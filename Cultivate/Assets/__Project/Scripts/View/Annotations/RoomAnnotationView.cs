
using TMPro;
using UnityEngine;

public class RoomAnnotationView : SimpleView
{
    // [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text DescriptionText;
    // [SerializeField] private TMP_Text AnnotationText;
    // [SerializeField] private GameObject LowerSeparator;
    // [SerializeField] private TMP_Text TriviaText;

    public override void Refresh()
    {
        base.Refresh();

        Room room = Get<Room>();
        DescriptionText.text = room.Descriptor.GetDescription();
        // Debug.Log(characterProfile.GetEntry().GetName());

        // Buff buff = Get<Buff>();
        // TitleText.text = $"{buff.GetName()} {buff.Stack}";
        // DescriptionText.text = buff.GetEntry().GetDescription();
        // AnnotationText.text = buff.GetExplanation();
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
