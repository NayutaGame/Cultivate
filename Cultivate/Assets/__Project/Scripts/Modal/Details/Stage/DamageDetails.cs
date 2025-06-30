
using UnityEngine;

public class DamageDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }
    public bool Crit;
    public bool LifeSteal;
    public bool CausedByAttack;
    public bool Recursive;

    /// <summary>
    /// 描述一次伤害行为的细节，不会结算目标的护甲
    /// </summary>
    /// <param name="src">伤害者</param>
    /// <param name="tgt">受伤害者</param>
    /// <param name="value">伤害数值</param>
    /// <param name="crit">是否暴击</param>
    /// <param name="lifeSteal">是否吸血</param>
    /// <param name="causedByAttack">由攻击造成的伤害</param>
    /// <param name="recursive">是否会递归</param>
    /// <param name="listener">技能来源</param>
    /// <param name="closures">额外行为</param>
    /// <param name="castResult">结果描述</param>
    /// <param name="induced">是否是间接行为</param>
    public DamageDetails(
        StageEnvironment env,
        StageEntity src,
        StageEntity tgt,
        int value,
        bool crit,
        bool lifeSteal,
        bool causedByAttack,
        bool recursive,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool induced) : base(env, induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Crit = crit;
        LifeSteal = lifeSteal;
        CausedByAttack = causedByAttack;
        Recursive = recursive;
        Listener = listener;
        Closures = closures;
        CastResult = castResult;
    }

    public static DamageDetails FromAttackDetails(AttackDetails d)
        => new(d.Env, d.Src, d.Tgt, d.Value, d.Crit, d.LifeSteal, true, d.Recursive, d.Listener, d.Closures, d.CastResult, d.Induced);

    public static DamageDetails FromIndirectDetails(IndirectDetails d)
        => new(d.Env, d.Src, d.Tgt, d.Value, false, d.LifeSteal, false, d.Recursive, d.SrcSkill, null, d.CastResult, d.Induced);

    public static DamageDetails FromAttackDetailsUndamaged(AttackDetails d)
        => new(d.Env, d.Src, d.Tgt, 0, d.Crit, d.LifeSteal, true, d.Recursive, d.Listener, d.Closures, d.CastResult, d.Induced);
    
    public static DamageDetails FromBurn(BurnDetails d)
        => new(d.Env, d.Owner, d.Owner, d.Value, false, false, false, true, null, null, null, d.Induced);
}
