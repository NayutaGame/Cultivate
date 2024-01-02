
public class BattleEntityView : AddressBehaviour
{
    public ListView FieldView;
    public ListView FormationListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        FieldView.SetAddress(GetAddress().Append(".Slots"));
        FieldView.PointerEnterNeuron.Set((ib, d)
            => ((FieldSlotInteractBehaviour)ib).HoverAnimation(ib, d));
        FieldView.PointerExitNeuron.Set(CanvasManager.Instance.SkillAnnotation.SetAddressToNull);
        FieldView.PointerMoveNeuron.Set(CanvasManager.Instance.SkillAnnotation.UpdateMousePos);

        FormationListView.SetAddress(GetAddress().Append(".ActivatedSubFormations"));
        FormationListView.PointerEnterNeuron.Set(CanvasManager.Instance.FormationAnnotation.SetAddressFromIB);
        FormationListView.PointerExitNeuron.Set(CanvasManager.Instance.FormationAnnotation.SetAddressToNull);
        FormationListView.PointerMoveNeuron.Set(CanvasManager.Instance.FormationAnnotation.UpdateMousePos);
    }

    public override void Refresh()
    {
        base.Refresh();
        FieldView.Refresh();
        FormationListView.Refresh();
    }
}
