using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class WaigongEntry : ChipEntry
{
    private string[] _descriptions;
    private int[] _manaCosts;
    public readonly Action<StringBuilder, StageEntity> _execute;

    public WaigongEntry(string name, string description, int manaCost = 0,
        Action<StringBuilder, StageEntity> execute = null) : this(name, new[] { description, description, description },
        new int[] { manaCost, manaCost, manaCost }, execute) { }

    public WaigongEntry(string name, string[] descriptions, int manaCost = 0,
        Action<StringBuilder, StageEntity> execute = null) : this(name, descriptions,
        new int[] { manaCost, manaCost, manaCost }, execute) { }

    public WaigongEntry(string name, string description, int[] manaCosts,
        Action<StringBuilder, StageEntity> execute = null) : this(name, new[] { description, description, description },
        manaCosts, execute) { }

    public WaigongEntry(string name, string[] descriptions, int[] manaCosts = null, Action<StringBuilder, StageEntity> execute = null) : base(name)
    {
        _descriptions = descriptions;
        _manaCosts = manaCosts ?? new []{0, 0, 0};
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
            seq.Append($"{caster.GetName()}使用了{Name}");
            _execute(seq, caster);
            seq.Append($"\n");
        }
    }

    public override bool IsWaiGong => true;
}
