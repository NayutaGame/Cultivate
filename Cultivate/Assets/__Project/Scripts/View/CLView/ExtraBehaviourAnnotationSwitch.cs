
public class ExtraBehaviourAnnotationSwitch : ExtraBehaviour
{
    private ExtraBehaviourAnnotation ExtraBehaviourAnnotation;

    public override void Init(CLView clView)
    {
        base.Init(clView);

        ExtraBehaviourAnnotation = GetComponent<ExtraBehaviourAnnotation>();

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = CLView.GetInteractBehaviour();
        if (ib == null)
            return;

        AnnotationView annotationView = ExtraBehaviourAnnotation.GetAnnotationView();
        SimpleView simpleView = annotationView.GetSimpleView();

        if (simpleView is FormationAnnotationView formationAnnotationView)
        {
            ib.RightClickNeuron.Join(formationAnnotationView.SwitchShowingJingJie);
        }
    }
}
