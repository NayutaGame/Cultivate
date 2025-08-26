
using System;
using UnityEngine.EventSystems;

public class BarterItemView : XView
{
    public SkillView LeftSkillView;
    public SkillView RightSkillView;
    public ExchangeButton ExchangeButton;

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

        ExchangeButton.OnClickNeuron.Join(Exchange);
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

        bool interactable = barterItem.Affordable();
        ExchangeButton.ExchangeButtonState state = ExchangeButton.GetState();
        ExchangeButton.SetState(interactable ? state : ExchangeButton.ExchangeButtonState.Inactive);
    }

    private void Exchange(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.CloseAnnotation();
        Get<BarterItem>().Exchange();
    }
}
