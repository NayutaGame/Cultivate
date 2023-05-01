using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEditor.Experimental;
using UnityEngine;

public class WaiGongEntry : ChipEntry
{
    private ManaCost _manaCost;
    public int GetManaCost(int level, JingJie jingJie, int dJingJie) => _manaCost.Eval(level, jingJie, dJingJie);
    public int GetManaCost(int level, JingJie jingJie, int dJingJie, int[] powers) => _manaCost.Eval(level, jingJie, dJingJie, powers);

    public SkillTypeCollection SkillTypeCollection { get; private set; }
    private Func<StageEntity, StageWaiGong, bool, Task> _execute;

    public WaiGongEntry(
        string name,
        CLLibrary.Range jingJieRange,
        ChipDescription description,
        WuXing? wuXing = null,
        ManaCost manaCost = null,
        SkillTypeCollection skillTypeCollection = null,
        Func<StageEntity, StageWaiGong, bool, Task> execute = null
    ) : base(name, jingJieRange, description, wuXing,
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
        _manaCost = manaCost ?? 0;
        SkillTypeCollection = skillTypeCollection ?? 0x00000000;
        _execute = execute ?? DefaultExecute;
    }

    public async Task Execute(StageEntity caster, StageWaiGong waiGong, bool recursive)
    {
        StageReport r = caster.Env.Report;
        if (r.UseTween)
            await r.PlayTween(new ShiftTweenDescriptor());
        r.Append($"{caster.GetName()}使用了{Name}");
        r.AppendNote(caster.Index, waiGong);
        await _execute(caster, waiGong, recursive);
        r.Append($"\n");
    }

    private async Task DefaultExecute(StageEntity caster, StageWaiGong waiGong, bool recursive) { }
}
