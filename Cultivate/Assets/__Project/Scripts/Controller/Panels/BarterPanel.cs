
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarterPanel : Panel
{
    public LegacyListView BarterItemListView;

    public Button ExitButton;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        BarterItemListView.SetAddress(_address.Append(".Inventory"));

        foreach (LegacyItemBehaviour itemBehaviour in BarterItemListView.ActivePool)
        {
            BarterItemView barterItemView = itemBehaviour.GetSimpleView() as BarterItemView;
            barterItemView.ClearExchangeEvent();
            barterItemView.ExchangeEvent += ExchangeEvent;
        }

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    public override void Refresh()
    {
        base.Refresh();
        BarterItemListView.Refresh();
    }

    private void ExchangeEvent(BarterItem barterItem)
    {
        BarterPanelDescriptor d = _address.Get<BarterPanelDescriptor>();
        d.Exchange(barterItem);
        // AudioManager.Instance.Play("钱币");
    }

    private void Exit()
    {
        Signal signal = new ExitShopSignal();
        CanvasManager.Instance.RunCanvas.LegacySetPanelSAsyncFromSignal(signal);
    }

    private void PlayCardHoverSFX(LegacyInteractBehaviour ib, PointerEventData d)
        => AudioManager.Play("CardHover");
}
