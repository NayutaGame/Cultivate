using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HeroChipSlot
{
    public int SlotIndex;
    public Tile XueWei;
    public AcquiredRunChip AcquiredRunChip;

    public HeroChipSlot(int slotIndex)
    {
        SlotIndex = slotIndex;
    }

    public RunChip RunChip => AcquiredRunChip?.Chip;
    public string GetName() => AcquiredRunChip.GetName();
    public int GetLevel() => AcquiredRunChip.GetLevel();
    public int GetPower(WuXing wuXing)
    {
        int power = 0;
        if (XueWei != null)
            power += XueWei.Powers[wuXing];
        if (AcquiredRunChip != null)
            power += AcquiredRunChip.GetPower(wuXing);
        return power;
    }

    public string GetPowerString()
    {
        StringBuilder sb = new();
        for (int i = 0; i < WuXing.Length; i++)
        {
            if (GetPower(i) == 0) continue;
            sb.Append($"{GetPower(i)}{(WuXing)i} ");
        }

        return sb.ToString();
    }
}
