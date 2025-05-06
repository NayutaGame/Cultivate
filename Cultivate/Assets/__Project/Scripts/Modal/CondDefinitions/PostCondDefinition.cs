
using System;

public class PostCondDefinition
{
    public Func<StageEnvironment, StageEntity, StageSkill, bool, bool> Cond;
    public string Description;

    protected PostCondDefinition(
        Func<StageEnvironment, StageEntity, StageSkill, bool, bool> cond,
        string description)
    {
        Cond = cond;
        Description = description;
    }

    public bool GetCond(CastDetails d)
    {
        return Cond(d.Env, d.Caster, d.Skill, d.IsStartStage);
    }

    public bool GetCond(CostDetails d)
    {
        return Cond(d.Env, d.Entity, d.Skill, false);
    }

    public static readonly PostCondDefinition Default = new((env, entity, skill, startStage) => !startStage, "");
    public static readonly PostCondDefinition StartStage = new((env, entity, skill, startStage) => startStage, "开局：");
    public static readonly PostCondDefinition FirstTime = new((env, entity, skill, startStage) => skill.IsFirstTime, "初次：");
    public static readonly PostCondDefinition FullHealth = new((env, entity, skill, startStage) => entity.IsFullHealth, "满血：");
    public static readonly PostCondDefinition LowHealth = new((env, entity, skill, startStage) => entity.IsLowHealth, "残血：");
    
    public static PostCondDefinition FromCc(int value, bool greaterEqual)
        => greaterEqual ?
            new((env, entity, skill, startStage) => skill.TotalStageCastedCount >= value, $"成长{value}次后：") :
            new((env, entity, skill, startStage) => skill.TotalStageCastedCount < value, $"成长{value}次前：");
}