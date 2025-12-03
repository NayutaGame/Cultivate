
using System;
using UnityEngine.EventSystems;

public class BarterItemView : XView
{
    public SkillView LeftSkillView;
    public SkillView RightSkillView;
    public Button4State ExchangeButton;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        LeftSkillView.CheckAwake();
        RightSkillView.CheckAwake();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        LeftSkillView.SetAddress(GetAddress().Append(".FromSkill"));
        RightSkillView.SetAddress(GetAddress().Append(".ToSkill"));

        ExchangeButton.LeftClickNeuron.Join(Exchange);
    }

    public override void Refresh()
    {
        base.Refresh();
        BarterItem barterItem = Get<BarterItem>();

        bool isReveal = barterItem != null;
        gameObject.SetActive(isReveal);
        if (!isReveal)
            return;

        LeftSkillView.Refresh();
        RightSkillView.Refresh();

        ExchangeButton.SetStateToActiveIf(barterItem.Affordable());
    }

    private void Exchange(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.CloseAnnotation();
        Get<BarterItem>().Exchange();
    }
}
