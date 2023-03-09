using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HeroRunChip
{
    public int SlotIndex;
    public AcquiredRunChip AcquiredRunChip;

    public HeroRunChip(int slotIndex, AcquiredRunChip acquiredRunChip)
    {
        SlotIndex = slotIndex;
        AcquiredRunChip = acquiredRunChip;
    }

    public RunChip RunChip => AcquiredRunChip.Chip;
    public string GetName() => AcquiredRunChip.GetName();
    public int GetLevel() => AcquiredRunChip.GetLevel();
    public int GetPower(WuXing wuXing)
    {
        return AcquiredRunChip.GetPower(wuXing) + RunManager.Instance.AcquiredSlotInventory[SlotIndex].GetPower(wuXing);
    }

    public string GetPowerString() => AcquiredRunChip.GetPowerString();
}
