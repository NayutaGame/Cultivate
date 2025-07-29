
using TMPro;
using UnityEngine;

public class JingJieAnnotationView : AnnotationView
{
    [SerializeField] private JingJieView JingJieView;
    [SerializeField] private TMP_Text Title;
    [SerializeField] private TMP_Text Description;
    
    public override Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d)
    {
        if (d is ImageAnnotationAlignmentDetails)
        {
            return JingJieView.GetRect().position - GetRect().position;
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
        JingJieView.SetAddress(d.Address);
    }

    public override void Refresh()
    {
        base.Refresh();
        
        AnnotationDetails d = Get<AnnotationDetails>();
        AnnotatableJingJie annotatableJingJie = d.Address.Get<AnnotatableJingJie>();
        JingJie jingJie = annotatableJingJie.GetJingJie();
        Title.text = jingJie.GetName();
        Description.text = jingJie.GetDescription().GetHighlightedString();

        JingJieView.Refresh();
    }
}
