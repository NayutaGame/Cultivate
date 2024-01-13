
public class ExtraBehaviourAnnotation : ExtraBehaviour
{
    public string AnnotationAddress;
    private AnnotationView Annotation;

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

        ib.PointerEnterNeuron.Add(Annotation.SetAddressFromIB);
        ib.PointerExitNeuron.Add(Annotation.SetAddressToNull);
        ib.PointerMoveNeuron.Add(Annotation.UpdateMousePos);
        ib.BeginDragNeuron.Add(Annotation.SetAddressToNull);
    }
}
