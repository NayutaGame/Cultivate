using UnityEngine;

public class SkillSlotView : XView
{
    [SerializeField] public SkillView SkillView;

    protected override void AwakeFunction()
    {
        SkillView.CheckAwake();
        base.AwakeFunction();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SkillView.SetAddress(GetAddress());
    }

    public override void Refresh()
    {
        base.Refresh();

        SkillSlot slot = Get<SkillSlot>();

        bool occupied = slot.IsOccupied();
        SkillView.gameObject.SetActive(occupied);
        if (!occupied)
            return;

        SkillView.Refresh();
    }
}
