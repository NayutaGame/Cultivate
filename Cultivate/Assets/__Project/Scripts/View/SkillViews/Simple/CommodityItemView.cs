
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommodityItemView : SimpleView
{
    public SimpleView SkillView;
    public TMP_Text PriceText;
    public Image Icon;
    public TMP_Text DiscountText;

    public Color NormalDiscountColor;

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

        if (commodity.Affordable())
        {
            DiscountText.color = NormalDiscountColor;
            Icon.color = Color.white;
            PriceText.color = Color.white;
        }
        else
        {
            DiscountText.color = Color.red;
            Icon.color = Color.red;
            PriceText.color = Color.red;
        }
    }
}
