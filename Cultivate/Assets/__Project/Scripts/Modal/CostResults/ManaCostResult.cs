
using System;
using System.Threading.Tasks;

public class ManaCostResult : CostResult
{
    public override async Task WillCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.WIL_MANA_COST, this);
    }

    public override async Task ApplyCost()
    {
        bool manaSufficient = Entity.GetStackOfBuff("灵气") >= Value;
        if (manaSufficient)
        {
            await Entity.TryConsumeProcedure("灵气", Value);
        }
        else
        {
            await Env.ManaShortageProcedure(this);
            await Entity.CastProcedure(Entity.ManaShortageAction);
        }
        
        Blocking = !manaSufficient;
    }

    public override async Task DidCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.DID_MANA_COST, this);
    }

    protected ManaCostResult(int value) : base(value)
    {
    }

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> FromValue(int value)
        => async (env, entity, skill, recursive) => new ManaCostResult(value);
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> FromDj(Func<int, int> dj)
        => async (env, entity, skill, recursive) => new ManaCostResult(dj(skill.Dj));

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> FromJiaShi(Func<bool, int> jiaShi)
        => async (env, entity, skill, recursive) =>
        {
            bool j = await entity.ToggleJiaShiProcedure();
            return new ManaCostResult(jiaShi(j));
        };
}
