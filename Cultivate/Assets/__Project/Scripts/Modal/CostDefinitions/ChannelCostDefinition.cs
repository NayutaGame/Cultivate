
using System;
using CLLibrary;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ChannelCostDefinition : CostDefinition
{
    public ChannelCostDefinition(int value, StageClosure[] closures = null) : base(value, closures) { }

    public ChannelCostDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Func<CostDefinition, ResultDict, Description> getDescription,
        int value, StageClosure[] closures) :
        base(preCondDefinition, postCondDefinition, getDescription, value, closures) { }

    public override CostDefinition Clone()
    {
        StageClosure[] clonedClosures = new StageClosure[Closures.Length];
        for (int i = 0; i < Closures.Length; i++)
            clonedClosures[i] = Closures[i];

        return new ChannelCostDefinition(
            PreCondDefinition.Clone(),
            PostCondDefinition.Clone(),
            _getDescription,
            Value,
            clonedClosures
        );
    }

    public static Func<int, int, ChannelCostDefinition> FromValue(int value)
        => (j, dj) => new(value);
    
    public static Func<int, int, ChannelCostDefinition> FromJ(Func<int, int> jFunc)
        => (j, dj) => new(jFunc(j));
    
    public static Func<int, int, ChannelCostDefinition> FromDj(Func<int, int> djFunc)
        => (j, dj) => new(djFunc(dj));
    
    public override async UniTask WillCostEvent(CostDetails d)
    {
        if (Closures != null)
        {
            bool postCond = await PostCondDefinition.GetCond(d);
            if (postCond)
                foreach (StageClosure closure in Closures)
                    if (closure.EventId == StageClosureDict.WIL_CHANNEL_COST)
                        await closure.Invoke(d.Skill, d);
            
            d.CostResult.Append(this, postCond);
        }
        
        await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_CHANNEL_COST, d);
        d.Counter = d.Value;
    }
    
    public override async UniTask ApplyCost(CostDetails d)
    {
        int oldProgress = d.Counter;
        ChannelDetails channelDetails = new ChannelDetails(d.Entity, d.Skill, oldProgress, d.Value, 1);
        await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_CHANNEL, channelDetails);

        int newProgress = oldProgress - channelDetails.ProgressGain;

        d.Blocking = newProgress >= 0;
        if (d.Blocking)
        {
            await d.Env.PlayAsync(new ShiftAnimation());
            d.Env.Result.TryAppendChannelNote(d.Entity.Index, d.Skill, oldProgress, d.Value);
            d.Env.Result.TryAppend($"{d.Entity.GetName()}正在吟唱{d.Skill.Entry.GetName()} 进度: {oldProgress}//{d.Value} 将推进：{channelDetails.ProgressGain}\n");
            d.Counter = newProgress.ClampLower(0);
            await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_CHANNEL, channelDetails);
        }
    }

    public override async UniTask DidCostEvent(CostDetails d)
    {
        await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_CHANNEL_COST, d);
    }

    public override CostDescription GetLiteralCostDescription()
        => new(CostType.Channel, CostState.Normal, Value);
}