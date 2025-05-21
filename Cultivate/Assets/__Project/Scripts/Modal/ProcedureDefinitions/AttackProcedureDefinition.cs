
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

public class AttackProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public int Times;
    public WuXing? WuXing;
    public bool Recursive;
    public bool Induced;

    public AttackProcedureDefinition(int value,
        int times = 1,
        WuXing? wuXing = null,
        bool recursive = true,
        bool induced = false)
    {
        Value = value;
        Times = times;
        WuXing = wuXing;
        Recursive = recursive;
        Induced = induced;
    }

    protected AttackProcedureDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription,
        List<StageClosure> closures,
        int value,
        int times,
        WuXing? wuXing,
        bool recursive,
        bool induced) :
        base(preCondDefinition, postCondDefinition, getDescription, closures)
    {
        Value = value;
        Times = times;
        WuXing = wuXing;
        Recursive = recursive;
        Induced = induced;
    }

    public override ProcedureDefinition Clone()
    {
        List<StageClosure> clonedClosures = new List<StageClosure>();
        for (int i = 0; i < Closures.Count; i++)
            clonedClosures.Add(Closures[i]);

        return new AttackProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            value: Value,
            times: Times,
            wuXing: WuXing,
            recursive: Recursive,
            induced: Induced
        );
    }

    public AttackDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            src: d.Caster,
            tgt: d.Caster.Opponent(),
            value: Value,
            times: Times,
            listener: d.Skill,
            wuxing: WuXing ?? d.Skill.Entry.WuXing,
            crit: false,
            lifeSteal: false,
            penetrate: false,
            doesntConsumeJianYi: false,
            shatter: false,
            evade: false,
            recursive: Recursive,
            castResult: d.CastResult,
            closures: ClosuresArray,
            induced: Induced);

    public override async UniTask Cast(CastDetails castDetails)
    {
        if (Closures != null)
            foreach (StageClosure closure in Closures)
            {
                if (closure.Description == null)
                    return;
                castDetails.CastResult.Append(closure.Key, false);
            }
        await castDetails.Env.AttackProcedure(GetDetailsFromCastDetails(castDetails));
    }

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        AttackProcedureDefinition pd = procedureDefinition as AttackProcedureDefinition;
        
        description.Sb.Append(pd.PostCondDefinition.Description);

        if (pd.Value > 0)
        {
            description.Sb.Append($"{pd.Value}攻");
            if (pd.Times > 1)
            {
                description.Sb.Append($"x{pd.Times}");
            }
        }
        
        if (pd.Closures != null)
            foreach (StageClosure c in pd.Closures)
            {
                description.AppendSoftReturn();
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                closureDescription.ApplyResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        
        description.ApplyStyle(castResult, pd);
    }
}