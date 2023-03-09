using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquiredSlotInventory
{
    private AcquiredRunChip[] _list;

    public AcquiredSlotInventory()
    {
        _list = new AcquiredRunChip[RunManager.WaiGongLimit];
    }

    public AcquiredRunChip this[int i]
    {
        get => _list[i];
        set => _list[i] = value;
    }
}
