
using UnityEngine;
using UnityEngine.UI;

public class ConstraintView : XView
{
    [SerializeField] public PackView PackView;
    [SerializeField] public Image SlotImage;

    protected override void AwakeFunction()
    {
        PackView.CheckAwake();
        base.AwakeFunction();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        PackView.SetAddress(GetAddress().Append(".Pack"));
    }

    public override void Refresh()
    {
        base.Refresh();

        PackConstraint constraint = Get<PackConstraint>();

        if (SlotImage != null)
            SlotImage.sprite = constraint.GetConstraintSprite();
        
        bool occupied = !constraint.IsEmpty;
        PackView.gameObject.SetActive(occupied);
        if (!occupied)
            return;
        
        PackView.Refresh();
        PackView.RefreshFromParentedConstraint(constraint);
    }
}
