
using UnityEngine.UI;

public class BarterPanel : Panel
{
    public ListView BarterItemListView;

    public Button ExitButton;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        ConfigureInteractDelegate();

        _address = new Address("Run.Environment.ActivePanel");
        BarterItemListView.SetAddress(_address.Append(".Inventory"));
        BarterItemListView.SetHandler(_interactHandler);

        foreach (AddressBehaviour itemView in BarterItemListView.ActivePool)
        {
            BarterItemView barterItemView = (BarterItemView)itemView;
            barterItemView.ClearExchangeEvent();
            barterItemView.ExchangeEvent += ExchangeEvent;
        }

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    private InteractHandler _interactHandler;
    private void ConfigureInteractDelegate()
    {
        _interactHandler = new InteractHandler(1,
            getId: view => 0
        );

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 0, (v, d) => ((StandardSkillInteractBehaviour)v).HoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 0, (v, d) => ((StandardSkillInteractBehaviour)v).UnhoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 0, (v, d) => ((StandardSkillInteractBehaviour)v).PointerMove(d));
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
