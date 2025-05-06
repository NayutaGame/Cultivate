
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ChannelCostDefinition : CostDefinition
{
    public ChannelCostDefinition(int value, StageClosure[] closures = null) : base(value, closures) { }

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
            bool postCond = PostCondDefinition.GetCond(d);
            if (postCond)
                foreach (StageClosure closure in Closures)
                    if (closure.EventId == StageClosureDict.WIL_CHANNEL_COST)
                        await closure.Invoke(d.Skill, d);
            
            d.CostResult.Append(this, postCond);
        }
        
        await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_CHANNEL_COST, d);
        d.Counter = Value;
    }
    
    public override async UniTask ApplyCost(CostDetails d)
    {
        int counter = d.Counter;
        ChannelDetails channelDetails = new ChannelDetails(d.Entity, d.Skill, counter, Value, 1);
        await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_CHANNEL, channelDetails);

        d.Blocking = counter > 0;
        if (d.Blocking)
        {
            await d.Env.PlayAsync(new ShiftAnimation());
            d.Env.Result.TryAppendChannelNote(d.Entity.Index, d.Skill, counter, Value);
            d.Env.Result.TryAppend($"{d.Entity.GetName()}正在吟唱{d.Skill.Entry.GetName()} 进度: {counter}//{Value} 将推进：{channelDetails.ProgressGain}\n");
            counter -= channelDetails.ProgressGain;
            counter = Mathf.Max(0, counter);
            d.Counter = counter;
        }
        
        await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_CHANNEL, channelDetails);
    }

    public override async UniTask DidCostEvent(CostDetails d)
    {
        await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_CHANNEL_COST, d);
    }

    public override CostDescription GetLiteralCostDescription()
        => new(CostType.Channel, CostState.Normal, Value);
}