
using System;
using TMPro;
using UnityEngine.UI;

public class CommodityItemView : ItemView
{
    public SkillView SkillView;
    public TMP_Text PriceText;
    public TMP_Text DiscountText;
    public Button BuyButton;

    public event Action<Commodity> BuyEvent;
    public void ClearBuyEvent() => BuyEvent = null;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SkillView.SetAddress(GetAddress().Append(".Skill"));

        // BuyButton.onClick.RemoveAllListeners();
        // BuyButton.onClick.AddListener(Buy);
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
        DiscountText.text = $"{commodity.Discount * 100}%OFF";
        // BuyButton.interactable = commodity.Affordable();
    }

    private void Buy()
    {
        Commodity commodity = Get<Commodity>();
        BuyEvent?.Invoke(commodity);
        CanvasManager.Instance.RunCanvas.Refresh();
    }
}
