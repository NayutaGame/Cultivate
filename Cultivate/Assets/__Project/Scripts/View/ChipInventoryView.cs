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
    public Button DrawChipButton;
    public Button UpgradeFirstButton;

    public override void Configure(IInventory inventory)
    {
        base.Configure(inventory);
        RefreshChipButton.onClick.AddListener(RefreshChip);
        ClearChipButton.onClick.AddListener(ClearChip);
        DrawChipButton.onClick.AddListener(DrawChip);
        UpgradeFirstButton.onClick.AddListener(UpgradeFirst);
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

    private void DrawChip()
    {
        RunManager.Instance.DrawChip();
        Refresh();
    }

    private void UpgradeFirst()
    {
        RunManager.Instance.UpgradeFirstChip();
        Refresh();
    }
}
