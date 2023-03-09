using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipEntry : Entry
{
    private JingJie _jingJie;
    public JingJie JingJie => _jingJie;

    private string _description;
    public string Description => _description;

    private Func<Tile, RunChip, bool> _canPlug;
    private Action<Tile, RunChip> _plug;
    private Func<AcquiredRunChip, bool> _canUnplug;
    private Action<AcquiredRunChip> _unplug;

    public ChipEntry(string name, JingJie jingJie, string description,
        Func<Tile, RunChip, bool> canPlug,
        Action<Tile, RunChip> plug,
        Func<AcquiredRunChip, bool> canUnplug,
        Action<AcquiredRunChip> unplug
    ) : base(name)
    {
        _jingJie = jingJie;
        _description = description;
        _canPlug = canPlug;
        _plug = plug;
        _canUnplug = canUnplug;
        _unplug = unplug;
    }

    public bool CanPlug(Tile tile, RunChip runChip) => _canPlug(tile, runChip);
    public void Plug(Tile tile, RunChip runChip) => _plug(tile, runChip);
    public bool CanUnplug(AcquiredRunChip acquiredRunChip) => _canUnplug(acquiredRunChip);
    public void Unplug(AcquiredRunChip acquiredRunChip) => _unplug(acquiredRunChip);
}
