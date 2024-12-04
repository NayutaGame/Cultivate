
using UnityEngine;

public class AnnotationBehaviour : XBehaviour
{
    public string AnnotationAddress;
    private AnnotationView _annotation;
    public RectTransform HoverTransform;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _annotation = new Address(AnnotationAddress).Get<AnnotationView>();
        BindInteractBehaviour();
    }

    private void BindInteractBehaviour()
    {
        InteractBehaviour ib = GetInteractBehaviour();
        if (ib == null)
            return;

        ib.PointerEnterNeuron.Join(_annotation.PointerEnter);
        ib.PointerExitNeuron.Join(_annotation.PointerExit);
        ib.PointerMoveNeuron.Join(_annotation.PointerMove);
        ib.BeginDragNeuron.Join(_annotation.PointerExit);
    }
}
