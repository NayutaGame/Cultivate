using System.Collections;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
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
    public string GetDescription()
    {
        if (AcquiredRunChip == null)
            return null;

        int[] powers = new int[5];
        WuXing.Traversal.Do(wuXing => powers[wuXing] = GetPower(wuXing));
        return AcquiredRunChip.Chip._entry.Description.Eval(GetLevel(), GetJingJie().Value, powers);
    }

    public int GetLevel() => AcquiredRunChip.GetLevel();

    public JingJie? GetJingJie()
    {
        if (AcquiredRunChip == null)
            return null;
        return AcquiredRunChip.GetJingJie();
    }
    public int GetPower(WuXing wuXing)
    {
        int power = 0;
        if (XueWei != null)
            power += XueWei.Powers[wuXing];
        if (AcquiredRunChip != null)
            power += AcquiredRunChip.GetPower(wuXing);
        return power;
    }

    public int GetManaCost()
    {
        if (AcquiredRunChip?.Chip._entry is WaiGongEntry waigongEntry)
        {
            int[] powers = new int[5];
            WuXing.Traversal.Do(wuXing => powers[wuXing] = GetPower(wuXing));
            return waigongEntry.GetManaCost(GetLevel(), GetJingJie().Value, powers);
        }

        return 0;
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

    public bool IsReveal()
    {
        int start = RunManager.Instance.Hero.HeroSlotInventory.Start;
        int end = RunManager.Instance.Hero.HeroSlotInventory.Start + RunManager.Instance.Hero.HeroSlotInventory.Limit;
        return start <= SlotIndex && SlotIndex < end;
    }

    public bool TryUnequip()
    {
        AcquiredRunChip toUnequip = AcquiredRunChip;
        if (toUnequip == null)
            return false;

        AcquiredRunChip = null;
        RunManager.Instance.AcquiredInventory.Add(toUnequip);
        RunManager.Instance.EquippedChanged();
        return true;
    }

    public bool TryUnequip(AcquiredRunChip acquired)
    {
        AcquiredRunChip toUnequip = AcquiredRunChip;
        if (toUnequip == null)
            return false;

        AcquiredRunChip = acquired;
        int i = RunManager.Instance.AcquiredInventory.IndexOf(acquired);
        RunManager.Instance.AcquiredInventory.RemoveAt(i);
        RunManager.Instance.AcquiredInventory.Insert(i, toUnequip);
        RunManager.Instance.EquippedChanged();
        return true;
    }

    public bool TryEquip(AcquiredRunChip toEquip)
    {
        int index = RunManager.Instance.AcquiredInventory.IndexOf(toEquip);

        if (AcquiredRunChip != null)
        {
            RunManager.Instance.AcquiredInventory[index] = AcquiredRunChip;
        }
        else
        {
            RunManager.Instance.AcquiredInventory.RemoveAt(index);
        }

        AcquiredRunChip = toEquip;

        RunManager.Instance.EquippedChanged();
        return true;
    }

    public bool IsManaShortage()
    {
        return RunManager.Instance.ManaShortageBrief[SlotIndex];
    }
}
