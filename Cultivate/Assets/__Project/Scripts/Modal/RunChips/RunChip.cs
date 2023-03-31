using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunChip
{
    public ChipEntry _entry;

    public string GetName() => _entry.Name;
    public string GetDescription() => _entry.Description.Eval(Level, JingJie, new int[] { 0, 0, 0, 0, 0 });

    public int GetManaCost()
    {
        if (_entry is WaiGongEntry waigongEntry)
            return waigongEntry.GetManaCost(Level, JingJie);

        return 0;
    }

    public JingJie JingJie;

    public int Level { get; private set; }
    public int RunUsedTimes { get; protected set; }
    public int RunEquippedTimes { get; protected set; }

    public RunChip(ChipEntry entry, JingJie jingJie, int level = 0)
    {
        _entry = entry;
        JingJie = jingJie;
        Level = level;
    }

    private RunChip(RunChip prototype)
    {
        _entry = prototype._entry;
        Level = prototype.Level;
        RunUsedTimes = prototype.RunUsedTimes;
        RunEquippedTimes = prototype.RunEquippedTimes;
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

    public RunChip Clone()
    {
        return new RunChip(this);
    }

    public bool CanPlug(Tile tile) => _entry.CanPlug(tile, this);
    public void Plug(Tile tile) => _entry.Plug(tile, this);

    public bool TryPlug(Tile tile)
    {
        if (!CanPlug(tile)) return false;
        Plug(tile);

        if (_entry is WaiGongEntry)
            RunManager.Instance.Acquire(new AcquireDetails(this));

        return true;
    }
}
