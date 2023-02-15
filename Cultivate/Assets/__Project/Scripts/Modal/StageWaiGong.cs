using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StageWaiGong
{
    private RunChip _runChip;
    private Action<Sequence, StageEntity, StageEntity> _execute;

    public StageWaiGong(RunChip runChip)
    {
        _runChip = runChip;

        if (_runChip != null)
        {
            _execute = (_runChip._entry as WaigongEntry).Execute;
        }
        else
        {
            _execute = (Encyclopedia.ChipCategory.Find("聚气术") as WaigongEntry).Execute;
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
    public void Execute(Sequence seq, StageEntity src, StageEntity tgt) => _execute(seq, src, tgt);
}
