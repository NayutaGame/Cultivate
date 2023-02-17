
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<RunChip> _chips;

    public Inventory()
    {
        _chips = new List<RunChip>();
    }

    public RunChip this[int i]
    {
        get => _chips[i];
        set => _chips[i] = value;
    }

    public void Add(RunChip chip) => _chips.Add(chip);

    public void Remove(RunChip chip) => _chips.Remove(chip);

    public void RemoveAt(int i) => _chips.RemoveAt(i);

    public int GetRunChipCount()
    {
        return _chips.Count;
    }

    public RunChip TryGetRunChip(int i)
    {
        if(i < _chips.Count)
            return _chips[i];
        return null;
    }

    public void Swap(int from, int to)
    {
        (_chips[from], _chips[to]) = (_chips[to], _chips[from]);
    }

    public bool CanUpgradeInventory(int from, int to)
    {
        return RunChip.CanUpgrade(_chips[from], _chips[to]);
    }

    public void UpgradeInventory(int from, int to)
    {
        _chips[to].Level += 1;
        RemoveAt(from);
    }

    public void RefreshChip()
    {
        _chips.RemoveAll(item => true);
        foreach (var chip in Encyclopedia.ChipCategory.Traversal)
        {
            _chips.Add(new RunChip(chip));
        }
    }

    public void UpgradeFirstChip()
    {
        if (_chips[0] != null)
            _chips[0].Level += 1;
    }
}
