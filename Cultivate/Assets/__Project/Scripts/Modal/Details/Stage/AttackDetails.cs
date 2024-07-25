
using System;
using System.Threading.Tasks;
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

    public bool Crit;
    public bool LifeSteal;
    public bool Penetrate;
    public bool Evade;

    public bool Recursive;
    
    public Func<DamageDetails, Task> WillDamage;
    public Func<DamageDetails, Task> Undamaged;
    public Func<DamageDetails, Task> DidDamage;

    public bool FromSeamless;

    public AttackDetails(StageEntity src, StageEntity tgt, int value,
        WuXing? wuxing,
        bool crit = false,
        bool lifeSteal = false,
        bool penetrate = false,
        bool evade = false,
        bool recursive = true,
        Func<DamageDetails, Task> willDamage = null,
        Func<DamageDetails, Task> undamaged = null,
        Func<DamageDetails, Task> didDamage = null,
        bool fromSeamless = false)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        WuXing = wuxing;
        Crit = crit;
        LifeSteal = lifeSteal;
        Penetrate = penetrate;
        Evade = evade;
        Recursive = recursive;
        WillDamage = willDamage;
        Undamaged = undamaged;
        DidDamage = didDamage;
        FromSeamless = fromSeamless;
    }

    public AttackDetails Clone() => new(Src, Tgt, _value, WuXing, Crit, LifeSteal, Penetrate, Evade, Recursive, WillDamage, Undamaged, DidDamage, FromSeamless);
}
