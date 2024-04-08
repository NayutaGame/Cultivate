
using UnityEngine;

public class ExtraBehaviourAnnotation : ExtraBehaviour
{
    public string AnnotationAddress;
    private AnnotationView Annotation;
    public AnnotationView GetAnnotationView() => Annotation;
    public RectTransform HoverTransform;

    public override void Init(CLView clView)
    {
        base.Init(clView);

        Annotation = new Address(AnnotationAddress).Get<AnnotationView>();

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = CLView.GetInteractBehaviour();
        if (ib == null)
            return;

        ib.PointerEnterNeuron.Join(Annotation.PointerEnter);
        ib.PointerExitNeuron.Join(Annotation.PointerExit);
        ib.PointerMoveNeuron.Join(Annotation.PointerMove);
        ib.BeginDragNeuron.Join(Annotation.PointerExit);
    }
}
