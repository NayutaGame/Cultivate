using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class WaigongEntry : ChipEntry
{
    public readonly int ManaCost;
    public readonly Action<StringBuilder, StageEntity> _execute;

    public WaigongEntry(string name, string description, int manaCost = 0, Action<StringBuilder, StageEntity> execute = null) : base(name, description)
    {
        ManaCost = manaCost;
        _execute = execute;
    }

    public void Execute(StringBuilder seq, StageEntity caster)
    {
        if (_execute == null)
        {
            // Debug.Log($"{Name}, not implemented yet");
        }
        else
        {
            // Debug.Log($"Executing {Name} => {Description}");
            _execute(seq, caster);
        }
    }
}
