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

    private IndexPath _indexPath;

    public override void Configure()
    {
        base.Configure();

        _indexPath = new IndexPath("Run.CurrentNode.CurrentPanel");
        CommodityInventoryView.Configure(new IndexPath($"{_indexPath}.Commodities"));

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
        ShopPanelDescriptor d = DataManager.Get<ShopPanelDescriptor>(_indexPath);
        d.Buy(commodity);
        AudioManager.Instance.Play("钱币");
    }

    private void Exit()
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Map.ReceiveSignal(new Signal());
        RunCanvas.Instance.SetNodeState(panelDescriptor);
    }
}
