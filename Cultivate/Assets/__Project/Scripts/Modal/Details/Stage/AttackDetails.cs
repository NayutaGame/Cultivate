
using System;
using UnityEngine;

public class AttackDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }

    private int _times;
    public int Times
    {
        get => _times;
        set => _times = Mathf.Max(1, value);
    }

    public WuXing WuXing;
    public bool Crit;
    public bool LifeSteal;
    public bool Penetrate;
    public bool DoesntConsumeJianYi;
    public bool Shatter;
    public bool Evade;
    public bool Recursive;

    /// <summary>
    /// 一次攻击行为的细节
    /// </summary>
    /// <param name="src">攻击者</param>
    /// <param name="tgt">受攻击者</param>
    /// <param name="value">攻击数值</param>
    /// <param name="times">攻击次数</param>
    /// <param name="wuXing">攻击特效的五行</param>
    /// <param name="crit">是否吸血</param>
    /// <param name="lifeSteal">是否吸血</param>
    /// <param name="penetrate">是否穿透</param>
    /// <param name="doesntConsumeJianYi">是否保存剑意</param>
    /// <param name="shatter">是否碎防</param>
    /// <param name="evade">是否闪避</param>
    /// <param name="recursive">是否会递归</param>
    /// <param name="listener">技能来源</param>
    /// <param name="closures">额外行为</param>
    /// <param name="castResult">结果描述</param>
    /// <param name="induced">该行为是间接行为，不会引起额外的角色动画</param>
    public AttackDetails(
        StageEnvironment env,
        StageEntity src,
        StageEntity tgt,
        int value,
        int times,
        WuXing wuXing,
        bool crit,
        bool lifeSteal,
        bool penetrate,
        bool doesntConsumeJianYi,
        bool shatter,
        bool evade,
        bool recursive,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool closureHasRegistered,
        bool induced) : base(env, listener, closures, castResult, closureHasRegistered, induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Times = times;
        WuXing = wuXing;
        Crit = crit;
        LifeSteal = lifeSteal;
        Penetrate = penetrate;
        DoesntConsumeJianYi = doesntConsumeJianYi;
        Shatter = shatter;
        Evade = evade;
        Recursive = recursive;
    }

    public AttackDetails ShallowClone() => new(
        Env,
        Src,
        Tgt,
        Value,
        Times,
        WuXing,
        Crit,
        LifeSteal,
        Penetrate,
        DoesntConsumeJianYi,
        Shatter,
        Evade,
        Recursive,
        Listener,
        Closures,
        CastResult,
        ClosureHasRegistered,
        Induced);

    public static AttackDetails FromAttackProcedureDefinition(AttackProcedureDefinition pd, CastDetails d)
        => new(
            env: d.Env,
            src: d.Caster,
            tgt: d.Caster.Opponent(),
            value: pd.Value,
            times: pd.Times,
            wuXing: pd.WuXing ?? d.Skill.Entry.WuXing,
            crit: false,
            lifeSteal: false,
            penetrate: false,
            doesntConsumeJianYi: false,
            shatter: false,
            evade: false,
            recursive: pd.Recursive,
            listener: d.Skill,
            closures: pd.ClosuresArray,
            castResult: d.CastResult,
            closureHasRegistered: false,
            induced: pd.Induced);

    public static AttackDetails FromCastDetails(CastDetails d, int value, int times, WuXing wuXing, bool recursive, StageClosure[] closures, bool induced)
        => new(
            env: d.Env,
            src: d.Caster,
            tgt: d.Caster.Opponent(),
            value: value,
            times: times,
            wuXing: wuXing ?? d.Skill.Entry.WuXing,
            crit: false,
            lifeSteal: false,
            penetrate: false,
            doesntConsumeJianYi: false,
            shatter: false,
            evade: false,
            recursive: recursive,
            listener: d.Skill,
            closures: closures,
            castResult: d.CastResult,
            closureHasRegistered: false,
            induced: induced);

    public static AttackDetails FromEntity(StageEntity e, int value, int times, WuXing wuXing, bool recursive, StageClosureListener listener, StageClosure[] closures, bool induced)
        => new(
            env: e.Env,
            src: e,
            tgt: e.Opponent(),
            value: value,
            times: times,
            wuXing: wuXing ?? WuXing.Wu,
            crit: false,
            lifeSteal: false,
            penetrate: false,
            doesntConsumeJianYi: false,
            shatter: false,
            evade: false,
            recursive: recursive,
            listener: listener,
            closures: closures,
            castResult: null,
            closureHasRegistered: false,
            induced: induced);
}
