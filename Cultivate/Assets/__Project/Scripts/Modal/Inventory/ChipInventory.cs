
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipInventory : Inventory<RunChip>
{
    public bool CanUpgradeInventory(int from, int to)
    {
        return RunChip.CanUpgrade(this[from], this[to]);
    }

    public void UpgradeInventory(int from, int to)
    {
        this[to].Upgrade();
        RemoveAt(from);
    }

    public void RefreshChip()
    {
        Clear();
        foreach (var chip in Encyclopedia.ChipCategory.Traversal)
        {
            Add(new RunChip(chip));
        }
    }

    public void UpgradeFirstChip()
    {
        if (this[0] != null)
            this[0].Upgrade();
    }
}
