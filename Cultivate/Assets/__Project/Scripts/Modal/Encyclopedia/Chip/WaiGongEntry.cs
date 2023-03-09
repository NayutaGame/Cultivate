using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class WaiGongEntry : ChipEntry
{
    private Func<int, int[], int> _manaCost;
    public int GetManaCost(int level, int[] powers) => _manaCost(level, powers);

    private Action<StringBuilder, StageEntity, StageWaiGong> _execute;

    public WaiGongEntry(
        string name,
        JingJie jingJie,
        string description,
        int manaCost = 0,
        Action<StringBuilder, StageEntity, StageWaiGong> execute = null) : this(name, jingJie, description, (level, powers) => manaCost, execute = null)
    {}

    public WaiGongEntry(
        string name,
        JingJie jingJie,
        string description,
        Func<int, int[], int> manaCost,
        Action<StringBuilder, StageEntity, StageWaiGong> execute = null) : base(name, jingJie, description,
        canPlug: (tile, runChip) => tile.AcquiredRunChip == null,
        plug: (tile, runChip) =>
        {
            AcquiredRunChip acquiredRunChip = new AcquiredRunChip(tile, runChip);
            tile.AcquiredRunChip = acquiredRunChip;
            RunManager.Instance.AcquiredInventory.Add(acquiredRunChip);
            RunManager.Instance.ChipInventory.Remove(runChip);
        },
        canUnplug: acquiredRunChip => true,
        unplug: acquiredRunChip =>
        {
            acquiredRunChip.Tile.AcquiredRunChip = null;
            int? idx = RunManager.Instance.Hero.HeroSlotInventory.FindAcquiredIdx(acquiredRunChip);
            if (idx != null)
            {
                RunManager.Instance.Hero.HeroSlotInventory[idx.Value].AcquiredRunChip = null;
            }
            else
            {

                RunManager.Instance.AcquiredInventory.Remove(acquiredRunChip);
            }

            RunManager.Instance.ChipInventory.Add(acquiredRunChip.Chip);
        })
    {
        _manaCost = manaCost;
        _execute = execute ?? DefaultExecute;
    }

    public void Execute(StringBuilder seq, StageEntity caster, StageWaiGong waiGong)
    {
        seq.Append($"{caster.GetName()}使用了{Name}");
        _execute(seq, caster, waiGong);
        seq.Append($"\n");
    }

    private void DefaultExecute(StringBuilder seq, StageEntity caster, StageWaiGong waiGong)
    {

    }
}
