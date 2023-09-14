using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : Panel
{
    public CommodityInventoryView CommodityInventoryView;

    public Button ExitButton;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Battle.Map.CurrentNode.CurrentPanel");
        CommodityInventoryView.Configure(new Address($"{_address}.Commodities"));

        foreach (CommodityView commodityView in CommodityInventoryView.Views)
        {
            commodityView.ClearBuyEvent();
            commodityView.BuyEvent += BuyEvent;
        }

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    public override void Refresh()
    {
        base.Refresh();
        CommodityInventoryView.Refresh();
    }

    private void BuyEvent(Commodity commodity)
    {
        ShopPanelDescriptor d = _address.Get<ShopPanelDescriptor>();
        d.Buy(commodity);
        AudioManager.Instance.Play("钱币");
    }

    private void Exit()
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Battle.Map.ReceiveSignal(new Signal());
        RunCanvas.Instance.SetNodeState(panelDescriptor);
    }
}
