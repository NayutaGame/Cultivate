
public class DamageDetails : ClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public StageSkill SrcSkill;
    public bool Crit;
    public bool LifeSteal;
    public bool CausedByAttack;
    public bool Recursive;
    public CastResult CastResult;

    /// <summary>
    /// 描述一次伤害行为的细节，不会结算目标的护甲
    /// </summary>
    /// <param name="src">伤害者</param>
    /// <param name="tgt">受伤害者</param>
    /// <param name="value">伤害数值</param>
    /// <param name="srcSkill">技能来源</param>
    /// <param name="crit">是否暴击</param>
    /// <param name="lifeSteal">是否吸血</param>
    /// <param name="causedByAttack">由攻击造成的伤害</param>
    /// <param name="recursive">是否会递归</param>
    /// <param name="castResult">结果描述</param>
    /// <param name="induced">是否是间接行为</param>
    public DamageDetails(
        StageEntity src,
        StageEntity tgt,
        int value,
        StageSkill srcSkill,
        bool crit,
        bool lifeSteal,
        bool causedByAttack,
        bool recursive,
        CastResult castResult,
        bool induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        SrcSkill = srcSkill;
        Crit = crit;
        LifeSteal = lifeSteal;
        CausedByAttack = causedByAttack;
        Recursive = recursive;
        CastResult = castResult;
        Induced = induced;
    }

    public static DamageDetails FromAttackDetails(AttackDetails d)
        => new(d.Src, d.Tgt, d.Value, d.SrcSkill, d.Crit, d.LifeSteal, true, d.Recursive, d.CastResult, d.Induced);

    public static DamageDetails FromIndirectDetails(IndirectDetails d)
        => new(d.Src, d.Tgt, d.Value, d.SrcSkill, false, d.LifeSteal, false, d.Recursive, d.CastResult, d.Induced);

    public static DamageDetails FromAttackDetailsUndamaged(AttackDetails d)
        => new(d.Src, d.Tgt, 0, d.SrcSkill, d.Crit, d.LifeSteal, true, d.Recursive, d.CastResult, d.Induced);

    public static DamageDetails FromCostHealth(StageEntity src, int value, bool induced)
        => new(src, src, value, null, false, false, false, true, null, induced);
}
