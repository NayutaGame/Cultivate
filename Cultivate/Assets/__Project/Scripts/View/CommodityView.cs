using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommodityView : MonoBehaviour, IAddress
{
    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    public RunSkillView SkillView;
    public TMP_Text PriceText;
    public Button BuyButton;

    public event Action<Commodity> BuyEvent;
    public void ClearBuyEvent() => BuyEvent = null;

    public void SetAddress(Address address)
    {
        _address = address;
        SkillView.SetAddress(_address.Append(".Skill"));

        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(Buy);
    }

    public void Refresh()
    {
        Commodity commodity = Get<Commodity>();

        bool isReveal = commodity != null;
        gameObject.SetActive(isReveal);
        if (!isReveal)
            return;

        SkillView.Refresh();
        PriceText.text = commodity.FinalPrice.ToString();
        BuyButton.interactable = commodity.Affordable();
    }

    private void Buy()
    {
        Commodity commodity = Get<Commodity>();
        BuyEvent?.Invoke(commodity);
        RunCanvas.Instance.Refresh();
    }
}
