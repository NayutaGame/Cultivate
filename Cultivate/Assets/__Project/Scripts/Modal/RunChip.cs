using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunChip
{
    public int Level;

    public ChipEntry _entry;

    public string GetName() => _entry.Name;

    public RunChip(string entryName, int level = 0) : this(Encyclopedia.ChipCategory[entryName], level) { }
    public RunChip(ChipEntry entry, int level = 0)
    {
        _entry = entry;
        Level = level;
    }

    public static bool CanUpgrade(RunChip c1, RunChip c2)
    {
        if (c1 == null || c2 == null) return false;
        // Level max case
        return c1._entry == c2._entry && c1.Level == c2.Level;
    }

    public bool IsNeiGong => _entry.IsNeiGong;
    public bool IsWaiGong => _entry.IsWaiGong;
}
