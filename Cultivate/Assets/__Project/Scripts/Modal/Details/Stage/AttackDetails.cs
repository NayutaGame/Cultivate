
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

    public StageSkill SrcSkill;
    public WuXing? WuXing;
    public bool Crit;
    public bool LifeSteal;
    public bool Penetrate;
    public bool Evade;
    public bool Recursive;
    public Func<AttackDetails, CastResult, Task> WilAttack;
    public Func<AttackDetails, CastResult, Task> DidAttack;
    public Func<DamageDetails, CastResult, Task> WilDamage;
    public Func<DamageDetails, CastResult, Task> Undamaged;
    public Func<DamageDetails, CastResult, Task> DidDamage;

    public AttackDetails(
        StageEntity src,
        StageEntity tgt,
        int value,
        StageSkill srcSkill,
        WuXing? wuxing,
        bool crit = false,
        bool lifeSteal = false,
        bool penetrate = false,
        bool evade = false,
        bool recursive = true,
        Func<AttackDetails, CastResult, Task> wilAttack = null,
        Func<AttackDetails, CastResult, Task> didAttack = null,
        Func<DamageDetails, CastResult, Task> wilDamage = null,
        Func<DamageDetails, CastResult, Task> undamaged = null,
        Func<DamageDetails, CastResult, Task> didDamage = null)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        SrcSkill = srcSkill;
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
        SrcSkill,
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
