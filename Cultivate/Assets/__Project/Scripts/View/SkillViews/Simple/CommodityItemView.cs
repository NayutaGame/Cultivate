
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommodityItemView : LegacySimpleView
{
    public LegacySimpleView SkillView;
    public TMP_Text PriceText;
    public GameObject DiscountSign;
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
            DiscountSign.SetActive(false);
        }
        else
        {
            DiscountText.text = $"{discount * 10}折";
            DiscountSign.SetActive(true);
        }

        if (commodity.Affordable())
        {
            PriceText.color = Color.white;
        }
        else
        {
            PriceText.color = Color.red;
        }
    }
}
