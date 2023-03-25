using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class StageWaiGong
{
    private StageEntity _owner;
    private RunChip _runChip;
    private Action<StageEntity, StageWaiGong, bool> _execute;

    private int _slotIndex;
    public int SlotIndex => _slotIndex;

    public bool Consumed;
    public int GetManaCost()
    {
        if (_runChip == null) // 聚气术
            return 0;

        var entry = _runChip._entry as WaiGongEntry;
        return entry.GetManaCost(Level, _powers);
    }

    // public int RunLevel { get; private set; }
    public int Level { get; private set; }
    public int RunUsedTimes { get; private set; }
    public int RunEquippedTimes { get; private set; }
    public int StageUsedTimes { get; private set; }

    // run powers

    private static readonly string[] PowerBuffNames = new string[] { "临金", "临水", "临木", "临火", "临土" };

    private int[] _powers;
    public int GetPower(WuXing wuXing) => _powers[wuXing] + _owner.GetStackOfBuff(PowerBuffNames[wuXing]);

    public StageWaiGong(StageEntity owner, RunChip runChip, int[] powers, int slotIndex)
    {
        _owner = owner;
        _runChip = runChip;
        _slotIndex = slotIndex;

        if (_runChip != null)
        {
            _execute = (_runChip._entry as WaiGongEntry).Execute;
            Consumed = false;
            // RunLevel = _runChip.Level;
            Level = _runChip.Level;
            RunUsedTimes = _runChip.RunUsedTimes;
            RunEquippedTimes = _runChip.RunEquippedTimes + 1;
            StageUsedTimes = 0;

            _powers = new int[WuXing.Length];
            WuXing.Traversal.Do(wuXing => _powers[wuXing] = powers[wuXing]);
        }
        else
        {
            _execute = (Encyclopedia.ChipCategory["聚气术"] as WaiGongEntry).Execute;
            Consumed = false;
            // RunLevel = 0;
            Level = 0;
            RunUsedTimes = 0;
            RunEquippedTimes = 0;
            StageUsedTimes = 0;

            _powers = new int[WuXing.Length];
            WuXing.Traversal.Do(wuXing => _powers[wuXing] = powers[wuXing]);
        }
    }

    public string GetName()
    {
        if (_runChip == null) return "聚气术";
        return _runChip.GetName();
    }

    public WaiGongEntry.WaiGongType GetWaiGongType() => (_runChip._entry as WaiGongEntry).Type;

    public void Execute(StageEntity caster, bool recursive = true)
    {
        _execute(caster, this, recursive);
        RunUsedTimes += 1;
        StageUsedTimes += 1;
    }

    public StageWaiGong Next()
        => _owner._waiGongList[(_slotIndex + 1) % _owner._waiGongList.Length];

    public StageWaiGong Prev()
        => _owner._waiGongList[(_slotIndex + _owner._waiGongList.Length - 1) % _owner._waiGongList.Length];

    public void Register()
    {
        WaiGongEntry entry = _runChip?._entry as WaiGongEntry;
        if (entry == null) return;

        if (entry._startStage != null) _owner.StartStageEvent += StartStage;
    }

    public void Unregister()
    {
        WaiGongEntry entry = _runChip?._entry as WaiGongEntry;
        if (entry == null) return;

        if (entry._startStage != null) _owner.StartStageEvent -= StartStage;
    }

    private void StartStage()
    {
        WaiGongEntry entry = _runChip?._entry as WaiGongEntry;
        if (entry == null) return;

        entry._startStage(_owner, this);
    }
}
