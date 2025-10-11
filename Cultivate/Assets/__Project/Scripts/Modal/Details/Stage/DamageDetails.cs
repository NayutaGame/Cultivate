
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
    public bool IsCritical;
    
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
        bool closureHasRegistered,
        bool induced,
        bool isCritical) : base(env, listener, closures, castResult, closureHasRegistered, induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Crit = crit;
        LifeSteal = lifeSteal;
        CausedByAttack = causedByAttack;
        Recursive = recursive;
        IsCritical = isCritical;
    }

    public static DamageDetails FromCastDetails(CastDetails d, bool tgtIsSrc, int value, bool recursive, bool induced)
        => new(
            env: d.Env,
            src: d.Caster,
            tgt: tgtIsSrc ? d.Caster : d.Caster.Opponent(),
            value: value,
            crit: false,
            lifeSteal: false,
            causedByAttack: false,
            recursive: recursive,
            listener: d.Skill,
            closures: null,
            castResult: d.CastResult,
            closureHasRegistered: false,
            induced: induced,
            isCritical: false);
    
    public static DamageDetails FromEntity(StageEntity e, bool tgtIsSrc, int value, bool recursive, StageClosureListener listener, StageClosure[] closures, bool induced)
        => new(
            env: e.Env,
            src: e,
            tgt: tgtIsSrc ? e : e.Opponent(),
            value: value,
            crit: false,
            lifeSteal: false,
            causedByAttack: false,
            recursive: recursive,
            listener: listener,
            closures: closures,
            castResult: null,
            closureHasRegistered: false,
            induced: induced,
            isCritical: false);

    public static DamageDetails FromAttackDetails(AttackDetails d)
        => new(
            d.Env,
            d.Src,
            d.Tgt,
            d.Value,
            d.Crit,
            d.LifeSteal,
            true,
            d.Recursive,
            d.Listener,
            d.Closures,
            d.CastResult,
            d.ClosureHasRegistered,
            d.Induced,
            d.IsCritical);

    public static DamageDetails FromIndirectDetails(IndirectDetails d)
        => new(
            d.Env,
            d.Src,
            d.Tgt,
            d.Value,
            false,
            d.LifeSteal,
            false,
            d.Recursive,
            d.SrcSkill,
            null,
            d.CastResult,
            false,
            d.Induced,
            false);

    public static DamageDetails FromAttackDetailsUndamaged(AttackDetails d)
        => new(
            d.Env,
            d.Src,
            d.Tgt,
            0,
            d.Crit,
            d.LifeSteal,
            true,
            d.Recursive,
            d.Listener,
            d.Closures,
            d.CastResult,
            d.ClosureHasRegistered,
            d.Induced,
            d.IsCritical);
    
    public static DamageDetails FromBurn(BurnDetails d)
        => new(
            d.Env,
            d.Owner,
            d.Owner,
            d.Value,
            false,
            false,
            false,
            true,
            null,
            null,
            null,
            false,
            d.Induced,
            false);
}
