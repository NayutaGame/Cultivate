using System;

public class PreCondDefinition
{
    public Func<int, int, bool> Cond;
    public string Description;

    private PreCondDefinition(
        Func<int, int, bool> cond,
        string description)
    {
        Cond = cond;
        Description = description;
    }

    public PreCondDefinition Clone()
        => new(Cond, Description);

    public static readonly PreCondDefinition Default = new((j, dj) => true, "");
    public static readonly PreCondDefinition GeZhuJi = new((j, dj) => j >= JingJie.ZhuJi, "");
    public static readonly PreCondDefinition GeJinDan = new((j, dj) => j >= JingJie.JinDan, "");
    public static readonly PreCondDefinition GeYuanYing = new((j, dj) => j >= JingJie.YuanYing, "");
    public static readonly PreCondDefinition GeHuaShen = new((j, dj) => j >= JingJie.HuaShen, "");
    public static readonly PreCondDefinition LeYuanYing = new((j, dj) => j <= JingJie.YuanYing, "");
    public static readonly PreCondDefinition LeLianQi = new((j, dj) => j <= JingJie.LianQi, "");
}