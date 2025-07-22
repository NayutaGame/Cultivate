
using TMPro;
using UnityEngine;

public class TextAnnotationView : AnnotationView
{
    [SerializeField] private TMP_Text Title;
    [SerializeField] private TMP_Text Description;
    
    public override Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d)
    {
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
        // Description.PropagateLink.RegisterCallback(LinkCallback);
    }

    public override void Refresh()
    {
        base.Refresh();
        
        AnnotationDetails d = Get<AnnotationDetails>();
        AnnotatableText text = d.Address.Get<AnnotatableText>();
        
        Title.text = text.GetTitle();
        Description.text = text.GetDescription().GetHighlightedString();
    }

    // 如果需要link回调，可以添加类似SkillAnnotationView的LinkCallback方法
    // private void LinkCallback(TMP_Text text, TMP_LinkInfo linkInfo) { ... }
}