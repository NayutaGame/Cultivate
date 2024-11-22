
public class XBehaviourAnnotationSwitch : XBehaviour
{
    private XBehaviourAnnotation _xBehaviourAnnotation;

    public override void Init(XView view)
    {
        base.Init(view);

        _xBehaviourAnnotation = GetComponent<XBehaviourAnnotation>();

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = View.GetInteractBehaviour();
        if (ib == null)
            return;

        AnnotationView annotationView = _xBehaviourAnnotation.GetAnnotationView();
        SimpleView simpleView = annotationView.GetSimpleView();

        if (simpleView is FormationAnnotationView formationAnnotationView)
        {
            ib.RightClickNeuron.Join(formationAnnotationView.SwitchShowingJingJie);
        }
    }
}
