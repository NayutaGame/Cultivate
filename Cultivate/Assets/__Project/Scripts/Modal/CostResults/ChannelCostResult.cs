
using Cysharp.Threading.Tasks;
using UnityEngine;

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
        ChannelDetails d = new ChannelDetails(Entity, Skill, _counter, Value, 1);
        await Env.ClosureDict.SendEvent(StageClosureDict.WIL_CHANNEL, d);

        Blocking = _counter > 0;
        if (Blocking)
        {
            await Env.PlayAsync(new ShiftAnimation());
            Env.Result.TryAppendChannelNote(Entity.Index, Skill, _counter, Value);
            Env.Result.TryAppend($"{Entity.GetName()}正在吟唱{Skill.Entry.GetName()} 进度: {_counter}//{Value} 将推进：{d.ProgressGain}\n");
            _counter -= d.ProgressGain;
            _counter = Mathf.Max(0, _counter);
        }
        
        await Env.ClosureDict.SendEvent(StageClosureDict.DID_CHANNEL, d);
    }

    public override async UniTask DidCostEvent()
    {
        await Env.ClosureDict.SendEvent(StageClosureDict.DID_CHANNEL_COST, this);
    }
}
