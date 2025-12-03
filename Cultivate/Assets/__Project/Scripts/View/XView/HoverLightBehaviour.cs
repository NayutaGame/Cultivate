using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(XView))]
public class HoverLightBehaviour : XBehaviour
{
    [SerializeField] private InteractBehaviour _ib;
    [SerializeField] private Image HoverImage;
    
    private Tween _handle;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        _ib.PointerEnterNeuron.Join(PointerEnter);
        _ib.PointerExitNeuron.Join(PointerExit);
    }

    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        _handle?.Kill();
        _handle = HoverImage.DOFade(1, 0.25f).SetEase(Ease.InOutQuad);
        _handle.SetAutoKill().Restart();
    }

    private void PointerExit(InteractBehaviour ib, PointerEventData d)
    {
        _handle?.Kill();
        _handle = HoverImage.DOFade(0, 0.25f).SetEase(Ease.InOutQuad);
        _handle.SetAutoKill().Restart();
    }
}