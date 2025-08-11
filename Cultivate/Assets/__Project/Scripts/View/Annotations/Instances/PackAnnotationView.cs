
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PackAnnotationView : AnnotationView
{
    [SerializeField] private PackView PackView;
    [SerializeField] private TMP_Text Title;
    [SerializeField] private Image WuXingDeco;
    [SerializeField] private TMP_Text Description;
    [SerializeField] private TMP_Text Trivia;
    [SerializeField] private ListView Skills;
    
    public override Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d)
    {
        if (d is ImageAnnotationAlignmentDetails)
        {
            return PackView.GetRect().position - GetRect().position;
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
        PackView.SetAddress(d.Address);
        Skills.SetAddress(d.Address.Append(".Skills"));
    }

    public override void Refresh()
    {
        base.Refresh();
        
        AnnotationDetails d = Get<AnnotationDetails>();
        AnnotatablePack pack = d.Address.Get<AnnotatablePack>();
        Title.text = pack.GetName();
        SetWuXing(pack.GetWuXing());
        Description.text = pack.GetDescription().GetHighlightedString();
        Trivia.text = pack.GetTrivia();

        PackView.Refresh();
        Skills.Sync();
    }

    private void SetWuXing(WuXing wuXing)
    {
        WuXingDeco.sprite = wuXing.GetDecoSprite();
    }
}