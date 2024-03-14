
using System.Threading.Tasks;

public class ChannelCostResult : CostResult
{
    public int Counter;
    
    public ChannelCostResult(int value) : base(value)
    {
    }

    public override CostDescription.CostType ToType()
        => CostDescription.CostType.Channel;

    public override async Task WillCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.WIL_CHANNEL_COST, this);
        Counter = Value;
    }
    
    public override async Task ApplyCost()
    {
        IncrementProgress();
        ChannelDetails d = new ChannelDetails(Entity, Skill, Counter, Value);
        await Env.EventDict.SendEvent(StageEventDict.WIL_CHANNEL, d);
        
        if (!IsFinished)
        {
            await Env.TryPlayTween(new ShiftTweenDescriptor());
            Env.Result.TryAppendChannelNote(Entity.Index, Skill, Counter, Value);
        }
        
        Env.Result.TryAppend($"{Entity.GetName()}吟唱了{Skill.GetName()} 进度: {Counter}//{Value}\n");
            
        await Env.EventDict.SendEvent(StageEventDict.DID_CHANNEL, d);

        Blocking = !IsFinished;
    }

    public override async Task DidCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.DID_CHANNEL_COST, this);
    }

    private void IncrementProgress()
    {
        Counter -= 1;
    }

    public bool IsFinished => Counter <= 0;
}
