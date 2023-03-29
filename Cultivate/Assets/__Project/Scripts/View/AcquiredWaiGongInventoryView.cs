using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AcquiredWaiGongInventoryView : InventoryView<RunChipView>, IDropHandler
{
    public Button[] DrawButtons;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);

        DrawButtons.Length.Do(i =>
        {
            JingJie jingJie = i;
            DrawButtons[i].onClick.AddListener(() => DrawJingJie(jingJie));
        });
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        RunChipView drop = eventData.pointerDrag.GetComponent<RunChipView>();
        if (drop == null) return;

        AcquiredWaiGongInventory to = RunManager.Get<AcquiredWaiGongInventory>(GetIndexPath());

        AcquiredRunChip fromAcquired = RunManager.Get<AcquiredRunChip>(drop.GetIndexPath());
        if (fromAcquired != null)
        {
            if (to.MoveToEnd(fromAcquired)) return;
            return;
        }

        HeroChipSlot fromHeroChipSlot = RunManager.Get<HeroChipSlot>(drop.GetIndexPath());
        if (fromHeroChipSlot != null)
        {
            if (fromHeroChipSlot.TryUnequip()) return;
            return;
        }
    }

    private void DrawJingJie(JingJie jingJie)
    {
        RunManager.Instance.TryDrawAcquired(jingJie);
        RunCanvas.Instance.Refresh();
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
