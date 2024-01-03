
public class BattleEntityView : AddressBehaviour
{
    public ListView FieldView;
    public ListView FormationListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        FieldView.SetAddress(GetAddress().Append(".Slots"));
        FieldView.PointerEnterNeuron.Join((ib, d)
            => ((FieldSlotInteractBehaviour)ib).HoverAnimation(ib, d));
        FieldView.PointerExitNeuron.Join(CanvasManager.Instance.SkillAnnotation.SetAddressToNull);
        FieldView.PointerMoveNeuron.Join(CanvasManager.Instance.SkillAnnotation.UpdateMousePos);

        FormationListView.SetAddress(GetAddress().Append(".ActivatedSubFormations"));
        FormationListView.PointerEnterNeuron.Join(CanvasManager.Instance.FormationAnnotation.SetAddressFromIB);
        FormationListView.PointerExitNeuron.Join(CanvasManager.Instance.FormationAnnotation.SetAddressToNull);
        FormationListView.PointerMoveNeuron.Join(CanvasManager.Instance.FormationAnnotation.UpdateMousePos);
    }

    public override void Refresh()
    {
        base.Refresh();
        FieldView.Refresh();
        FormationListView.Refresh();
    }
}
