using System.Collections;
using System.Collections.Generic;
using System.Text;
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
