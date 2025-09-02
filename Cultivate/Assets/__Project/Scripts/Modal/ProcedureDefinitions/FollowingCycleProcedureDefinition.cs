
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class FollowingCycleProcedureDefinition : ProcedureDefinition
{
    public bool Rotate;
    public int Gain;
    public int Recover;
    public bool Induced;
    
    public FollowingCycleProcedureDefinition(
        bool rotate = true,
        int gain = 0,
        int recover = 0,
        bool induced = false)
    {
        Rotate = rotate;
        Gain = gain;
        Recover = recover;
        Induced = induced;
    }

    protected FollowingCycleProcedureDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription,
        List<StageClosure> closures,
        bool rotate,
        int gain,
        int recover,
        bool induced) :
        base(preCondDefinition, postCondDefinition, getDescription, closures)
    {
        Rotate = rotate;
        Gain = gain;
        Recover = recover;
        Induced = induced;
    }

    public override ProcedureDefinition Clone()
    {
        List<StageClosure> clonedClosures = new List<StageClosure>();
        for (int i = 0; i < Closures.Count; i++)
            clonedClosures.Add(Closures[i]);

        return new FollowingCycleProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            rotate: Rotate,
            gain: Gain,
            recover: Recover,
            induced: Induced
        );
    }
    
    public CycleDetails GetDetailsFromCastDetails(CastDetails d, WuXing toRotateWuXing)
        => new(d.Env, d.Caster, Rotate, toRotateWuXing, Gain, Recover, d.Skill, ClosuresArray, d.CastResult, Induced);

    public override async UniTask Cast(CastDetails castDetails)
    {
        WuXing wuXing = castDetails.Caster.Memory.TryGetVariable<WuXing>(StageEntity.LastRotatedWuXingKey, null);
        if (wuXing == null || !wuXing.IsBasic())
        {
            castDetails.CastResult["WuXing"] = $"{WuXing.Wu.GetName()}";
            return;
        }

        WuXing nextWuXing = wuXing.Next;
        castDetails.CastResult["WuXing"] = $"{nextWuXing.GetName()}";
        await castDetails.Env.CycleProcedure(GetDetailsFromCastDetails(castDetails, nextWuXing));
    }

    public override void DefaultGetDescription(
        Description description,
        ProcedureDefinition procedureDefinition,
        ResultDict costResult,
        ResultDict castResult)
    {
        FollowingCycleProcedureDefinition pd = procedureDefinition as FollowingCycleProcedureDefinition;
        description.Join(pd.PostCondDefinition.Description);

        WuXing wuXing = WuXing.Wu;
        if (castResult.ContainsKey("WuXing"))
        {
            wuXing = Encyclopedia.WuXingCategory.FromName(castResult["WuXing"]) ?? WuXing.Wu;
        }

        if (wuXing == WuXing.Wu)
        {
            description.Join($"跟随流转");
        }
        else
        {
            description.Join($"跟随流转({wuXing.GetElementaryBuff().GetName()})");
        }

        if (Gain != 0)
        {
            description.AppendSoftReturn();
            description.Join($"额外获得{Gain}层");
        }
        
        if (pd.Closures != null)
            foreach (StageClosure c in pd.Closures)
            {
                description.AppendSoftReturn();
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                closureDescription.ApplyResult(castResult, c.Key);
                description.Join(closureDescription);
            }
        
        description.ApplyStyle(castResult, pd);
    }
}