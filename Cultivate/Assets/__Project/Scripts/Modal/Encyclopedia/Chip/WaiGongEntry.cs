using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class WaiGongEntry : ChipEntry
{
    private Func<int, int[], int> _manaCost;
    public int GetManaCost(int level) => GetManaCost(level, new int[] { 0, 0, 0, 0, 0 });
    public int GetManaCost(int level, int[] powers) => _manaCost(level, powers);

    public WaiGongType Type { get; private set; }
    private Action<StageEntity, StageWaiGong, bool> _execute;
    public Action<StageEntity, StageWaiGong> _startStage;

    public WaiGongEntry(
        string name,
        JingJie jingJie,
        string description,
        int manaCost = 0,
        WaiGongType type = WaiGongType.NONATTACK,
        Action<StageEntity, StageWaiGong, bool> execute = null,
        Action<StageEntity, StageWaiGong> startStage = null
        ) : this(name, jingJie, description, (level, powers) => manaCost, type, execute, startStage)
    {}

    public WaiGongEntry(
        string name,
        JingJie jingJie,
        string description,
        Func<int, int[], int> manaCost,
        WaiGongType type = WaiGongType.NONATTACK,
        Action<StageEntity, StageWaiGong, bool> execute = null,
        Action<StageEntity, StageWaiGong> startStage = null
    ) : base(name, jingJie, description,
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
        Type = type;
        _execute = execute ?? DefaultExecute;
        _startStage = startStage;
    }

    public void Execute(StageEntity caster, StageWaiGong waiGong, bool recursive)
    {
        StageManager.Instance.Report.Append($"{caster.GetName()}使用了{Name}");
        _execute(caster, waiGong, recursive);
        StageManager.Instance.Report.Append($"\n");
    }

    private void DefaultExecute(StageEntity caster, StageWaiGong waiGong, bool recursive) { }

    public enum WaiGongType
    {
        ATTACK,
        NONATTACK,
    }
}
