
using UnityEngine.EventSystems;

public class PlayerEntityView : XView
{
    public ListView FieldView;
    public ListView FormationList;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        FieldView.SetAddress(GetAddress().Append(".Slots"));
        FieldView.PointerEnterNeuron.Join(PlayCardHoverSFX);
        FieldView.DropNeuron.Join(Equip, Swap);
        
        FormationList.SetAddress(GetAddress().Append(".ShowingFormations"));
    }

    public override void Refresh()
    {
        base.Refresh();

        // SkillList.Refresh();
        FormationList.Refresh();
    }

    public void RefreshField()
    {
        FieldView.Sync();
        FormationList.Sync();
    }

    public void Sync()
    {
        FieldView.Refresh();
        FormationList.Sync();
    }

    #region IInteractable

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
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

        SkillSlot fromSkillSlot = from.Get<SkillSlot>();
        if (fromSkillSlot.Skill == null)
            return;

        SwapDetails swapDetails = new(fromSkillSlot, to.Get<SkillSlot>());
        CanvasManager.Instance.RunCanvas.SwapEvent.Invoke(swapDetails);
    }

    #endregion
}
