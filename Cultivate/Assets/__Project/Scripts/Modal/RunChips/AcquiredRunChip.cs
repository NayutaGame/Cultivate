using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    public string GetPowerString() => Tile.GetPowerString();

    public void Upgrade()
    {
        Chip.Upgrade();
    }
}
