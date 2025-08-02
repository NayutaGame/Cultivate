
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
        StageClosureListener listener,
        WuXing wuxing,
        bool crit,
        bool lifeSteal,
        bool penetrate,
        bool doesntConsumeJianYi,
        bool shatter,
        bool evade,
        bool recursive,
        ResultDict castResult,
        StageClosure[] closures,
        bool induced) : base(env, induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Times = times;
        Listener = listener;
        WuXing = wuxing;
        Crit = crit;
        LifeSteal = lifeSteal;
        Penetrate = penetrate;
        DoesntConsumeJianYi = doesntConsumeJianYi;
        Shatter = shatter;
        Evade = evade;
        Recursive = recursive;
        CastResult = castResult;
        Closures = closures ?? Array.Empty<StageClosure>();
    }

    public AttackDetails ShallowClone() => new(
        Env,
        Src,
        Tgt,
        Value,
        Times,
        Listener,
        WuXing,
        Crit,
        LifeSteal,
        Penetrate,
        DoesntConsumeJianYi,
        Shatter,
        Evade,
        Recursive,
        CastResult,
        Closures,
        Induced);
}
