
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

        AnnotationView annotationView = _annotationBehaviour.GetAnnotationView();
        FormationAnnotationView formationAnnotationView = annotationView.GetView() as FormationAnnotationView;
        ib.RightClickNeuron.Join(formationAnnotationView.SwitchShowingJingJie);
    }
}
