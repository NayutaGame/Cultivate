
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

    public AttackDetails(
        StageEntity src,
        StageEntity tgt,
        int value,
        int times,
        StageSkill srcSkill,
        WuXing? wuxing,
        bool crit = false,
        bool lifeSteal = false,
        bool penetrate = false,
        bool evade = false,
        bool recursive = true,
        CastResult castResult = null,
        StageClosure[] closures = null)
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
        Closures);
}
