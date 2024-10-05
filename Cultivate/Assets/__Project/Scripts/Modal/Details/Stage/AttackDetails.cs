
using System;
using UnityEngine;

public class AttackDetails : ClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }

    public int Times;
    public StageSkill SrcSkill;
    public WuXing? WuXing;
    public bool Crit;
    public bool LifeSteal;
    public bool Penetrate;
    public bool Evade;
    public bool Recursive;
    public CastResult CastResult;
    public StageClosure[] Closures;

    /// <summary>
    /// 一次攻击行为的细节
    /// </summary>
    /// <param name="src">攻击者</param>
    /// <param name="tgt">受攻击者</param>
    /// <param name="value">攻击数值</param>
    /// <param name="times">攻击次数</param>
    /// <param name="srcSkill">技能来源</param>
    /// <param name="castResult">结果描述</param>
    /// <param name="wuXing">攻击特效的五行</param>
    /// <param name="crit">是否吸血</param>
    /// <param name="lifeSteal">是否吸血</param>
    /// <param name="penetrate">是否穿透</param>
    /// <param name="evade">是否闪避</param>
    /// <param name="closures">额外行为</param>
    /// <param name="recursive">是否会递归</param>
    /// <param name="induced">该行为是间接行为，不会引起额外的角色动画</param>
    public AttackDetails(
        StageEntity src,
        StageEntity tgt,
        int value,
        int times,
        StageSkill srcSkill,
        WuXing? wuxing,
        bool crit,
        bool lifeSteal,
        bool penetrate,
        bool evade,
        bool recursive,
        CastResult castResult,
        StageClosure[] closures,
        bool induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Times = times;
        SrcSkill = srcSkill;
        WuXing = wuxing;
        Crit = crit;
        LifeSteal = lifeSteal;
        Penetrate = penetrate;
        Evade = evade;
        Recursive = recursive;
        CastResult = castResult;
        Closures = closures ?? Array.Empty<StageClosure>();
        Induced = induced;
    }

    public AttackDetails ShallowClone() => new(
        Src,
        Tgt,
        Value,
        Times,
        SrcSkill,
        WuXing,
        Crit,
        LifeSteal,
        Penetrate,
        Evade,
        Recursive,
        CastResult,
        Closures,
        Induced);
}
