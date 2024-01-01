
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopPanel : Panel
{
    public ListView CommodityListView;

    public Button ExitButton;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        ConfigureInteractDelegate();

        _address = new Address("Run.Environment.ActivePanel");
        CommodityListView.SetAddress(_address.Append(".Commodities"));
        CommodityListView.SetHandler(_interactHandler);

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    private InteractHandler _interactHandler;
    private void ConfigureInteractDelegate()
    {
        _interactHandler = new InteractHandler(1,
            getId: view => 0);

        _interactHandler.SetHandle(InteractHandler.POINTER_ENTER, 0, (v, d) => ((CommodityItemInteractBehaviour)v).SkillView.HoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_EXIT, 0, (v, d) => ((CommodityItemInteractBehaviour)v).SkillView.UnhoverAnimation(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_MOVE, 0, (v, d) => ((CommodityItemInteractBehaviour)v).SkillView.PointerMove(d));
        _interactHandler.SetHandle(InteractHandler.POINTER_LEFT_CLICK, 0, (v, d) => BuySkill(v, d));
    }

    public override void Refresh()
    {
        base.Refresh();
        CommodityListView.Refresh();
    }

    private bool BuySkill(InteractBehaviour interactBehaviour, PointerEventData eventData)
    {
        Commodity commodity = interactBehaviour.ComplexView.AddressBehaviour.Get<Commodity>();

        ShopPanelDescriptor d = _address.Get<ShopPanelDescriptor>();

        bool success = d.Buy(commodity);
        if (!success)
            return false;

        // AudioManager.Instance.Play("钱币");
        CanvasManager.Instance.RunCanvas.Refresh();

        return true;
    }

    private void Exit()
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.Map.ReceiveSignal(new Signal());
        CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
    }
}
