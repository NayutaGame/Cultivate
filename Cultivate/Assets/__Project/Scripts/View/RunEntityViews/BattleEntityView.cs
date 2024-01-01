
using UnityEngine.EventSystems;

public class BattleEntityView : LegacyAddressBehaviour
{
    public ListView FieldView;
    public ListView FormationListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        FieldView.SetAddress(GetAddress().Append(".Slots"));
        FieldView.PointerEnterNeuron.Set((ib, d) => ((FieldSlotInteractBehaviour)ib).HoverAnimation(d));
        FieldView.PointerExitNeuron.Set((ib, d) => ((FieldSlotInteractBehaviour)ib).UnhoverAnimation(d));
        FieldView.PointerMoveNeuron.Set((ib, d) => ((FieldSlotInteractBehaviour)ib).PointerMove(d));

        FormationListView.SetAddress(GetAddress().Append(".ActivatedSubFormations"));
        FormationListView.PointerEnterNeuron.Set(PointerEnterFormation);
        FormationListView.PointerExitNeuron.Set(PointerExitFormation);
        FormationListView.PointerMoveNeuron.Set(PointerMoveFormation);
    }

    public override void Refresh()
    {
        base.Refresh();
        FieldView.Refresh();
        FormationListView.Refresh();
    }

    private void PointerEnterFormation(InteractBehaviour ib, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationAnnotation.SetAddressFromIB(ib, eventData);
    }

    private void PointerExitFormation(InteractBehaviour ib, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationAnnotation.SetAddressToNull(ib, eventData);
    }

    private void PointerMoveFormation(InteractBehaviour ib, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationAnnotation.UpdateMousePos(eventData.position);
    }
}
