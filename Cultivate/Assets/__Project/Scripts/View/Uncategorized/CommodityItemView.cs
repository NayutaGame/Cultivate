
using TMPro;
using UnityEngine.EventSystems;

public class CommodityItemView : LegacyAddressBehaviour
{
    public PenetrateSkillView SkillView;
    public TMP_Text PriceText;
    public TMP_Text DiscountText;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SkillView.SetAddress(GetAddress().Append(".Skill"));
    }

    public override void Refresh()
    {
        base.Refresh();
        Commodity commodity = Get<Commodity>();

        bool isReveal = commodity != null;
        gameObject.SetActive(isReveal);
        if (!isReveal)
            return;

        SkillView.Refresh();
        PriceText.text = commodity.FinalPrice.ToString();

        float discount = 1 - commodity.Discount;
        if (discount == 0)
        {
            DiscountText.gameObject.SetActive(false);
        }
        else
        {
            DiscountText.text = $"{discount * 100}%OFF";
            DiscountText.gameObject.SetActive(true);
        }

        // BuyButton.interactable = commodity.Affordable();
    }
}
