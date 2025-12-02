
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommodityItemView : XView
{
    public SkillView SkillView;
    public GameObject DiscountSign;

    public Button4StateSeperateContent PayWithGoldButton;
    public TMP_Text IdleGoldPriceText;
    public TMP_Text HoverGoldPriceText;
    public TMP_Text PressedGoldPriceText;
    public TMP_Text InactiveGoldPriceText;
    public Button4StateSeperateContent PayWithHealthButton;
    public TMP_Text IdleHealthPriceText;
    public TMP_Text HoverHealthPriceText;
    public TMP_Text PressedHealthPriceText;
    public TMP_Text InactiveHealthPriceText;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SkillView.SetAddress(GetAddress().Append(".Skill"));
        
        PayWithGoldButton.LeftClickNeuron.Join(PayWithGold);
        PayWithHealthButton.LeftClickNeuron.Join(PayWithHealth);
    }

    public override void Refresh()
    {
        base.Refresh();
        Commodity commodity = Get<Commodity>();
        ConfigureDiscountSign(commodity);
        SkillView.Refresh();
        ConfigureGoldButton(commodity);
        ConfigureHealthButton(commodity);
    }

    private void ConfigureDiscountSign(Commodity commodity)
    {
        float discount = 1 - commodity.Discount;
        DiscountSign.SetActive(discount != 0);
    }

    private void ConfigureGoldButton(Commodity commodity)
    {
        bool acceptGold = commodity.AcceptGold();
        PayWithGoldButton.gameObject.SetActive(acceptGold);
        if (!acceptGold)
            return;
        
        string priceText = commodity.GetGoldPrice();
        IdleGoldPriceText.text = priceText;
        HoverGoldPriceText.text = priceText;
        PressedGoldPriceText.text = priceText;
        InactiveGoldPriceText.text = priceText;
        PayWithGoldButton.SetStateToInactiveFrom(!commodity.GoldAffordable());
    }

    private void ConfigureHealthButton(Commodity commodity)
    {
        bool acceptHealth = commodity.AcceptHealth();
        PayWithHealthButton.gameObject.SetActive(acceptHealth);
        if (!acceptHealth)
            return;

        string priceText = commodity.GetHealthPrice();
        IdleHealthPriceText.text = priceText;
        HoverHealthPriceText.text = priceText;
        PressedHealthPriceText.text = priceText;
        InactiveHealthPriceText.text = priceText;
        PayWithHealthButton.SetStateToInactiveFrom(!commodity.HealthAffordable());
    }

    private void PayWithGold(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.CloseAnnotation();
        Get<Commodity>().PayWithGold();
    }

    private void PayWithHealth(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.CloseAnnotation();
        Get<Commodity>().PayWithHealth();
    }
}
