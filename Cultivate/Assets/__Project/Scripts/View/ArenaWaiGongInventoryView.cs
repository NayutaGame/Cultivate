using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaWaiGongInventoryView : InventoryView<RunChipView>
{
    public Button IndexOrderButton;
    public Button WuXingOrderButton;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);

        IndexOrderButton.onClick.AddListener(SortIndexOrder);
        WuXingOrderButton.onClick.AddListener(SortWuXingOrder);
    }

    private void SortIndexOrder()
    {
        ArenaWaiGongInventory inventory = RunManager.Get<ArenaWaiGongInventory>(GetIndexPath());
        inventory.Sort((lhs, rhs) =>
        {
            // int lJingJie = lhs.JingJie;
            // int rJingJie = rhs.JingJie;
            // if (lJingJie != rJingJie)
            //     return rJingJie - lJingJie;
            //
            // int lWuXing = lhs._entry.WuXing ?? -1;
            // int rWuXing = rhs._entry.WuXing ?? -1;
            // if (lWuXing != rWuXing)
            //     return lWuXing - rWuXing;

            int lIndex = Encyclopedia.ChipCategory.IndexOf(lhs._entry);
            int rIndex = Encyclopedia.ChipCategory.IndexOf(rhs._entry);
            return lIndex - rIndex;
        });
        RunCanvas.Instance.Refresh();
    }

    private void SortWuXingOrder()
    {
        ArenaWaiGongInventory inventory = RunManager.Get<ArenaWaiGongInventory>(GetIndexPath());
        inventory.Sort((lhs, rhs) =>
        {
            // int lJingJie = lhs.JingJie;
            // int rJingJie = rhs.JingJie;
            // if (lJingJie != rJingJie)
            //     return rJingJie - lJingJie;

            int lWuXing = lhs._entry.WuXing.HasValue ? lhs._entry.WuXing.Value : -1;
            int rWuXing = rhs._entry.WuXing.HasValue ? rhs._entry.WuXing.Value : -1;
            if (lWuXing != rWuXing)
                return lWuXing - rWuXing;

            int lIndex = Encyclopedia.ChipCategory.IndexOf(lhs._entry);
            int rIndex = Encyclopedia.ChipCategory.IndexOf(rhs._entry);
            return lIndex - rIndex;
        });
        RunCanvas.Instance.Refresh();
    }
}
