
using System;

public struct CostDescription
{
    public enum CostType
    {
        Empty,
        Mana,
        Health,
        Channel,
        Armor,
    }

    public CostType Type;
    public CostResult.CostState State;
    public int Value;

    public CostDescription(CostType type, CostResult.CostState state, int value)
    {
        Type = type;
        State = state;
        Value = value;
    }

    public int ByType(CostType type)
    {
        if (Type == type)
            return Value;
        return 0;
    }

    public static Func<JingJie, int, CostResult, CostDescription> Empty
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Empty, CostResult.CostState.Normal, 0);

    public static Func<JingJie, int, CostResult, CostDescription> ArmorFromValue(int value)
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Armor, CostResult.CostState.Normal, value);
    
    public static Func<JingJie, int, CostResult, CostDescription> ArmorFromDj(Func<int, int> djFunc)
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Armor, CostResult.CostState.Normal, djFunc(dj));

    public static Func<JingJie, int, CostResult, CostDescription> ArmorFromJiaShi(Func<bool, int> jiaShi)
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Armor, CostResult.CostState.Normal, jiaShi(false));

    public static Func<JingJie, int, CostResult, CostDescription> ChannelFromValue(int value)
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Channel, CostResult.CostState.Normal, value);
    
    public static Func<JingJie, int, CostResult, CostDescription> ChannelFromDj(Func<int, int> djFunc)
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Channel, CostResult.CostState.Normal, djFunc(dj));

    public static Func<JingJie, int, CostResult, CostDescription> ChannelFromJiaShi(Func<bool, int> jiaShi)
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Channel, CostResult.CostState.Normal, jiaShi(false));

    public static Func<JingJie, int, CostResult, CostDescription> HealthFromValue(int value)
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Health, CostResult.CostState.Normal, value);
    
    public static Func<JingJie, int, CostResult, CostDescription> HealthFromDj(Func<int, int> djFunc)
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Health, CostResult.CostState.Normal, djFunc(dj));

    public static Func<JingJie, int, CostResult, CostDescription> HealthFromJiaShi(Func<bool, int> jiaShi)
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Health, CostResult.CostState.Normal, jiaShi(false));

    public static Func<JingJie, int, CostResult, CostDescription> ManaFromValue(int value)
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Mana, CostResult.CostState.Normal, value);
    
    public static Func<JingJie, int, CostResult, CostDescription> ManaFromDj(Func<int, int> djFunc)
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Mana, CostResult.CostState.Normal, djFunc(dj));

    public static Func<JingJie, int, CostResult, CostDescription> ManaFromJiaShi(Func<bool, int> jiaShi)
        => (j, dj, costResult) =>
            costResult?.ToCostDescription() ??
            new CostDescription(CostType.Mana, CostResult.CostState.Normal, jiaShi(false));
}
