
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopPanel : Panel
{
    public LegacyListView CommodityListView;
    public Image Illustration;
    public Button ExitButton;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        CommodityListView.SetAddress(_address.Append(".Commodities"));
        // CommodityListView.PointerEnterNeuron.Join(PointerEnter);
        // CommodityListView.PointerExitNeuron.Join(PointerExit);
        // CommodityListView.PointerMoveNeuron.Join(PointerMove);
        CommodityListView.LeftClickNeuron.Join(BuySkill);

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    // private void PointerEnter(LegacyInteractBehaviour ib, PointerEventData d)
    //     => CanvasManager.Instance.SkillAnnotation.PointerEnter(ib, d, ib.GetSimpleView().GetAddress().Append(".Skill"));
    //
    // private void PointerExit(LegacyInteractBehaviour ib, PointerEventData d)
    //     => CanvasManager.Instance.SkillAnnotation.PointerExit(ib, d, ib.GetSimpleView().GetAddress().Append(".Skill"));
    //
    // private void PointerMove(LegacyInteractBehaviour ib, PointerEventData d)
    //     => CanvasManager.Instance.SkillAnnotation.PointerMove(ib, d, ib.GetSimpleView().GetAddress().Append(".Skill"));

    public override void Refresh()
    {
        base.Refresh();
        CommodityListView.Refresh();

        ShopPanelDescriptor pd = _address.Get<ShopPanelDescriptor>();
        Illustration.sprite = pd.GetSprite().Sprite;
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.BuySkillNeuron.Add(BuySkillStaging);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.BuySkillNeuron.Remove(BuySkillStaging);
    }

    private void BuySkill(LegacyInteractBehaviour ib, PointerEventData eventData)
        => BuySkill(ib.GetSimpleView());

    private bool BuySkill(LegacySimpleView v)
    {
        Commodity commodity = v.Get<Commodity>();

        ShopPanelDescriptor d = _address.Get<ShopPanelDescriptor>();
        return d.Buy(commodity);
    }

    private void BuySkillStaging(BuySkillDetails d)
    {
        LegacyInteractBehaviour commodityIB = CommodityListView.InactivePools[0][^1].GetInteractBehaviour();
        LegacyInteractBehaviour cardIB = CanvasManager.Instance.RunCanvas.SkillInteractBehaviourFromDeckIndex(d.DeckIndex);

        CanvasManager.Instance.RunCanvas.BuySkillStaging(cardIB, commodityIB);
        CanvasManager.Instance.SkillAnnotation.PointerExit();
        
        // ExtraBehaviourPivot extraBehaviourPivot = commodityIB.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        // if (extraBehaviourPivot != null)
        //     extraBehaviourPivot.Disappear();
    }

    private void Exit()
    {
        Signal signal = new ExitShopSignal();
        CanvasManager.Instance.RunCanvas.SetPanelSAsyncFromSignal(signal);
    }

    private void PlayCardHoverSFX(LegacyInteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");
}
