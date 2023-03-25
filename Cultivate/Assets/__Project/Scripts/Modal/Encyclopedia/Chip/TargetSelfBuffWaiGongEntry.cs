using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelfBuffWaiGongEntry : WaiGongEntry
{
    public TargetSelfBuffWaiGongEntry(string name, JingJie jingJie, string description, int manaCost, WaiGongType type, string buffName, int buffStack = 1)
        : this(name, jingJie, description, (level, powers) => manaCost, type, buffName, buffStack) { }
    public TargetSelfBuffWaiGongEntry(string name, JingJie jingJie, string description, Func<int, int[], int> manaCost, WaiGongType type, string buffName, int buffStack = 1)
        : base(name, jingJie, description, manaCost, type,
            execute: (caster, waiGong, recursive) =>
                StageManager.Instance.BuffProcedure(caster, caster, buffName, buffStack))
    {
    }
}
