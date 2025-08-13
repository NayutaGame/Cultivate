
using UnityEngine;

public class RequirementSlotView : SlotView
{
    protected override void AwakeFunction()
    {
        // SlotView.CheckAwake();
        base.AwakeFunction();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        // SkillView.SetAddress(GetAddress().Append(".Skill"));
    }

    public override void Refresh()
    {
        base.Refresh();

        // SkillSlot slot = Get<SkillSlot>();
        //
        // bool occupied = slot.IsOccupied();
        // SkillView.gameObject.SetActive(occupied);
        // if (!occupied)
        //     return;
        //
        // SkillView.Refresh();
    }
}
