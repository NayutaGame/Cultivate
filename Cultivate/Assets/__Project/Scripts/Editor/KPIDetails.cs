
using System;
using System.Collections.Generic;

public class KPIDetails
{
    public string Name { get; }
    public Func<SkillEntry, bool> Predicate { get; }
    public Dictionary<WuXing, (int Min, int Max)> TargetTable { get; }
    public string Description { get; }

    public KPIDetails(
        string name, 
        Func<SkillEntry, bool> predicate, 
        Dictionary<WuXing, (int Min, int Max)> targetTable,
        string description = null)
    {
        Name = name;
        Predicate = predicate;
        TargetTable = targetTable;
        Description = description;
    }

    public static readonly KPIDetails ManaCost = new(
        name: "法力消耗",
        predicate: s => s.GetCostDescription(s.LowestJingJie).Type == CostDescription.CostType.Mana,
        targetTable: new Dictionary<WuXing, (int Min, int Max)>
        {
            { WuXing.Jin, (0, 1) },    // 正常消耗
            { WuXing.Shui, (1, 3) },   // 大量消耗
            { WuXing.Mu, (1, 3) },     // 大量消耗
            { WuXing.Huo, (0, 1) },    // 不消耗
            { WuXing.Tu, (0, 0) }      // 少量消耗
        },
        description: "金系正常消耗，水木系大量消耗，火系不消耗，土系少量消耗"
    );

    public static readonly KPIDetails HealthCost = new(
        name: "生命消耗",
        predicate: s => s.GetCostDescription(s.LowestJingJie).Type == CostDescription.CostType.Health,
        targetTable: new Dictionary<WuXing, (int Min, int Max)>
        {
            { WuXing.Jin, (0, 0) },    // 不消耗
            { WuXing.Shui, (0, 0) },   // 不消耗
            { WuXing.Mu, (0, 0) },     // 不消耗
            { WuXing.Huo, (1, 3) },    // 大量消耗
            { WuXing.Tu, (0, 1) }      // 少量消耗
        },
        description: "火系大量消耗，土系少量消耗，其他系不消耗"
    );

    public static readonly KPIDetails ChannelCost = new(
        name: "引导消耗",
        predicate: s => s.GetCostDescription(s.LowestJingJie).Type == CostDescription.CostType.Channel,
        targetTable: new Dictionary<WuXing, (int Min, int Max)>
        {
            { WuXing.Jin, (0, 1) },    // 少量消耗
            { WuXing.Shui, (0, 0) },   // 不消耗
            { WuXing.Mu, (0, 1) },     // 不消耗
            { WuXing.Huo, (0, 2) },    // 正常消耗
            { WuXing.Tu, (1, 3) }      // 大量消耗
        },
        description: "金系少量消耗，火系正常消耗，土系大量消耗"
    );

    public static readonly KPIDetails[] All = { ManaCost, HealthCost, ChannelCost };
}
