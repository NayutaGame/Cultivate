
using UnityEngine;

public class SlotView : AddressBehaviour
{
    [SerializeField] protected SkillView SkillView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SkillView.SetAddress(GetAddress().Append(".Skill"));
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillSlot slot = Get<SkillSlot>();

        bool locked = slot.State == SkillSlot.SkillSlotState.Locked;
        gameObject.SetActive(!locked);
        if (locked)
            return;

        bool occupied = slot.State == SkillSlot.SkillSlotState.Occupied;
        SkillView.gameObject.SetActive(occupied);
        if (!occupied)
            return;

        SkillView.Refresh();
        SkillView.SetManaCostState(slot.ManaIndicator.State);
    }

    public bool IsSelected()
    {
        if (SkillView == null)
            return false;
        return SkillView.IsSelected();
    }

    public void SetSelected(bool selected)
    {
        if (SkillView != null)
            SkillView.SetSelected(selected);
    }
}
