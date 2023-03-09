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
            RunManager.Instance.Hero.HeroSlotInventory[slotIndex].XueWei = tile;
            RunManager.Instance.ChipInventory.Remove(runChip);
        },
        canUnplug: acquiredRunChip => true,
        unplug: acquiredRunChip =>
        {
            acquiredRunChip.Tile.AcquiredRunChip = null;
            RunManager.Instance.Hero.HeroSlotInventory[slotIndex].XueWei = null;
            RunManager.Instance.ChipInventory.Add(acquiredRunChip.Chip);
        })
    {
    }
}
