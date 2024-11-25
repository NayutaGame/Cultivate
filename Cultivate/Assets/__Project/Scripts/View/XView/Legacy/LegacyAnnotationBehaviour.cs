
using UnityEngine;

public class LegacyAnnotationBehaviour : LegacyBehaviour
{
    public string AnnotationAddress;
    private AnnotationView Annotation;
    public AnnotationView GetAnnotationView() => Annotation;
    public RectTransform HoverTransform;

    public override void Init(LegacyView view)
    {
        base.Init(view);

        Annotation = new Address(AnnotationAddress).Get<AnnotationView>();

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        LegacyInteractBehaviour ib = View.GetInteractBehaviour();
        if (ib == null)
            return;

        ib.PointerEnterNeuron.Join(Annotation.PointerEnter);
        ib.PointerExitNeuron.Join(Annotation.PointerExit);
        ib.PointerMoveNeuron.Join(Annotation.PointerMove);
        ib.BeginDragNeuron.Join(Annotation.PointerExit);
    }
}
