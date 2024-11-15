
using UnityEngine;

public class XBehaviourAnnotation : XBehaviour
{
    public string AnnotationAddress;
    private AnnotationView Annotation;
    public AnnotationView GetAnnotationView() => Annotation;
    public RectTransform HoverTransform;

    public override void Init(XView xView)
    {
        base.Init(xView);

        Annotation = new Address(AnnotationAddress).Get<AnnotationView>();

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = XView.GetInteractBehaviour();
        if (ib == null)
            return;

        ib.PointerEnterNeuron.Join(Annotation.PointerEnter);
        ib.PointerExitNeuron.Join(Annotation.PointerExit);
        ib.PointerMoveNeuron.Join(Annotation.PointerMove);
        ib.BeginDragNeuron.Join(Annotation.PointerExit);
    }
}
