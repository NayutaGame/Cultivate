
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
        CommodityListView.PointerEnterNeuron.Join(CanvasManager.Instance.SkillAnnotation.SetAddressFromIB);
        CommodityListView.PointerExitNeuron.Join(CanvasManager.Instance.SkillAnnotation.SetAddressToNull);
        CommodityListView.PointerMoveNeuron.Join(CanvasManager.Instance.SkillAnnotation.UpdateMousePos);
        CommodityListView.LeftClickNeuron.Join(BuySkill);

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    public override void Refresh()
    {
        base.Refresh();
        CommodityListView.Refresh();
    }

    private void BuySkill(InteractBehaviour ib, PointerEventData eventData)
        => BuySkill(ib);

    private bool BuySkill(InteractBehaviour ib)
    {
        Commodity commodity = ib.ComplexView.AddressBehaviour.Get<Commodity>();

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
