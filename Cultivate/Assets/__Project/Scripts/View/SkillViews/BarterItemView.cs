
using CLLibrary;

public class BarterItemView : XView
{
    public SkillView LeftSkillView;
    public SkillView RightSkillView;
    public GlowingButton ExchangeButton;

    public Neuron<BarterItem> ExchangeSkillEvent = new();

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
        ExchangeButton.SetInteractable(barterItem.Affordable());
    }

    private void Exchange()
    {
        BarterItem barterItem = Get<BarterItem>();
        ExchangeSkillEvent.Invoke(barterItem);
    }
}
