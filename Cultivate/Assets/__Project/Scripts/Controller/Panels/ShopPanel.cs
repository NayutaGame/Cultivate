using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : Panel
{
    public ListView CommodityListView;

    public Button ExitButton;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        CommodityListView.SetAddress(_address.Append(".Commodities"));

        foreach (ItemView itemView in CommodityListView.ActivePool)
        {
            CommodityItemView commodityItemView = (CommodityItemView)itemView;
            commodityItemView.ClearBuyEvent();
            commodityItemView.BuyEvent += BuyEvent;
        }

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    public override void Refresh()
    {
        base.Refresh();
        CommodityListView.Refresh();
    }

    private void BuyEvent(Commodity commodity)
    {
        ShopPanelDescriptor d = _address.Get<ShopPanelDescriptor>();
        d.Buy(commodity);
        AudioManager.Instance.Play("钱币");
    }

    private void Exit()
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.Map.ReceiveSignal(new Signal());
        RunCanvas.Instance.SetNodeState(panelDescriptor);
    }
}
