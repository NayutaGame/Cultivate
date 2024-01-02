
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

        _address = new Address("Run.Environment.ActivePanel");
        CommodityListView.SetAddress(_address.Append(".Commodities"));
        CommodityListView.PointerEnterNeuron.Set(CanvasManager.Instance.SkillAnnotation.SetAddressFromIB);
        CommodityListView.PointerExitNeuron.Set(CanvasManager.Instance.SkillAnnotation.SetAddressToNull);
        CommodityListView.PointerMoveNeuron.Set(CanvasManager.Instance.SkillAnnotation.UpdateMousePos);
        CommodityListView.LeftClickNeuron.Set((ib, d) => BuySkill(ib, d));

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
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

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");
}
