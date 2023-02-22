using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class WaigongEntry : ChipEntry
{
    private string[] _descriptions;
    public string GetDescription(int level) => _descriptions[level];
    private int[] _manaCosts;
    public readonly Action<StringBuilder, StageEntity, StageWaiGong> _execute;

    public WaigongEntry(string name, string description, int manaCost = 0,
        Action<StringBuilder, StageEntity, StageWaiGong> execute = null) : this(name, new[] { description, description, description },
        new int[] { manaCost, manaCost, manaCost }, execute) { }

    public WaigongEntry(string name, string[] descriptions, int manaCost = 0,
        Action<StringBuilder, StageEntity, StageWaiGong> execute = null) : this(name, descriptions,
        new int[] { manaCost, manaCost, manaCost }, execute) { }

    public WaigongEntry(string name, string description, int[] manaCosts,
        Action<StringBuilder, StageEntity, StageWaiGong> execute = null) : this(name, new[] { description, description, description },
        manaCosts, execute) { }

    public WaigongEntry(string name, string[] descriptions, int[] manaCosts = null, Action<StringBuilder, StageEntity, StageWaiGong> execute = null) : base(name)
    {
        _descriptions = descriptions;
        _manaCosts = manaCosts ?? new []{0, 0, 0};
        _execute = execute;
    }

    public void Execute(StringBuilder seq, StageEntity caster, StageWaiGong waiGong)
    {
        if (_execute == null)
        {
            // Debug.Log($"{Name}, not implemented yet");
        }
        else
        {
            seq.Append($"{caster.GetName()}使用了{Name}");
            _execute(seq, caster, waiGong);
            seq.Append($"\n");
        }
    }

    public override bool IsWaiGong => true;
}
