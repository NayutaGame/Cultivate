using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BarterPanel : Panel
{
    public BarterInventoryView BarterInventoryView;

    public Button ExitButton;

    private IndexPath _indexPath;

    public override void Configure()
    {
        base.Configure();

        _indexPath = new IndexPath("CurrentNode.CurrentPanel");
        BarterInventoryView.Configure(new IndexPath($"{_indexPath}.Inventory"));

        foreach (BarterItemView barterItemView in BarterInventoryView.Views)
        {
            barterItemView.ClearExchangeEvent();
            barterItemView.ExchangeEvent += ExchangeEvent;
        }

        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(Exit);
    }

    public override void Refresh()
    {
        base.Refresh();
        BarterInventoryView.Refresh();
    }

    private void ExchangeEvent(BarterItem barterItem)
    {
        BarterPanelDescriptor d = RunManager.Get<BarterPanelDescriptor>(_indexPath);
        d.Exchange(barterItem);
    }

    private void Exit()
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Map.ReceiveSignal(new Signal());
        RunCanvas.Instance.SetNodeState(panelDescriptor);
    }
}
