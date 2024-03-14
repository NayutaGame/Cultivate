
using System.Threading.Tasks;
using UnityEngine;

public class ArmorCostResult : CostResult
{
    public ArmorCostResult(int value) : base(value)
    {
    }

    public override CostDescription.CostType ToType()
        => CostDescription.CostType.Armor;

    public override async Task WillCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.WIL_ARMOR_COST, this);
    }
    
    public override async Task ApplyCost()
    {
        int shortage = Mathf.Max(Value - Mathf.Max(0, Entity.Armor), 0);
        if (shortage > 0)
            await Env.ArmorShortageProcedure(this);
        
        int total = Value + 2 * shortage;
        await Entity.LoseArmorProcedure(total);
        
        Env.Result.TryAppend($"{Entity.GetName()}消耗了{Value}护甲，不足的部分变成了三倍的减甲，以使用{Skill.GetName()}\n");
        await Env.LoseHealthProcedure(Entity, Value);
    }

    public override async Task DidCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.DID_ARMOR_COST, this);
    }
}
