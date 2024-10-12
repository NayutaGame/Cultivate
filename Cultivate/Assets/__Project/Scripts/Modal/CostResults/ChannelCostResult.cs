
using Cysharp.Threading.Tasks;

public class ChannelCostResult : CostResult
{
    private int _counter;
    
    public ChannelCostResult(int value) : base(value)
    {
    }

    public override CostDescription.CostType ToType()
        => CostDescription.CostType.Channel;

    public override async UniTask WillCostEvent()
    {
        await Env.ClosureDict.SendEvent(StageClosureDict.WIL_CHANNEL_COST, this);
        _counter = Value;
    }
    
    public override async UniTask ApplyCost()
    {
        ChannelDetails d = new ChannelDetails(Entity, Skill, _counter, Value);
        await Env.ClosureDict.SendEvent(StageClosureDict.WIL_CHANNEL, d);

        Blocking = _counter > 0;
        if (Blocking)
        {
            await Env.PlayAsync(new ShiftAnimation());
            Env.Result.TryAppendChannelNote(Entity.Index, Skill, _counter, Value);
            Env.Result.TryAppend($"{Entity.GetName()}吟唱了{Skill.Entry.GetName()} 进度: {_counter}//{Value}\n");
            _counter -= 1;
        }
        
        await Env.ClosureDict.SendEvent(StageClosureDict.DID_CHANNEL, d);
    }

    public override async UniTask DidCostEvent()
    {
        await Env.ClosureDict.SendEvent(StageClosureDict.DID_CHANNEL_COST, this);
    }
}
