using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XueWeiEntry : ChipEntry
{
    public XueWeiEntry(
        string name,
        string description,
        int slotIndex) : base(name, JingJie.LianQi, description,
        canPlug: (tile, runChip) => tile.AcquiredRunChip == null,
        plug: (tile, runChip) =>
        {
            AcquiredRunChip acquiredRunChip = new AcquiredRunChip(tile, runChip);
            tile.AcquiredRunChip = acquiredRunChip;
            RunManager.Instance.AcquiredSlotInventory[slotIndex] = acquiredRunChip;
        },
        canUnplug: acquiredRunChip => true,
        unplug: acquiredRunChip =>
        {
            acquiredRunChip.Tile.AcquiredRunChip = null;
            RunManager.Instance.AcquiredSlotInventory[slotIndex] = null;
        })
    {
    }
}
