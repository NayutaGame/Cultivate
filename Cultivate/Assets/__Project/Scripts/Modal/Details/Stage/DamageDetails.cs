
using System;
using System.Threading.Tasks;

public class DamageDetails : EventDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public StageSkill SrcSkill;
    public bool Crit;
    public bool LifeSteal;
    public bool Recursive;

    public Func<DamageDetails, CastResult, Task> WilDamage;
    public Func<DamageDetails, CastResult, Task> Undamaged;
    public Func<DamageDetails, CastResult, Task> DidDamage;

    public DamageDetails(StageEntity src, StageEntity tgt, int value,
        StageSkill srcSkill,
        bool crit = false,
        bool lifeSteal = false,
        bool recursive = true,
        Func<DamageDetails, CastResult, Task> wilDamage = null,
        Func<DamageDetails, CastResult, Task> undamaged = null,
        Func<DamageDetails, CastResult, Task> didDamage = null)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        SrcSkill = srcSkill;
        Crit = crit;
        LifeSteal = lifeSteal;
        Recursive = recursive;

        WilDamage = wilDamage;
        Undamaged = undamaged;
        DidDamage = didDamage;
    }

    public DamageDetails Clone() => new(Src, Tgt, Value, SrcSkill, Crit, LifeSteal, Recursive, WilDamage, Undamaged, DidDamage);
}
