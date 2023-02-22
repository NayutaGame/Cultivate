using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunChip
{
    public ChipEntry _entry;

    public string GetName() => _entry.Name;
    public string GetDescription()
    {
        if (_entry is NeigongEntry neigongEntry)
            return neigongEntry.Description;

        if (_entry is WaigongEntry waigongEntry)
            return waigongEntry.GetDescription(Level);

        return null;
    }

    public int Level { get; private set; }
    public int RunUsedTimes { get; protected set; }
    public int RunEquippedTimes { get; protected set; }

    private int[] _powers;
    public int GetPower(WuXing wuXing) => _powers[wuXing];
    public void SetPower(WuXing wuXing, int value) => _powers[wuXing] = value;

    public RunChip(string entryName, int level = 0) : this(Encyclopedia.ChipCategory[entryName], level) { }
    public RunChip(ChipEntry entry, int level = 0)
    {
        _entry = entry;
        Level = level;
        _powers = new int[] { 0, 0, 0, 0, 0 };
    }

    public static bool CanUpgrade(RunChip c1, RunChip c2)
    {
        if (c1 == null || c2 == null) return false;
        // Level max case
        return c1._entry == c2._entry && c1.Level == c2.Level;
    }

    public void Upgrade()
    {
        Level += 1;
    }

    public bool IsNeiGong => _entry.IsNeiGong;
    public bool IsWaiGong => _entry.IsWaiGong;
}
