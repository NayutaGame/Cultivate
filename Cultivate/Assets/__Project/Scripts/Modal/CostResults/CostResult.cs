
using System;
using System.Threading.Tasks;

public abstract class CostResult : EventDetails
{
    public enum CostState
    {
        Unwritten,
        Normal,
        Reduced,
        Shortage,
    }
    
    public StageEnvironment Env;
    public StageEntity Entity;
    public StageSkill Skill;

    public int Value;
    public CostState State;

    public bool Blocking = false;

    protected CostResult(int value)
    {
        Value = value;
    }

    public virtual async Task WillCostEvent()
    {
    }

    public virtual async Task ApplyCost()
    {
    }

    public virtual async Task DidCostEvent()
    {
        
    }

    public abstract CostDescription.CostType ToType();

    public CostDescription ToCostDescription()
        => new(ToType(), State, Value);

    public static async Task<CostResult> FromEnvironment(StageEnvironment env, StageEntity entity, StageSkill skill, bool recursive = true)
        => await skill.Entry.Cost(env, entity, skill, recursive);

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> Empty
        => async (env, entity, skill, recursive) => new EmptyCostResult();

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> ArmorFromValue(int value)
        => async (env, entity, skill, recursive) => new ArmorCostResult(value);
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> ArmorFromDj(Func<int, int> dj)
        => async (env, entity, skill, recursive) => new ArmorCostResult(dj(skill.Dj));

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> ArmorFromJiaShi(Func<bool, int> jiaShi)
        => async (env, entity, skill, recursive) =>
        {
            bool j = await entity.ToggleJiaShiProcedure();
            return new ArmorCostResult(jiaShi(j));
        };

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> ChannelFromValue(int value)
        => async (env, entity, skill, recursive) => new ChannelCostResult(value);
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> ChannelFromDj(Func<int, int> dj)
        => async (env, entity, skill, recursive) => new ChannelCostResult(dj(skill.Dj));

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> ChannelFromJiaShi(Func<bool, int> jiaShi)
        => async (env, entity, skill, recursive) =>
        {
            bool j = await entity.ToggleJiaShiProcedure();
            return new ChannelCostResult(jiaShi(j));
        };

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> HealthFromValue(int value)
        => async (env, entity, skill, recursive) => new HealthCostResult(value);
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> HealthFromDj(Func<int, int> dj)
        => async (env, entity, skill, recursive) => new HealthCostResult(dj(skill.Dj));

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> HealthFromJiaShi(Func<bool, int> jiaShi)
        => async (env, entity, skill, recursive) =>
        {
            bool j = await entity.ToggleJiaShiProcedure();
            return new HealthCostResult(jiaShi(j));
        };

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> ManaFromValue(int value)
        => async (env, entity, skill, recursive) => new ManaCostResult(value);
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> ManaFromDj(Func<int, int> dj)
        => async (env, entity, skill, recursive) => new ManaCostResult(dj(skill.Dj));

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> ManaFromJiaShi(Func<bool, int> jiaShi)
        => async (env, entity, skill, recursive) =>
        {
            bool j = await entity.ToggleJiaShiProcedure();
            return new ManaCostResult(jiaShi(j));
        };
}
