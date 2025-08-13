
using UnityEngine;

public class RequirementContentView : XView
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
        SkillView.SetAddress(GetAddress().Append(".Skill"));
    }

    public override void Refresh()
    {
        base.Refresh();

        RequirementSlot slot = Get<RequirementSlot>();

        bool occupied = slot.IsOccupied();
        SkillView.gameObject.SetActive(occupied);
        if (!occupied)
            return;

        SkillView.Refresh();
    }
}