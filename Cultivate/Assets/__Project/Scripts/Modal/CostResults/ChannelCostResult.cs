
using System;
using System.Threading.Tasks;

public class ChannelCostResult : CostResult
{
    private ChannelDetails _details;

    public ChannelCostResult(int value) : base(value)
    {
    }

    public override async Task WillCostEvent()
    {
        _details = new ChannelDetails(Entity, Skill, Value);
        await Env.EventDict.SendEvent(StageEventDict.WIL_CHANNEL_COST, _details);
    }
    
    public override async Task ApplyCost()
    {
        _details.IncrementProgress();
        bool finished = _details.FinishedChannelling();
        if (finished)
        {
            await _details.Skill.ChannelNoTween(Entity, _details);
        }
        else
        {
            await _details.Skill.Channel(Entity, _details);
        }

        Blocking = !finished;
    }

    public override async Task DidCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.DID_CHANNEL_COST, _details);
        _details = null;
    }

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> FromValue(int value)
        => async (env, entity, skill, recursive) => new ChannelCostResult(value);
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> FromDj(Func<int, int> dj)
        => async (env, entity, skill, recursive) => new ChannelCostResult(dj(skill.Dj));

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> FromJiaShi(Func<bool, int> jiaShi)
        => async (env, entity, skill, recursive) =>
        {
            bool j = await entity.ToggleJiaShiProcedure();
            return new ChannelCostResult(jiaShi(j));
        };
}
