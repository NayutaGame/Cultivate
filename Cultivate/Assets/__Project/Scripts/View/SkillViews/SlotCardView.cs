using UnityEngine;

public class SlotCardView : SimpleView
{
    [SerializeField] protected SkillCardView SkillCardView;

    public override void Awake()
    {
        SkillCardView.Awake();
        base.Awake();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SkillCardView.SetAddress(GetAddress().Append(".Skill"));
    }

    public override void Refresh()
    {
        base.Refresh();

        SkillSlot slot = Get<SkillSlot>();

        bool occupied = slot.State == SkillSlot.SkillSlotState.Occupied;
        SkillCardView.gameObject.SetActive(occupied);
        if (!occupied)
            return;

        SkillCardView.Refresh();
        SkillCardView.SetManaCostState(slot.ManaIndicator.State);
    }
}
