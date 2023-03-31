using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class HeroSlotInventory : GDictionary
{
    private HeroChipSlot[] _slots;
    public int Start;
    public int Limit;

    public HeroChipSlot this[int i]
    {
        get => _slots[i];
        set => _slots[i] = value;
    }

    public IEnumerable<HeroChipSlot> Traversal
    {
        get
        {
            foreach (var item in _slots)
                yield return item;
        }
    }

    public AcquiredRunChip GetAcquired(int i) => _slots[i].AcquiredRunChip;
    public void SetAcquired(int i, AcquiredRunChip acquiredRunChip) => _slots[i].AcquiredRunChip = acquiredRunChip;

    public int? FindAcquiredIdx(AcquiredRunChip acquiredRunChip) => _slots.FirstIdx(item => item.AcquiredRunChip == acquiredRunChip);

    public void SetJingJie(JingJie jingJie)
    {
        Start = RunManager.WaiGongStartFromJingJie[jingJie];
        Limit = RunManager.WaiGongLimitFromJingJie[jingJie];
    }

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;

    public HeroSlotInventory()
    {
        _accessors = new()
        {
            { "Slots", () => _slots },
        };

        _slots = new HeroChipSlot[RunManager.WaiGongLimit];
        for (int i = 0; i < _slots.Length; i++)
            _slots[i] = new HeroChipSlot(i);

        Start = 0;
        Limit = RunManager.WaiGongLimit;
    }

    public bool Swap(int from, int to)
    {
        (_slots[from].AcquiredRunChip, _slots[to].AcquiredRunChip) = (_slots[to].AcquiredRunChip, _slots[from].AcquiredRunChip);
        RunManager.Instance.EquippedChanged();
        return true;
    }

    public bool Swap(HeroChipSlot from, HeroChipSlot to) => Swap(IndexOf(from), IndexOf(to));

    public int IndexOf(HeroChipSlot heroChipSlot)
    {
        return _slots.FirstIdx(slot => slot == heroChipSlot).Value;
    }
}
