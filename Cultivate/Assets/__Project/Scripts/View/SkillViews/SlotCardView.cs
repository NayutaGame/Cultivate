using UnityEngine;

public class SlotCardView : LegacySimpleView
{
    [SerializeField] public SkillCardView SkillCardView;

    public override void AwakeFunction()
    {
        SkillCardView.AwakeFunction();
        base.AwakeFunction();
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

        bool occupied = slot.IsOccupied;
        SkillCardView.gameObject.SetActive(occupied);
        if (!occupied)
            return;

        SkillCardView.Refresh();
        // SkillCardView.SetManaCostState(slot.ManaIndicator.State);
    }
}
