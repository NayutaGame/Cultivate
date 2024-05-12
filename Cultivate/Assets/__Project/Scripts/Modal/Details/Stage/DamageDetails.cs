
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

    public Func<DamageDetails, Task> Damaged;
    public Func<DamageDetails, Task> Undamaged;

    public DamageDetails(StageEntity src, StageEntity tgt, int value,
        bool crit = false,
        bool lifeSteal = false,
        bool recursive = true,
        Func<DamageDetails, Task> damaged = null,
        Func<DamageDetails, Task> undamaged = null)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Crit = crit;
        LifeSteal = lifeSteal;
        Recursive = recursive;

        Damaged = damaged;
        Undamaged = undamaged;
    }

    public DamageDetails Clone() => new(Src, Tgt, Value, Crit, LifeSteal, Recursive, Damaged, Undamaged);
}
