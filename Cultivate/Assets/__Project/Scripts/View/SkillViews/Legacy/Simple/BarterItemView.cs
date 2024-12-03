
using System;

public class BarterItemView : LegacySimpleView
{
    public LegacySimpleView PlayerSkillView;
    public LegacySimpleView SkillView;
    public GlowingButton ExchangeButton;

    // TODO: use Neuron
    public event Action<BarterItem> ExchangeEvent;
    public void ClearExchangeEvent() => ExchangeEvent = null;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        PlayerSkillView.SetAddress(GetAddress().Append(".FromSkill"));
        SkillView.SetAddress(GetAddress().Append(".ToSkill"));

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

        PlayerSkillView.Refresh();
        SkillView.Refresh();
        ExchangeButton.SetInteractable(barterItem.Affordable());
    }

    private void Exchange()
    {
        BarterItem barterItem = Get<BarterItem>();
        ExchangeEvent?.Invoke(barterItem);
        CanvasManager.Instance.RunCanvas.Refresh();
    }
}
