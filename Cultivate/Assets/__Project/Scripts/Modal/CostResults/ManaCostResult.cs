
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

public class ManaCostResult : CostResult
{
    public ManaCostResult(int value) : base(value)
    {
    }
    
    public override CostDescription.CostType ToType()
        => CostDescription.CostType.Mana;
    
    public override async UniTask WillCostEvent()
    {
        await Env.ClosureDict.SendEvent(StageClosureDict.WIL_MANA_COST, this);
    }

    public struct ManaConsumptionDetails
    {
        public int manaConsumption;
        public int duanTiConsumption;
        public bool manaSufficient;

        public void Reset()
        {
            manaConsumption = 0;
            duanTiConsumption = 0;
            manaSufficient = false;
        }
    }

    public void PayWithMana(ref ManaConsumptionDetails d)
    {
        int manaRequirement = Value;
        int availableMana = Entity.GetStackOfBuff("灵气");
        if (availableMana >= manaRequirement)
        {
            d.manaSufficient = true;
            d.manaConsumption = manaRequirement;
            d.duanTiConsumption = 0;
            return;
        }
        d.manaConsumption = availableMana;
        d.manaSufficient = false;
    }

    public void PayWithDuanTi(ref ManaConsumptionDetails d)
    {
        if (d.manaSufficient)
            return;

        int manaRequirement = Value;
        int suoHunStack = Entity.GetStackOfBuff("塑魂");
        if (suoHunStack <= 0)
            return;

        int availableDuanTi = Entity.GetStackOfBuff("锻体");
        int availableManaFromDuanTi = availableDuanTi / suoHunStack;
        int remainingRequirement = manaRequirement - d.manaConsumption;

        if (availableManaFromDuanTi >= remainingRequirement)
        {
            d.duanTiConsumption = remainingRequirement * suoHunStack;
            d.manaSufficient = true;
        }
    }

    public override async UniTask ApplyCost()
    {
        Assert.IsTrue(Value >= 0);
        ManaConsumptionDetails d = new ManaConsumptionDetails();
        d.Reset();

        PayWithMana(ref d);
        PayWithDuanTi(ref d);

        if (d.manaSufficient)
        {
            if (d.manaConsumption > 0)
                await Entity.TryConsumeProcedure("灵气", d.manaConsumption);
            if (d.duanTiConsumption > 0)
                await Entity.TryConsumeProcedure("锻体", d.duanTiConsumption);
        }
        else
        {
            await Env.ManaShortageProcedure(this);
            await Entity.CastProcedure(Entity.ManaShortageAction);
        }
        
        Blocking = !d.manaSufficient;
    }

    public override async UniTask DidCostEvent()
    {
        await Env.ClosureDict.SendEvent(StageClosureDict.DID_MANA_COST, this);
    }
}
