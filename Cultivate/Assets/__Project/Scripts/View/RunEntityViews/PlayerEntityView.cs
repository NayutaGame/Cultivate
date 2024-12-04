
using UnityEngine.EventSystems;

public class PlayerEntityView : LegacySimpleView
{
    public ListView FieldView;
    public LegacyListView FormationList;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        FieldView.SetAddress(GetAddress().Append(".Slots"));
        // FieldView.PointerEnterNeuron.Join(PlayCardHoverSFX);
        FieldView.DropNeuron.Join(Equip, Swap);
        
        FormationList.SetAddress(GetAddress().Append(".ShowingFormations"));
    }

    public override void Refresh()
    {
        base.Refresh();

        // SkillList.Refresh();
        FormationList.Refresh();
    }

    public void Sync()
    {
        // SkillList.Sync();
        FormationList.Sync();
    }

    #region IInteractable

    private void PlayCardHoverSFX(LegacyInteractBehaviour ib, PointerEventData d)
        => AudioManager.Play("CardHover");

    private void Equip(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;

        EquipDetails equipDetails = new(from.Get<RunSkill>(), to.Get<SkillSlot>());
        CanvasManager.Instance.RunCanvas.EquipEvent.Invoke(equipDetails);
    }

    private void Swap(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is FieldSlotInteractBehaviour))
            return;

        SwapDetails swapDetails = new(from.Get<SkillSlot>(), to.Get<SkillSlot>());
        CanvasManager.Instance.RunCanvas.SwapEvent.Invoke(swapDetails);
    }

    #endregion
}
