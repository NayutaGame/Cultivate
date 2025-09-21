
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommodityItemView : XView
{
    public SkillView SkillView;
    public GameObject DiscountSign;
    public TMP_Text DiscountText;

    public BuyButton PayWithGoldButton;
    public TMP_Text GoldPriceText;
    public BuyButton PayWithHealthButton;
    public TMP_Text HealthPriceText;

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
        if (discount == 0)
        {
            DiscountSign.SetActive(false);
        }
        else
        {
            DiscountText.text = $"{discount * 10}折";
            DiscountSign.SetActive(true);
        }
    }

    private void ConfigureGoldButton(Commodity commodity)
    {
        bool acceptGold = commodity.AcceptGold();
        PayWithGoldButton.gameObject.SetActive(acceptGold);
        if (!acceptGold)
            return;
        
        GoldPriceText.text = commodity.GetGoldPrice();
        PayWithGoldButton.SetStateToInactiveFrom(!commodity.GoldAffordable());
    }

    private void ConfigureHealthButton(Commodity commodity)
    {
        bool acceptHealth = commodity.AcceptHealth();
        PayWithHealthButton.gameObject.SetActive(acceptHealth);
        if (!acceptHealth)
            return;

        HealthPriceText.text = commodity.GetHealthPrice();
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
