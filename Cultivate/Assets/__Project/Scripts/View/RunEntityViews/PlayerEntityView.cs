
using UnityEngine.EventSystems;

public class PlayerEntityView : XView
{
    public ListView FieldView;
    public ListView FormationList;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        FieldView.SetAddress(GetAddress().Append(".Slots"));
        FieldView.DropNeuron.Join(MoveSkill);
        
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

    private void MoveSkill(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        IDeckIndex fromIndex = GetDeckIndex(from);
        IDeckIndex toIndex = GetDeckIndex(to);
        if (fromIndex == null || toIndex == null)
            return;

        RunManager.Instance.Environment.MoveSkillProcedure(fromIndex, toIndex);
    }

    private IDeckIndex GetDeckIndex(InteractBehaviour ib)
    {
        object obj = ib.Get<object>();
        if (obj is RunSkill runSkill)
            return runSkill.ToDeckIndex();
        
        if (obj is SkillSlot skillSlot)
            return skillSlot.ToDeckIndex();
        
        if (obj is RequirementSlot requirementSlot)
            return requirementSlot.ToDeckIndex();

        return null;
    }

    #endregion
}
