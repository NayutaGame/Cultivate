using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class WuXingChipEntry : ChipEntry
{
    public WuXingChipEntry(string name, string description, WuXing wuXing) : base(name, JingJie.LianQi, description,
        canPlug: (tile, runChip) => tile.AcquiredRunChip == null,
        plug: (tile, runChip) =>
        {
            tile.AcquiredRunChip = new AcquiredRunChip(tile, runChip);
            RunManager.Instance.DanTian.Adjacents(tile).Do(t => t.Powers[wuXing] += 1);
        },
        canUnplug: acquiredRunChip => true,
        unplug: acquiredRunchip =>
        {
            acquiredRunchip.Tile.AcquiredRunChip = null;
            RunManager.Instance.DanTian.Adjacents(acquiredRunchip.Tile).Do(t => t.Powers[wuXing] -= 1);
        })
    {
    }
}
