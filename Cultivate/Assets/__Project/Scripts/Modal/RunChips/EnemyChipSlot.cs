using System.Collections;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using UnityEngine;

public class EnemyChipSlot
{
    public int SlotIndex;
    private int[] _powers;
    public RunChip Chip;

    public EnemyChipSlot(int slotIndex)
    {
        SlotIndex = slotIndex;
        _powers = new int[WuXing.Length];
    }

    public string GetName() => Chip.GetName();
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

    public bool IsReveal()
    {
        int start = RunManager.Instance.Enemy.Start;
        int end = RunManager.Instance.Enemy.Start + RunManager.Instance.Enemy.Limit;
        return start <= SlotIndex && SlotIndex < end;
    }
}
