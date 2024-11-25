
public class LegacyAnnotationSwitchBehaviour : LegacyBehaviour
{
    private LegacyAnnotationBehaviour _legacyAnnotationBehaviour;

    public override void Init(LegacyView view)
    {
        base.Init(view);

        _legacyAnnotationBehaviour = GetComponent<LegacyAnnotationBehaviour>();

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        LegacyInteractBehaviour ib = View.GetInteractBehaviour();
        if (ib == null)
            return;

        AnnotationView annotationView = _legacyAnnotationBehaviour.GetAnnotationView();
        LegacySimpleView simpleView = annotationView.GetSimpleView();

        if (simpleView is FormationAnnotationView formationAnnotationView)
        {
            ib.RightClickNeuron.Join(formationAnnotationView.SwitchShowingJingJie);
        }
    }
}
