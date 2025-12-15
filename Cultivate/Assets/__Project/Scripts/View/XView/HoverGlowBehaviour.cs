
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverGlowBehaviour : XBehaviour
{
    [SerializeField] private InteractBehaviour _ib;

    [SerializeField] private Image HoverImage;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        SetInteractBehaviour(_ib);
    }

    public override void SetInteractBehaviour(InteractBehaviour ib)
    {
        if (_ib != null)
        {
            _ib.PointerEnterNeuron.Remove(Hover);
            _ib.PointerExitNeuron.Remove(UnHover);
        }

        _ib = ib;

        if (_ib != null)
        {
            _ib.PointerEnterNeuron.Join(Hover);
            _ib.PointerExitNeuron.Join(UnHover);
        }
    }

    private Tween _handle;
    
    private void Hover(InteractBehaviour ib, PointerEventData d)
    {
        _handle?.Kill();
        _handle = HoverImage.DOFade(1, 0.15f).SetEase(Ease.OutQuad);
        _handle.SetAutoKill().Restart();
    }

    private void UnHover(InteractBehaviour ib, PointerEventData d)
    {
        _handle?.Kill();
        _handle = HoverImage.DOFade(0, 0.15f).SetEase(Ease.OutQuad);
        _handle.SetAutoKill().Restart();
    }
}
