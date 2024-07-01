
using System;
using System.Threading.Tasks;

public class DamageDetails : EventDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Crit;
    public bool LifeSteal;
    public bool Recursive;

    public Func<DamageDetails, Task> WillDamage;
    public Func<DamageDetails, Task> Undamaged;
    public Func<DamageDetails, Task> DidDamage;

    public DamageDetails(StageEntity src, StageEntity tgt, int value,
        bool crit = false,
        bool lifeSteal = false,
        bool recursive = true,
        Func<DamageDetails, Task> willDamage = null,
        Func<DamageDetails, Task> undamaged = null,
        Func<DamageDetails, Task> didDamage = null)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Crit = crit;
        LifeSteal = lifeSteal;
        Recursive = recursive;

        WillDamage = willDamage;
        Undamaged = undamaged;
        DidDamage = didDamage;
    }

    public DamageDetails Clone() => new(Src, Tgt, Value, Crit, LifeSteal, Recursive, WillDamage, Undamaged, DidDamage);
}
