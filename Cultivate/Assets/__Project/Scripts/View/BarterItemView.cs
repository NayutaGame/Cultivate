
using System;
using UnityEngine.UI;

public class BarterItemView : ItemView, IInteractable
{
    public StandardSkillView PlayerSkillView;
    public StandardSkillView SkillView;
    public Button ExchangeButton;

    public event Action<BarterItem> ExchangeEvent;
    public void ClearExchangeEvent() => ExchangeEvent = null;

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;

    public void SetDelegate(InteractDelegate interactDelegate)
    {
        InteractDelegate = interactDelegate;
        PlayerSkillView.SetDelegate(InteractDelegate);
        SkillView.SetDelegate(InteractDelegate);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        PlayerSkillView.SetAddress(GetAddress().Append(".PlayerSkill"));
        SkillView.SetAddress(GetAddress().Append(".Skill"));

        ExchangeButton.onClick.RemoveAllListeners();
        ExchangeButton.onClick.AddListener(Exchange);
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
        ExchangeButton.interactable = barterItem.Affordable();
    }

    private void Exchange()
    {
        BarterItem barterItem = Get<BarterItem>();
        ExchangeEvent?.Invoke(barterItem);
        CanvasManager.Instance.RunCanvas.Refresh();
    }
}
