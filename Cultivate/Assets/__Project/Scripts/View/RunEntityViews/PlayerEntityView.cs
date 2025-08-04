
using UnityEngine.EventSystems;

public class PlayerEntityView : XView
{
    public ListView FieldView;
    public ListView FormationList;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        FieldView.SetAddress(GetAddress().Append(".Slots"));
        FieldView.DropNeuron.Join(Equip, Swap);
        
        FormationList.SetAddress(GetAddress().Append(".ShowingFormations"));
    }

    private void OnEnable()
    {
        FieldView.Sync();
        FormationList.Sync();
    }

    public override void Refresh()
    {
        base.Refresh();

        // SkillList.Refresh();
        FormationList.Refresh();
    }

    public void Sync()
    {
        FieldView.Sync();
        FormationList.Sync();
    }

    public void OnFieldChange()
    {
        FieldView.Sync();
        FormationList.Refresh();
    }

    #region IInteractable

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
    {
        SkillSlot slot = ib.Get<SkillSlot>();
        if (slot.Skill != null)
            AudioManager.Play("CardHover");
    }

    private void Equip(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        RunSkill skill = from.Get<RunSkill>();
        SkillSlot slot = to.Get<SkillSlot>();
        if (skill == null || slot == null)
            return;
        
        RunManager.Instance.Environment.TryEquipProcedure(skill, slot);
    }

    private void Swap(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        SkillSlot fromSlot = from.Get<SkillSlot>();
        SkillSlot toSlot = to.Get<SkillSlot>();
        if (fromSlot == null || toSlot == null)
            return;
        
        RunManager.Instance.Environment.TrySwapProcedure(fromSlot, toSlot);
    }

    #endregion
}
