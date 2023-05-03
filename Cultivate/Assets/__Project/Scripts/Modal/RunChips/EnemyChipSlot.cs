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

    public string GetDescription()
    {
        if (Chip == null)
            return null;

        int[] powers = new int[5];
        WuXing.Traversal.Do(wuXing => powers[wuXing] = GetPower(wuXing));
        return Chip._entry.Evaluate(GetJingJie().Value, GetJingJie().Value - Chip._entry.JingJieRange.Start);
    }
    public int GetLevel() => Chip.Level;
    public int GetPower(WuXing wuXing) => _powers[wuXing];
    public void SetPower(WuXing wuXing, int value) => _powers[wuXing] = value;
    public JingJie? GetJingJie()
    {
        if (Chip == null)
            return null;
        return Chip.JingJie;
    }

    public int GetManaCost()
    {
        if (Chip?._entry is WaiGongEntry waigongEntry)
        {
            int[] powers = new int[5];
            WuXing.Traversal.Do(wuXing => powers[wuXing] = GetPower(wuXing));
            return waigongEntry.GetManaCost(GetLevel(), GetJingJie().Value, GetJingJie().Value - Chip._entry.JingJieRange.Start, powers);
        }

        return 0;
    }

    public string GetManaCostString()
    {
        int manaCost = GetManaCost();
        return manaCost == 0 ? "" : manaCost.ToString();
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
        RunManager.Instance.StageEnvironmentChanged();
        return true;
    }

    public bool TryWrite(HeroChipSlot heroChipSlot)
    {
        Chip = heroChipSlot.RunChip?.Clone();
        WuXing.Traversal.Do(wuXing => SetPower(wuXing, heroChipSlot.GetPower(wuXing)));
        RunManager.Instance.StageEnvironmentChanged();
        return true;
    }

    public bool TryIncreseJingJie()
    {
        if (Chip == null)
            return false;

        JingJie curr = Chip.JingJie;
        JingJie next = curr + 1;
        if (!Chip._entry.JingJieRange.Contains(next))
            next = Chip._entry.JingJieRange.Start;
        Chip.JingJie = next;
        RunManager.Instance.StageEnvironmentChanged();
        return true;
    }
}
