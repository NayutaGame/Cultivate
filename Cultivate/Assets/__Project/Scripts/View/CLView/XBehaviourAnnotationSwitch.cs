
public class XBehaviourAnnotationSwitch : XBehaviour
{
    private XBehaviourAnnotation _xBehaviourAnnotation;

    public override void Init(XView xView)
    {
        base.Init(xView);

        _xBehaviourAnnotation = GetComponent<XBehaviourAnnotation>();

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = XView.GetInteractBehaviour();
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
