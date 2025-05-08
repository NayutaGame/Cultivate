
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ArmorCostDefinition : CostDefinition
{
    public ArmorCostDefinition(int value, StageClosure[] closures = null) : base(value, closures) { }

    public ArmorCostDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Func<CostDefinition, ResultDict, Description> getDescription,
        int value, StageClosure[] closures) :
        base(preCondDefinition, postCondDefinition, getDescription, value, closures) { }

    public override CostDefinition Clone()
    {
        StageClosure[] clonedClosures = new StageClosure[Closures.Length];
        for (int i = 0; i < Closures.Length; i++)
            clonedClosures[i] = Closures[i];

        return new ArmorCostDefinition(
            PreCondDefinition.Clone(),
            PostCondDefinition.Clone(),
            _getDescription,
            Value,
            clonedClosures
        );
    }

    public static Func<int, int, ArmorCostDefinition> FromValue(int value)
        => (j, dj) => new(value);
    
    public static Func<int, int, ArmorCostDefinition> FromJ(Func<int, int> jFunc)
        => (j, dj) => new(jFunc(j));
    
    public static Func<int, int, ArmorCostDefinition> FromDj(Func<int, int> djFunc)
        => (j, dj) => new(djFunc(dj));

    public override async UniTask WillCostEvent(CostDetails d)
    {
        if (Closures != null)
        {
            bool postCond = PostCondDefinition.GetCond(d);
            if (postCond)
                foreach (StageClosure closure in Closures)
                    if (closure.EventId == StageClosureDict.WIL_ARMOR_COST)
                        await closure.Invoke(d.Skill, d);
            
            d.CostResult.Append(this, postCond);
        }
        
        await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_ARMOR_COST, d);
    }
    
    public override async UniTask ApplyCost(CostDetails d)
    {
        int shortage = Mathf.Max(Value - Mathf.Max(0, d.Entity.Armor), 0);
        if (shortage > 0)
            await d.Env.ArmorShortageProcedure(d);
        
        int total = Value + 2 * shortage;
        await d.Entity.LoseArmorProcedure(total, induced: false);
        
        d.Env.Result.TryAppend($"{d.Entity.GetName()}消耗了{Value}护甲，不足的部分变成了三倍的减甲，以使用{d.Skill.Entry.GetName()}\n");
        // suspicious
        await d.Env.LoseHealthProcedure(d.Entity, Value, false, induced: true);
    }
    
    public override async UniTask DidCostEvent(CostDetails d)
    {
        await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_ARMOR_COST, d);
    }

    public override CostDescription GetLiteralCostDescription()
        => new(CostType.Armor, CostState.Normal, Value);
}