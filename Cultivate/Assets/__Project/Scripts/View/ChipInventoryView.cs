using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChipInventoryView : InventoryView<InventoryChipView>, IDropHandler
{
    public Button RefreshChipButton;
    public Button ClearChipButton;
    public Button UpgradeFirstButton;
    public Button DrawWaiGongButton;
    public Button DrawStoneButton;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);

        RefreshChipButton.onClick.AddListener(RefreshChip);
        ClearChipButton.onClick.AddListener(ClearChip);
        UpgradeFirstButton.onClick.AddListener(UpgradeFirst);
        DrawWaiGongButton.onClick.AddListener(DrawWaiGong);
        DrawStoneButton.onClick.AddListener(DrawStone);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        RunChipView drop = eventData.pointerDrag.GetComponent<RunChipView>();
        if (drop == null) return;

        ChipInventory to = RunManager.Get<ChipInventory>(GetIndexPath());

        RunChip from = RunManager.Get<RunChip>(drop.GetIndexPath());
        if (from != null)
        {
            if (to.MoveToEnd(from)) return;
        }
    }

    private void RefreshChip()
    {
        RunManager.Instance.RefreshChip();
        Refresh();
    }

    private void ClearChip()
    {
        RunManager.Instance.ClearChip();
        Refresh();
    }

    private void UpgradeFirst()
    {
        RunManager.Instance.UpgradeFirstChip();
        Refresh();
    }

    private void DrawWaiGong()
    {
        RunManager.Instance.TryDrawWaiGong();
        Refresh();
    }

    private void DrawStone()
    {
        RunManager.Instance.TryDrawStone();
        Refresh();
    }
}
