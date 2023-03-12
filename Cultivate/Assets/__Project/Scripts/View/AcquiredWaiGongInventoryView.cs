using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AcquiredWaiGongInventoryView : InventoryView<AcquiredWaiGongView>, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        RunChipView drop = eventData.pointerDrag.GetComponent<RunChipView>();
        if (drop == null)
            return;

        if (drop.GetIndexPath()._str == "TryGetAcquired")
        {
            if (RunManager.Instance.AcquiredWaiGongMoveToEnd(drop.GetIndexPath())) return;
        }
        else if (drop.GetIndexPath()._str == "GetHeroSlot")
        {
            if (RunManager.Instance.Unequip(drop.GetIndexPath())) return;
        }
    }

    // private void RefreshChip()
    // {
    //     RunManager.Instance.RefreshChip();
    //     Refresh();
    // }
    //
    // private void ClearChip()
    // {
    //     RunManager.Instance.ClearChip();
    //     Refresh();
    // }
    //
    // private void DrawChip()
    // {
    //     RunManager.Instance.DrawChip();
    //     Refresh();
    // }
    //
    // private void UpgradeFirst()
    // {
    //     RunManager.Instance.UpgradeFirstChip();
    //     Refresh();
    // }
}
