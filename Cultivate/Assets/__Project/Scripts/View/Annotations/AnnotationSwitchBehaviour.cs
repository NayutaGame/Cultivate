
public class AnnotationSwitchBehaviour : XBehaviour
{
    private AnnotationBehaviour _annotationBehaviour;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        _annotationBehaviour = GetBehaviour<AnnotationBehaviour>();
        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = GetInteractBehaviour();
        if (ib == null)
            return;

        LegacyAnnotationView annotationView = _annotationBehaviour.GetAnnotationView();
        LegacyFormationAnnotationView formationAnnotationView = annotationView.GetView() as LegacyFormationAnnotationView;
        ib.RightClickNeuron.Join(formationAnnotationView.SwitchShowingJingJie);
    }
}
