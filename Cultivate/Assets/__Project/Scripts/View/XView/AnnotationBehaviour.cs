
using UnityEngine;
using UnityEngine.EventSystems;

public class AnnotationBehaviour : XBehaviour
{
    [SerializeField] private InteractBehaviour _ib;
    
    public string AnnotationAddress;
    private AnnotationView _annotation;
    public AnnotationView GetAnnotationView() => _annotation;
    public RectTransform HoverTransform;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _annotation = new Address(AnnotationAddress).Get<AnnotationView>();
        SetInteractBehaviour(_ib);
    }

    public override void SetInteractBehaviour(InteractBehaviour ib)
    {
        if (_ib != null)
        {
            _ib.PointerEnterNeuron.Remove(PointerEnter);
            _ib.PointerExitNeuron.Remove(_annotation.PointerExit);
            _ib.PointerMoveNeuron.Remove(_annotation.PointerMove);
            _ib.BeginDragNeuron.Remove(_annotation.PointerExit);
        }

        _ib = ib;
        if (_ib != null)
        {
            _ib.PointerEnterNeuron.Join(PointerEnter);
            _ib.PointerExitNeuron.Join(_annotation.PointerExit);
            _ib.PointerMoveNeuron.Join(_annotation.PointerMove);
            _ib.BeginDragNeuron.Join(_annotation.PointerExit);
        }
    }

    public void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        RectTransform rect = ib.GetView().GetRect();
        RectTransform hoverRect = HoverTransform;
        _annotation.PointerEnter(rect, hoverRect, ib.GetAddress());
    }
}
