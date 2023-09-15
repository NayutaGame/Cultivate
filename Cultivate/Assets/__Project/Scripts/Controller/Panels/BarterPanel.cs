
using UnityEngine.UI;

public class BarterPanel : Panel
{
    public ListView BarterInventoryView;

    public Button ExitButton;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Battle.Map.CurrentNode.CurrentPanel");
        BarterInventoryView.SetAddress(_address.Append(".Inventory"));

        foreach (ItemView itemView in BarterInventoryView.Views)
        {
            BarterItemView barterItemView = (BarterItemView)itemView;
            barterItemView.ClearExchangeEvent();
            barterItemView.ExchangeEvent += ExchangeEvent;
        }

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    public override void Refresh()
    {
        base.Refresh();
        BarterInventoryView.Refresh();
    }

    private void ExchangeEvent(BarterItem barterItem)
    {
        BarterPanelDescriptor d = _address.Get<BarterPanelDescriptor>();
        d.Exchange(barterItem);
        AudioManager.Instance.Play("钱币");
    }

    private void Exit()
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Battle.Map.ReceiveSignal(new Signal());
        RunCanvas.Instance.SetNodeState(panelDescriptor);
    }
}
