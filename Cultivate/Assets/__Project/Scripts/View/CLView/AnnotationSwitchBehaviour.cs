
public class AnnotationSwitchBehaviour : XBehaviour
{
    private AnnotationBehaviour _annotationBehaviour;

    public override void Init(XView view)
    {
        base.Init(view);

        _annotationBehaviour = GetComponent<AnnotationBehaviour>();

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = View.GetInteractBehaviour();
        if (ib == null)
            return;

        AnnotationView annotationView = _annotationBehaviour.GetAnnotationView();
        SimpleView simpleView = annotationView.GetSimpleView();

        if (simpleView is FormationAnnotationView formationAnnotationView)
        {
            ib.RightClickNeuron.Join(formationAnnotationView.SwitchShowingJingJie);
        }
    }
}
