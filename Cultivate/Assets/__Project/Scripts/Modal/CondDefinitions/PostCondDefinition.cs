
using System;

public class PostCondDefinition
{
    public Func<StageEnvironment, CastDetails, bool> Cond;
    public string Description;

    protected PostCondDefinition(
        Func<StageEnvironment, CastDetails, bool> cond,
        string description)
    {
        Cond = cond;
        Description = description;
    }

    public static readonly PostCondDefinition Default = new((env, castDetails) => !castDetails.IsStartStage, "");
    public static readonly PostCondDefinition StartStage = new((env, castDetails) => castDetails.IsStartStage, "开局：");
    public static readonly PostCondDefinition FirstTime = new((env, castDetails) => castDetails.Skill.IsFirstTime, "初次：");
    
    // isGe: comparison sign is greater equal, otherwise Lt -> less than
    public static PostCondDefinition FromCc(int value, bool isGe)
        => isGe ?
            new((env, castDetails) => castDetails.Skill.TotalStageCastedCount >= value, $"成长{value}次后：") :
            new((env, castDetails) => castDetails.Skill.TotalStageCastedCount < value, $"成长{value}次前：");
}