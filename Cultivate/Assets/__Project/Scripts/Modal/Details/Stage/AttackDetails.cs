
using System;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
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
    public Func<AttackDetails, Task> WilAttack;
    public Func<AttackDetails, Task> DidAttack;
    public Func<DamageDetails, Task> WilDamage;
    public Func<DamageDetails, Task> Undamaged;
    public Func<DamageDetails, Task> DidDamage;

    public AttackDetails(
        StageEntity src,
        StageEntity tgt,
        int value,
        WuXing? wuxing,
        bool crit = false,
        bool lifeSteal = false,
        bool penetrate = false,
        bool evade = false,
        bool recursive = true,
        Func<AttackDetails, Task> wilAttack = null,
        Func<AttackDetails, Task> didAttack = null,
        Func<DamageDetails, Task> wilDamage = null,
        Func<DamageDetails, Task> undamaged = null,
        Func<DamageDetails, Task> didDamage = null)
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
        WilAttack = wilAttack;
        DidAttack = didAttack;
        WilDamage = wilDamage;
        Undamaged = undamaged;
        DidDamage = didDamage;
    }

    public AttackDetails Clone() => new(
        Src,
        Tgt,
        Value,
        WuXing,
        Crit,
        LifeSteal,
        Penetrate,
        Evade,
        Recursive,
        WilAttack,
        DidAttack,
        WilDamage,
        Undamaged,
        DidDamage);
}
