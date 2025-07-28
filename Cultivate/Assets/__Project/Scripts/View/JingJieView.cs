
using UnityEngine;
using UnityEngine.UI;

public class JingJieView : XView
{
    [SerializeField] private Image Icon;

    public override void Refresh()
    {
        base.Refresh();
        
        AnnotatableJingJie annotatableJingJie = Get<AnnotatableJingJie>();
        JingJie jingJie = annotatableJingJie.GetJingJie();
        Icon.sprite = jingJie.GetSprite();
    }
}