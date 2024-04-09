
using System.Threading.Tasks;

public class ChannelCostResult : CostResult
{
    private int _counter;
    
    public ChannelCostResult(int value) : base(value)
    {
    }

    public override CostDescription.CostType ToType()
        => CostDescription.CostType.Channel;

    public override async Task WillCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.WIL_CHANNEL_COST, this);
        _counter = Value;
    }
    
    public override async Task ApplyCost()
    {
        ChannelDetails d = new ChannelDetails(Entity, Skill, _counter, Value);
        await Env.EventDict.SendEvent(StageEventDict.WIL_CHANNEL, d);

        Blocking = _counter > 0;
        if (Blocking)
        {
            await Env.TryPlayTween(new ShiftTweenDescriptor());
            Env.Result.TryAppendChannelNote(Entity.Index, Skill, _counter, Value);
            Env.Result.TryAppend($"{Entity.GetName()}吟唱了{Skill.GetName()} 进度: {_counter}//{Value}\n");
            _counter -= 1;
        }
        
        await Env.EventDict.SendEvent(StageEventDict.DID_CHANNEL, d);
    }

    public override async Task DidCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.DID_CHANNEL_COST, this);
    }
}
