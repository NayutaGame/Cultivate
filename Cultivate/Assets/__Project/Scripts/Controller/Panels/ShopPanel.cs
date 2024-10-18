
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopPanel : Panel
{
    public ListView CommodityListView;
    public Image Illustration;
    public Button ExitButton;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        CommodityListView.SetAddress(_address.Append(".Commodities"));
        CommodityListView.PointerEnterNeuron.Join(PointerEnter);
        CommodityListView.PointerExitNeuron.Join(PointerExit);
        CommodityListView.PointerMoveNeuron.Join(PointerMove);
        CommodityListView.LeftClickNeuron.Join(BuySkill);

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
        => CanvasManager.Instance.SkillAnnotation.PointerEnter(ib, d, ib.GetSimpleView().GetAddress().Append(".Skill"));

    private void PointerExit(InteractBehaviour ib, PointerEventData d)
        => CanvasManager.Instance.SkillAnnotation.PointerExit(ib, d, ib.GetSimpleView().GetAddress().Append(".Skill"));

    private void PointerMove(InteractBehaviour ib, PointerEventData d)
        => CanvasManager.Instance.SkillAnnotation.PointerMove(ib, d, ib.GetSimpleView().GetAddress().Append(".Skill"));

    public override void Refresh()
    {
        base.Refresh();
        CommodityListView.Refresh();

        ShopPanelDescriptor pd = _address.Get<ShopPanelDescriptor>();
        Illustration.sprite = pd.GetSprite().Sprite;
    }

    private void BuySkill(InteractBehaviour ib, PointerEventData eventData)
        => BuySkill(ib.GetSimpleView());

    private bool BuySkill(SimpleView v)
    {
        Commodity commodity = v.Get<Commodity>();

        ShopPanelDescriptor d = _address.Get<ShopPanelDescriptor>();

        bool success = d.Buy(commodity);
        if (!success)
            return false;

        BuyStaging();
        CanvasManager.Instance.RunCanvas.Refresh();
        return true;
    }

    private void BuyStaging()
    {
        // AudioManager.Instance.Play("钱币");
    }

    private void Exit()
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(new ExitShopSignal());
        PanelS panelS = PanelS.FromPanelDescriptor(panelDescriptor);
        CanvasManager.Instance.RunCanvas.SetPanelSAsync(panelS);
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");
}
