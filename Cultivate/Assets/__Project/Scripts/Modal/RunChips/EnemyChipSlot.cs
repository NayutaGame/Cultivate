using System.Collections;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChipSlot
{
    public int SlotIndex;
    private int[] _powers;
    public RunChip Chip;

    public bool IsReveal;

    public EnemyChipSlot(int slotIndex)
    {
        SlotIndex = slotIndex;
        _powers = new int[WuXing.Length];
        IsReveal = true;
    }

    public string GetName() => Chip?.GetName();
    public int GetLevel() => Chip.Level;
    public int GetPower(WuXing wuXing) => _powers[wuXing];
    public void SetPower(WuXing wuXing, int value) => _powers[wuXing] = value;

    public int GetManaCost()
    {
        if (Chip?._entry is WaiGongEntry waigongEntry)
        {
            int[] powers = new int[5];
            WuXing.Traversal.Do(wuXing => powers[wuXing] = GetPower(wuXing));
            return waigongEntry.GetManaCost(GetLevel(), powers);
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

    public bool TryWrite(RunChip chip)
    {
        Chip = chip.Clone();
        // RunManager.Instance.EquippedChanged();
        return true;
    }

    public bool TryWrite(AcquiredRunChip acquired)
    {
        Chip = acquired.Chip.Clone();
        WuXing.Traversal.Do(wuXing => SetPower(wuXing, acquired.GetPower(wuXing)));
        RunManager.Instance.EquippedChanged();
        return true;
    }

    public bool TryWrite(HeroChipSlot heroChipSlot)
    {
        Chip = heroChipSlot.RunChip?.Clone();
        WuXing.Traversal.Do(wuXing => SetPower(wuXing, heroChipSlot.GetPower(wuXing)));
        RunManager.Instance.EquippedChanged();
        return true;
    }
}
