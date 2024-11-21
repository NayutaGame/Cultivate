
public class XBehaviourAnnotationSwitch : XBehaviour
{
    private XBehaviourAnnotation _xBehaviourAnnotation;

    public override void AwakeFunction(XView view)
    {
        base.AwakeFunction(view);

        _xBehaviourAnnotation = GetComponent<XBehaviourAnnotation>();

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = View.GetInteractBehaviour();
        if (ib == null)
            return;

        AnnotationView annotationView = _xBehaviourAnnotation.GetAnnotationView();
        XView simpleView = annotationView.GetSimpleView();

        if (simpleView is FormationAnnotationView formationAnnotationView)
        {
            ib.RightClickNeuron.Join(formationAnnotationView.SwitchShowingJingJie);
        }
    }
}
