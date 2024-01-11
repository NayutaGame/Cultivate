using UnityEngine;

public class SlotSimpleView : SimpleView
{
    [SerializeField] protected SkillSimpleView SkillSimpleView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SkillSimpleView.SetAddress(GetAddress().Append(".Skill"));
    }

    public override void Refresh()
    {
        base.Refresh();

        SkillSlot slot = Get<SkillSlot>();

        bool occupied = slot.State == SkillSlot.SkillSlotState.Occupied;
        SkillSimpleView.gameObject.SetActive(occupied);
        if (!occupied)
            return;

        SkillSimpleView.Refresh();
        SkillSimpleView.SetManaCostState(slot.ManaIndicator.State);
    }
}
