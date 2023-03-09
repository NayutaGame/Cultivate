using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class HeroSlotInventory
{
    private HeroChipSlot[] _slots;
    public int Start;
    public int Limit;

    public HeroChipSlot this[int i]
    {
        get => _slots[i];
        set => _slots[i] = value;
    }

    public AcquiredRunChip GetAcquired(int i) => _slots[i].AcquiredRunChip;
    public void SetAcquired(int i, AcquiredRunChip acquiredRunChip) => _slots[i].AcquiredRunChip = acquiredRunChip;

    public int? FindAcquiredIdx(AcquiredRunChip acquiredRunChip) => _slots.FirstIdx(item => item.AcquiredRunChip == acquiredRunChip);

    public void SetJingJie(JingJie jingJie)
    {
        Start = RunManager.WaiGongStartFromJingJie[jingJie];
        Limit = RunManager.WaiGongLimitFromJingJie[jingJie];
    }

    public HeroSlotInventory()
    {
        _slots = new HeroChipSlot[RunManager.WaiGongLimit];
        for (int i = 0; i < _slots.Length; i++)
            _slots[i] = new HeroChipSlot(i);

        Start = 0;
        Limit = RunManager.WaiGongLimit;
    }

    public void Swap(int from, int to)
    {
        (_slots[@from].AcquiredRunChip, _slots[to].AcquiredRunChip) = (_slots[to].AcquiredRunChip, _slots[@from].AcquiredRunChip);
    }
}
