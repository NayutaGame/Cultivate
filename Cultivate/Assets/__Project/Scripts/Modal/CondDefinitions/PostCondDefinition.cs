
using System;
using Cysharp.Threading.Tasks;

public class PostCondDefinition
{
    public Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<bool>> Cond;
    public string Description;

    protected PostCondDefinition(
        Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<bool>> cond,
        string description)
    {
        Cond = cond;
        Description = description;
    }

    public PostCondDefinition Clone()
        => new(Cond, Description);

    public async UniTask<bool> GetCond(CastDetails d)
    {
        return await Cond(d.Env, d.Caster, d.Skill, d.IsStartStage);
    }

    public async UniTask<bool> GetCond(CostDetails d)
    {
        return await Cond(d.Env, d.Entity, d.Skill, false);
    }

    public static readonly PostCondDefinition Default = new(async (env, entity, skill, startStage) => !startStage, "");
    public static readonly PostCondDefinition StartStage = new(async (env, entity, skill, startStage) => startStage, "开局：");
    public static readonly PostCondDefinition FirstTime = new(async (env, entity, skill, startStage) => skill.IsFirstTime, "初次：");
    public static readonly PostCondDefinition NotFirstTime = new(async (env, entity, skill, startStage) => skill.IsNotFirstTime, "非初次：");
    public static readonly PostCondDefinition FullHealth = new(async (env, entity, skill, startStage) => entity.IsFullHealth, "满血：");
    public static readonly PostCondDefinition LowHealth = new(async (env, entity, skill, startStage) => entity.IsLowHealth, "残血：");
    public static readonly PostCondDefinition NoOtherAttack = new(async (env, entity, skill, startStage) => skill.NoOtherAttack, "唯一攻击牌：");
    public static readonly PostCondDefinition HasOtherAttack = new(async (env, entity, skill, startStage) => !skill.NoOtherAttack, "有其他攻击牌：");
    public static readonly PostCondDefinition HasArmor = new(async (env, entity, skill, startStage) => entity.Armor > 0, "有护甲：");
    public static PostCondDefinition ManaBurst(int value)
        => new(async (env, entity, skill, startStage) => await entity.TryConsumeProcedure(Encyclopedia.BuffCategory.FromName("灵气"), value), $"爆能{value}：");
    public static readonly PostCondDefinition IsEnd = new(async (env, entity, skill, startStage) =>
    {
        if (skill.IsEnd)
            return true;

        if (await entity.TryConsumeProcedure(Encyclopedia.BuffCategory.FromName("终结")))
            return true;

        return false;
    }, "终结：");
    
    public static PostCondDefinition FromCc(int value, bool greaterEqual)
        => greaterEqual ?
            new(async (env, entity, skill, startStage) => skill.TotalStageCastedCount >= value, $"成长{value}次后：") :
            new(async (env, entity, skill, startStage) => skill.TotalStageCastedCount < value, $"成长{value}次前：");
}