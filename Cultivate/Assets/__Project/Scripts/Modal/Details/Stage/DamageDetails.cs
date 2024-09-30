
public class DamageDetails : ClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public StageSkill SrcSkill;
    public bool Crit;
    public bool LifeSteal;
    public bool Recursive;
    public CastResult CastResult;

    public DamageDetails(StageEntity src, StageEntity tgt, int value,
        StageSkill srcSkill,
        bool crit = false,
        bool lifeSteal = false,
        bool recursive = true,
        CastResult castResult = null)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        SrcSkill = srcSkill;
        Crit = crit;
        LifeSteal = lifeSteal;
        Recursive = recursive;
        CastResult = castResult;
    }

    public DamageDetails ShallowClone() => new(Src, Tgt, Value, SrcSkill, Crit, LifeSteal, Recursive, CastResult);

    public static DamageDetails FromAttackDetails(AttackDetails d)
        => new(d.Src, d.Tgt, d.Value, d.SrcSkill, crit: d.Crit, lifeSteal: d.LifeSteal, castResult: d.CastResult);

    public static DamageDetails FromAttackDetailsUndamaged(AttackDetails d)
        => new(d.Src, d.Tgt, 0, d.SrcSkill, crit: d.Crit, lifeSteal: d.LifeSteal, castResult: d.CastResult);
}
