using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class StageWaiGong
{
    private RunChip _runChip;
    private Action<StringBuilder, StageEntity, StageWaiGong> _execute;

    public bool Consumed;

    public int Level { get; private set; }
    public int RunUsedTimes { get; private set; }
    public int RunEquippedTimes { get; private set; }
    public int StageUsedTimes { get; private set; }

    private int[] _powers;
    public int GetPower(WuXing wuXing) => _powers[wuXing];
    public void SetPower(WuXing wuXing, int value) => _powers[wuXing] = value;

    public StageWaiGong(RunChip runChip)
    {
        _runChip = runChip;

        if (_runChip != null)
        {
            _execute = (_runChip._entry as WaigongEntry).Execute;
            Consumed = false;
            Level = _runChip.Level;
            RunUsedTimes = _runChip.RunUsedTimes;
            RunEquippedTimes = _runChip.RunEquippedTimes + 1;
            StageUsedTimes = 0;

            _powers = new int[WuXing.Length];
            foreach (var wuXing in WuXing.Traversal)
            {
                _powers[wuXing] = _runChip.GetPower(wuXing);
            }
        }
        else
        {
            _execute = (Encyclopedia.ChipCategory["聚气术"] as WaigongEntry).Execute;
            Consumed = false;
            Level = 0;
            RunUsedTimes = 0;
            RunEquippedTimes = 0;
            StageUsedTimes = 0;
            _powers = new int[] { 0, 0, 0, 0, 0 };
        }
    }

    public string GetName()
    {
        if (_runChip == null) return "聚气术";
        return _runChip.GetName();
    }

    public void Upgrade(StringBuilder seq)
    {
        Level += 1;
    }

    public void Execute(StringBuilder seq, StageEntity caster)
    {
        _execute(seq, caster, this);
        RunUsedTimes += 1;
        StageUsedTimes += 1;
    }
}
