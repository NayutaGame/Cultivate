using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class StageWaiGong
{
    private RunChip _runChip;
    private Action<StringBuilder, StageEntity> _execute;

    public bool Consumed;

    public StageWaiGong(RunChip runChip)
    {
        _runChip = runChip;
        Consumed = false;

        if (_runChip != null)
        {
            _execute = (_runChip._entry as WaigongEntry).Execute;
        }
        else
        {
            _execute = (Encyclopedia.ChipCategory["聚气术"] as WaigongEntry).Execute;
        }
    }

    public string GetName()
    {
        if (_runChip == null) return "聚气术";
        return _runChip.GetName();
    }

    public int Level
    {
        get
        {
            if (_runChip == null) return 0;
            return _runChip.Level;
        }
    }
    public void Execute(StringBuilder seq, StageEntity caster) => _execute(seq, caster);
}
