using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageWaiGong
{
    private RunChip _runChip;
    private Action _execute;

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

    public void Execute() => _execute();
}
