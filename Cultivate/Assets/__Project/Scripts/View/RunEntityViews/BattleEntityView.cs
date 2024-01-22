
public class BattleEntityView : SimpleView
{
    public ListView FieldView;
    public ListView FormationListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        FieldView.SetAddress(GetAddress().Append(".Slots"));
        FieldView.PointerEnterNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerEnter);
        FieldView.PointerExitNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit);
        FieldView.PointerMoveNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerMove);

        FormationListView.SetAddress(GetAddress().Append(".RunFormations"));
        FormationListView.PointerEnterNeuron.Join(CanvasManager.Instance.FormationAnnotation.PointerEnter);
        FormationListView.PointerExitNeuron.Join(CanvasManager.Instance.FormationAnnotation.PointerExit);
        FormationListView.PointerMoveNeuron.Join(CanvasManager.Instance.FormationAnnotation.PointerMove);
    }

    public override void Refresh()
    {
        base.Refresh();
        FieldView.Refresh();
        FormationListView.Refresh();
    }
}
