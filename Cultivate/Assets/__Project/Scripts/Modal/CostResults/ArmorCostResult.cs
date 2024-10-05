
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
        await Env.ClosureDict.SendEvent(StageClosureDict.WIL_ARMOR_COST, this);
    }
    
    public override async Task ApplyCost()
    {
        int shortage = Mathf.Max(Value - Mathf.Max(0, Entity.Armor), 0);
        if (shortage > 0)
            await Env.ArmorShortageProcedure(this);
        
        int total = Value + 2 * shortage;
        await Entity.LoseArmorProcedure(total, false);
        
        Env.Result.TryAppend($"{Entity.GetName()}消耗了{Value}护甲，不足的部分变成了三倍的减甲，以使用{Skill.Entry.GetName()}\n");
        await Env.LoseHealthProcedure(Entity, Value, induced: true);
    }

    public override async Task DidCostEvent()
    {
        await Env.ClosureDict.SendEvent(StageClosureDict.DID_ARMOR_COST, this);
    }
}
