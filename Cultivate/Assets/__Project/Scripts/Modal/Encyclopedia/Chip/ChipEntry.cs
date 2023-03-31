using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ChipEntry : Entry
{
    private CLLibrary.Range _jingJieRange;
    public CLLibrary.Range JingJieRange => _jingJieRange;

    private ChipDescription _description;
    public ChipDescription Description => _description;

    private WuXing? _wuXing;
    public WuXing? WuXing => _wuXing;

    private Func<Tile, RunChip, bool> _canPlug;
    private Action<Tile, RunChip> _plug;
    private Func<AcquiredRunChip, bool> _canUnplug;
    private Action<AcquiredRunChip> _unplug;

    public ChipEntry(string name,
        CLLibrary.Range jingJieRange,
        ChipDescription description,
        WuXing? wuXing,
        Func<Tile, RunChip, bool> canPlug,
        Action<Tile, RunChip> plug,
        Func<AcquiredRunChip, bool> canUnplug,
        Action<AcquiredRunChip> unplug
        ) : base(name)
    {
        _jingJieRange = jingJieRange;
        _description = description;
        _wuXing = wuXing;
        _canPlug = canPlug;
        _plug = plug;
        _canUnplug = canUnplug;
        _unplug = unplug;
    }

    public bool CanPlug(Tile tile, RunChip runChip) => _canPlug(tile, runChip);
    public void Plug(Tile tile, RunChip runChip) => _plug(tile, runChip);
    public bool CanUnplug(AcquiredRunChip acquiredRunChip) => _canUnplug(acquiredRunChip);
    public void Unplug(AcquiredRunChip acquiredRunChip) => _unplug(acquiredRunChip);

    public static implicit operator ChipEntry(string name) => Encyclopedia.ChipCategory[name];
}
