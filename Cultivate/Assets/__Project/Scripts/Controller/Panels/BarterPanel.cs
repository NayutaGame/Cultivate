
using UnityEngine.UI;

public class BarterPanel : Panel
{
    public ListView BarterItemListView;

    public Button ExitButton;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        BarterItemListView.SetAddress(_address.Append(".Inventory"));
        BarterItemListView.PointerEnterNeuron.Set((ib, d) => ((StandardSkillInteractBehaviour)ib).HoverAnimation(d));
        BarterItemListView.PointerExitNeuron.Set((ib, d) => ((StandardSkillInteractBehaviour)ib).UnhoverAnimation(d));
        BarterItemListView.PointerMoveNeuron.Set((ib, d) => ((StandardSkillInteractBehaviour)ib).PointerMove(d));

        foreach (ItemView itemView in BarterItemListView.ActivePool)
        {
            BarterItemView barterItemView = itemView.AddressBehaviour as BarterItemView;
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
        Map map = new Address("Run.Environment.Map").Get<Map>();
        PanelDescriptor panelDescriptor = map.ReceiveSignal(new Signal());
        CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
    }
}
