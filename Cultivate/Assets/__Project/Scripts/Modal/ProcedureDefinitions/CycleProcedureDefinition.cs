
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class CycleProcedureDefinition : ProcedureDefinition
{
    public WuXing WuXing;
    public bool Rotate;
    public int Gain;
    public int Recover;
    public bool Induced;
    
    public CycleProcedureDefinition(
        WuXing wuXing,
        bool rotate = true,
        int gain = 0,
        int recover = 0,
        bool induced = false)
    {
        WuXing = wuXing;
        Rotate = rotate;
        Gain = gain;
        Recover = recover;
        Induced = induced;
    }

    protected CycleProcedureDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription,
        List<StageClosure> closures,
        WuXing wuXing,
        bool rotate,
        int gain,
        int recover,
        bool induced) :
        base(preCondDefinition, postCondDefinition, getDescription, closures)
    {
        WuXing = wuXing;
        Rotate = rotate;
        Gain = gain;
        Recover = recover;
        Induced = induced;
    }

    public override ProcedureDefinition Clone()
    {
        List<StageClosure> clonedClosures = new List<StageClosure>();
        for (int i = 0; i < Closures.Count; i++)
            clonedClosures[i] = Closures[i];

        return new CycleProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            wuXing: WuXing,
            rotate: Rotate,
            gain: Gain,
            recover: Recover,
            induced: Induced
        );
    }
    
    public CycleDetails GetDetailsFromCastDetails(CastDetails d)
        => new(d.Caster, Rotate, WuXing, Gain, Recover, d.Skill, ClosuresArray, d.CastResult, Induced);

    public override async UniTask Cast(CastDetails castDetails)
    {
        await castDetails.Env.CycleProcedure(GetDetailsFromCastDetails(castDetails));
    }

    public override void DefaultGetDescription(
        Description description,
        ProcedureDefinition procedureDefinition,
        ResultDict costResult,
        ResultDict castResult)
    {
        CycleProcedureDefinition pd = procedureDefinition as CycleProcedureDefinition;
        description.Sb.Append(pd.PostCondDefinition.Description);
        description.Sb.Append($"{pd.WuXing._elementaryBuff}+{pd.Gain}");
        if (pd.Closures != null)
            foreach (StageClosure c in pd.Closures)
            {
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                // closureDescription.ApplyCastResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        
        description.ApplyStyle(castResult, pd);
    }
}