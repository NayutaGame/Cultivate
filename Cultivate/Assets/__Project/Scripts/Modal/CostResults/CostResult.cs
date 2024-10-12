
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class CostResult : ClosureDetails
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
        Value = Mathf.Max(0, value);
    }

    public virtual async UniTask WillCostEvent()
    {
    }

    public virtual async UniTask ApplyCost()
    {
    }

    public virtual async UniTask DidCostEvent()
    {
        
    }

    public abstract CostDescription.CostType ToType();

    public CostDescription ToCostDescription()
        => new(ToType(), State, Value);

    public static async UniTask<CostResult> FromEnvironment(StageEnvironment env, StageEntity entity, StageSkill skill, bool recursive = true)
        => await skill.Entry.Cost(env, entity, skill, recursive);
    

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> Empty
        => async (env, entity, skill, recursive) => new EmptyCostResult();
    

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> ArmorFromValue(int value)
        => async (env, entity, skill, recursive) => new ArmorCostResult(value);
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> ArmorFromDj(Func<int, int> dj)
        => async (env, entity, skill, recursive) => new ArmorCostResult(dj(skill.Dj));

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> ArmorFromJiaShi(Func<bool, int> jiaShi)
        => async (env, entity, skill, recursive) => new ArmorCostResult(jiaShi(await entity.JiaShiProcedure()));
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> ArmorFromCastedCount(Func<int, int> cc)
        => async (env, entity, skill, recursive) => new ArmorCostResult(cc(skill.StageCastedCount));
    

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> ChannelFromValue(int value)
        => async (env, entity, skill, recursive) => new ChannelCostResult(value);
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> ChannelFromDj(Func<int, int> dj)
        => async (env, entity, skill, recursive) => new ChannelCostResult(dj(skill.Dj));

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> ChannelFromJiaShi(Func<bool, int> jiaShi)
        => async (env, entity, skill, recursive) => new ChannelCostResult(jiaShi(await entity.JiaShiProcedure()));
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> ChannelFromCastedCount(Func<int, int> cc)
        => async (env, entity, skill, recursive) => new ChannelCostResult(cc(skill.StageCastedCount));
    

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> HealthFromValue(int value)
        => async (env, entity, skill, recursive) => new HealthCostResult(value);
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> HealthFromDj(Func<int, int> dj)
        => async (env, entity, skill, recursive) => new HealthCostResult(dj(skill.Dj));

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> HealthFromJiaShi(Func<bool, int> jiaShi)
        => async (env, entity, skill, recursive) => new HealthCostResult(jiaShi(await entity.JiaShiProcedure()));
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> HealthFromCastedCount(Func<int, int> cc)
        => async (env, entity, skill, recursive) => new HealthCostResult(cc(skill.StageCastedCount));
    

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> ManaFromValue(int value)
        => async (env, entity, skill, recursive) => new ManaCostResult(value);
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> ManaFromDj(Func<int, int> dj)
        => async (env, entity, skill, recursive) => new ManaCostResult(dj(skill.Dj));

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> ManaFromJiaShi(Func<bool, int> jiaShi)
        => async (env, entity, skill, recursive) => new ManaCostResult(jiaShi(await entity.JiaShiProcedure()));
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> ManaFromCastedCount(Func<int, int> cc)
        => async (env, entity, skill, recursive) => new ManaCostResult(cc(skill.StageCastedCount));
}
