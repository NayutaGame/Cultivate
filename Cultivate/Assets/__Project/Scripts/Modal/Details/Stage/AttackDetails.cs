using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class AttackDetails : EventDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }

    public WuXing? WuXing;

    public bool LifeSteal;
    public bool Pierce;
    public bool Crit;
    public bool Evade;

    public bool Recursive;
    public Func<DamageDetails, Task> Damaged;
    public Func<DamageDetails, Task> Undamaged;

    public AttackDetails(StageEntity src, StageEntity tgt, int value,
        WuXing? wuxing,
        bool lifeSteal = false,
        bool pierce = false,
        bool crit = false,
        bool evade = false,
        bool recursive = true,
        Func<DamageDetails, Task> damaged = null,
        Func<DamageDetails, Task> undamaged = null)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        WuXing = wuxing;
        LifeSteal = lifeSteal;
        Pierce = pierce;
        Crit = crit;
        Evade = evade;
        Recursive = recursive;
        Damaged = damaged;
        Undamaged = undamaged;
    }

    public AttackDetails Clone() => new(Src, Tgt, _value, WuXing, LifeSteal, Pierce, Crit, Evade, Recursive, Damaged, Undamaged);
}
