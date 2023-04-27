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
    public string GetDescription()
    {
        int[] powers = new int[5];
        WuXing.Traversal.Do(wuXing => powers[wuXing] = GetPower(wuXing));
        return Chip._entry.Description.Eval(GetLevel(), GetJingJie(), GetJingJie() - Chip._entry.JingJieRange.Start, powers);
    }

    public int GetLevel() => Chip.Level;
    public int GetPower(WuXing wuXing) => Tile.Powers[wuXing];
    public JingJie GetJingJie() => Chip.JingJie;

    // dirty variable
    public int GetManaCost()
    {
        if (Chip._entry is WaiGongEntry waigongEntry)
        {
            int[] powers = new int[5];
            WuXing.Traversal.Do(wuXing => powers[wuXing] = GetPower(wuXing));
            return waigongEntry.GetManaCost(GetLevel(), GetJingJie(), GetJingJie() - Chip._entry.JingJieRange.Start, powers);
        }

        return 0;
    }

    public string GetManaCostString()
    {
        int manaCost = GetManaCost();
        return manaCost == 0 ? "" : manaCost.ToString();
    }

    public string GetPowerString() => Tile.GetPowerString();

    public void Upgrade()
    {
        Chip.Upgrade();
    }

    public bool CanUnplug() => Chip._entry.CanUnplug(this);
    public void Unplug() => Chip._entry.Unplug(this);
}
