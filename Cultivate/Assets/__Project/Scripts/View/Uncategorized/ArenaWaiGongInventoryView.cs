using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaWaiGongInventoryView : InventoryView<RunChipView>
{
    public Button IndexOrderButton;
    public Button WuXingOrderButton;
    public Button TypeOrderButton;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);

        IndexOrderButton.onClick.AddListener(SortIndexOrder);
        WuXingOrderButton.onClick.AddListener(SortWuXingOrder);
        TypeOrderButton.onClick.AddListener(SortTypeOrder);
    }

    private void SortIndexOrder()
    {
        ArenaWaiGongInventory inventory = RunManager.Get<ArenaWaiGongInventory>(GetIndexPath());
        inventory.Sort((lhs, rhs) =>
        {
            int lIndex = Encyclopedia.SkillCategory.IndexOf(lhs._entry);
            int rIndex = Encyclopedia.SkillCategory.IndexOf(rhs._entry);
            return lIndex - rIndex;
        });
        RunCanvas.Instance.Refresh();
    }

    private void SortWuXingOrder()
    {
        ArenaWaiGongInventory inventory = RunManager.Get<ArenaWaiGongInventory>(GetIndexPath());
        inventory.Sort((lhs, rhs) =>
        {
            int lWuXing = lhs._entry.WuXing.HasValue ? lhs._entry.WuXing.Value : -1;
            int rWuXing = rhs._entry.WuXing.HasValue ? rhs._entry.WuXing.Value : -1;
            if (lWuXing != rWuXing)
                return lWuXing - rWuXing;

            int lIndex = Encyclopedia.SkillCategory.IndexOf(lhs._entry);
            int rIndex = Encyclopedia.SkillCategory.IndexOf(rhs._entry);
            return lIndex - rIndex;
        });
        RunCanvas.Instance.Refresh();
    }

    private void SortTypeOrder()
    {
        ArenaWaiGongInventory inventory = RunManager.Get<ArenaWaiGongInventory>(GetIndexPath());
        inventory.Sort((lhs, rhs) =>
        {
            int lType = lhs.GetSkillTypeCollection().Value;
            int rType = rhs.GetSkillTypeCollection().Value;
            if (lType != rType)
                return lType - rType;

            int lIndex = Encyclopedia.SkillCategory.IndexOf(lhs._entry);
            int rIndex = Encyclopedia.SkillCategory.IndexOf(rhs._entry);
            return lIndex - rIndex;
        });
        RunCanvas.Instance.Refresh();
    }


}
