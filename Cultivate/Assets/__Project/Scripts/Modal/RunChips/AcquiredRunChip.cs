using System.Collections;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using UnityEngine;

public class AcquiredRunChip
{
    public Tile Tile;
    public RunChip Chip;

    public AcquiredRunChip(Tile tile, RunChip runChip)
    {
        Tile = tile;
        Chip = runChip;
    }

    public string GetName() => Chip.GetName();
    public int GetLevel() => Chip.Level;
    public int GetPower(WuXing wuXing) => Tile.Powers[wuXing];

    // dirty variable
    public int GetManaCost()
    {
        if (Chip._entry is WaiGongEntry waigongEntry)
        {
            int[] powers = new int[5];
            WuXing.Traversal.Do(wuXing => powers[wuXing] = GetPower(wuXing));
            return waigongEntry.GetManaCost(GetLevel(), powers);
        }

        return 0;
    }

    public string GetPowerString() => Tile.GetPowerString();

    public void Upgrade()
    {
        Chip.Upgrade();
    }

    public bool CanUnplug() => Chip._entry.CanUnplug(this);
    public void Unplug() => Chip._entry.Unplug(this);
}
