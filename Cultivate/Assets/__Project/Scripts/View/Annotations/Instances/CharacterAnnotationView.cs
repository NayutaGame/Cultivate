
using TMPro;
using UnityEngine;

public class CharacterAnnotationView : AnnotationView
{
    [SerializeField] private CharacterIconView CharacterIconView;
    [SerializeField] private TMP_Text Title;
    [SerializeField] private TMP_Text AbilityDescription;
    
    public override Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d)
    {
        if (d is ImageAnnotationAlignmentDetails)
        {
            return CharacterIconView.GetRect().position - GetRect().position;
        }

        int characterIndex = 0;
        if (d is CharacterAnnotationAlignmentDetails characterD)
        {
            Refresh();
            characterIndex = characterD.CharacterIndex;
        }

        if (Title.textInfo != null && Title.textInfo.characterInfo.Length > characterIndex)
        {
            var charInfo = Title.textInfo.characterInfo[characterIndex];
            Vector3 charCenter = (charInfo.bottomLeft + charInfo.topRight) * 0.5f;
            Vector3 charWorldCenter = Title.transform.TransformPoint(charCenter);

            return charWorldCenter - GetRect().position;
        }

        return Title.rectTransform.position - GetRect().position;
    }
    
    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        AnnotationDetails d = Get<AnnotationDetails>();
        CharacterIconView.SetAddress(d.Address);
    }

    public override void Refresh()
    {
        base.Refresh();
        
        AnnotationDetails d = Get<AnnotationDetails>();
        AnnotatableCharacter character = d.Address.Get<AnnotatableCharacter>();
        Title.text = character.GetTitle();
        AbilityDescription.text = character.GetAbilityDescription().GetHighlightedString();

        CharacterIconView.Refresh();
    }
}
