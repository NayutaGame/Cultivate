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

    public override void Configure(IInventory inventory)
    {
        base.Configure(inventory);
        RefreshChipButton.onClick.AddListener(RefreshChip);
        ClearChipButton.onClick.AddListener(ClearChip);
        UpgradeFirstButton.onClick.AddListener(UpgradeFirst);
        DrawWaiGongButton.onClick.AddListener(DrawWaiGong);
        DrawStoneButton.onClick.AddListener(DrawStone);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        RunChipView drop = eventData.pointerDrag.GetComponent<RunChipView>();
        if (drop == null)
            return;

        if (drop.GetIndexPath()._str == "TryGetRunChip")
        {
            if (RunManager.Instance.InventoryMoveToEnd(drop.GetIndexPath())) return;
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
        RunManager.Instance.DrawWaiGong();
        Refresh();
    }

    private void DrawStone()
    {
        RunManager.Instance.DrawStone();
        Refresh();
    }
}
