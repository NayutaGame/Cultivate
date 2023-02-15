using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNeiGong
{
    private RunChip _runChip;

    public StageNeiGong(RunChip runChip)
    {
        _runChip = runChip;

        if (_runChip != null)
        {
            // register
        }
    }

    public string GetName()
    {
        if (_runChip == null) return "空";
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
}
