using UnityEngine;

public class SlotAddressBehaviour : AddressBehaviour
{
    [SerializeField] protected SkillAddressBehaviour SkillAddressBehaviour;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SkillAddressBehaviour.SetAddress(GetAddress().Append(".Skill"));
    }

    public override void Refresh()
    {
        base.Refresh();

        SkillSlot slot = Get<SkillSlot>();

        bool occupied = slot.State == SkillSlot.SkillSlotState.Occupied;
        SkillAddressBehaviour.gameObject.SetActive(occupied);
        if (!occupied)
            return;

        SkillAddressBehaviour.Refresh();
        SkillAddressBehaviour.SetManaCostState(slot.ManaIndicator.State);
    }
}
