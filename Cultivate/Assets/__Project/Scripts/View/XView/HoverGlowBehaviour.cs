
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverGlowBehaviour : XBehaviour
{
    [SerializeField] private InteractBehaviour _ib;
    
    [SerializeField] private Image HoverImage;
    
    private Tween _handle;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        SetInteractBehaviour(_ib);
    }

    public override void SetInteractBehaviour(InteractBehaviour ib)
    {
        if (_ib != null)
        {
            _ib.NeuronBundle.PointerEnterNeuron.Remove(Hover);
            _ib.NeuronBundle.PointerExitNeuron.Remove(UnHover);
        }

        _ib = ib;

        if (_ib != null)
        {
            _ib.NeuronBundle.PointerEnterNeuron.Join(Hover);
            _ib.NeuronBundle.PointerExitNeuron.Join(UnHover);
        }
    }
    
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
