
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopPanel : Panel
{
    public ListView CommodityListView;
    public Image Illustration;
    public Button ExitButton;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        CommodityListView.SetAddress(_address.Append(".Commodities"));
        CommodityListView.LeftClickNeuron.Join(BuySkill);

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(RunManager.Instance.Environment.ExitShopProcedure);
    }

    public override void Refresh()
    {
        base.Refresh();
        CommodityListView.Refresh();

        ShopPanelDescriptor pd = _address.Get<ShopPanelDescriptor>();
        Illustration.sprite = pd.GetSprite().Sprite;
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.BuySkillNeuron.Add(CanvasManager.Instance.RunCanvas.BuySkillStaging);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.BuySkillNeuron.Remove(CanvasManager.Instance.RunCanvas.BuySkillStaging);
    }
    
    private void BuySkill(InteractBehaviour ib, PointerEventData eventData)
    {
        CanvasManager.Instance.SkillAnnotation.PointerExit();
        
        ShopPanelDescriptor shopPanelDescriptor = _address.Get<ShopPanelDescriptor>();
        Commodity commodity = ib.Get<Commodity>();
        int commodityIndex = CommodityListView.IndexFromView(ib.GetView()).Value;
        BuySkillDetails details = new(commodity, commodityIndex);
        shopPanelDescriptor.BuySkillProcedure(details);
    }

    public XView CommodityItemFromIndex(int commodityIndex)
    {
        return CommodityListView.ViewFromIndex(commodityIndex);
    }

    private void PlayCardHoverSFX(LegacyInteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");
}
