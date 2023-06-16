using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : Panel
{
    public CommodityInventoryView CommodityInventoryView;

    public Button ExitButton;

    private IndexPath _indexPath;

    public override void Configure()
    {
        base.Configure();

        _indexPath = new IndexPath("CurrentNode.CurrentPanel");
        CommodityInventoryView.Configure(new IndexPath($"{_indexPath}.Commodities"));

        foreach (CommodityView commodityView in CommodityInventoryView.Views)
        {
            commodityView.BuyEvent += BuyEvent;
        }

        ExitButton.onClick.AddListener(Exit);
    }

    public override void Refresh()
    {
        base.Refresh();
        CommodityInventoryView.Refresh();
    }

    private void BuyEvent(Commodity commodity)
    {
        ShopPanelDescriptor d = RunManager.Get<ShopPanelDescriptor>(_indexPath);
        d.Buy(commodity);
    }

    private void Exit()
    {
        ShopPanelDescriptor d = RunManager.Get<ShopPanelDescriptor>(_indexPath);
        d.ReceiveSignal(new Signal());
    }
}
