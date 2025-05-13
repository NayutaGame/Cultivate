
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

public class ManaCostDefinition : CostDefinition
{
    public ManaCostDefinition(int value, StageClosure[] closures = null) : base(value, closures) { }

    public ManaCostDefinition(
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

        return new ManaCostDefinition(
            PreCondDefinition.Clone(),
            PostCondDefinition.Clone(),
            _getDescription,
            Value,
            clonedClosures
        );
    }

    public static Func<int, int, ManaCostDefinition> FromValue(int value)
        => (j, dj) => new(value);
    
    public static Func<int, int, ManaCostDefinition> FromJ(Func<int, int> jFunc)
        => (j, dj) => new(jFunc(j));
    
    public static Func<int, int, ManaCostDefinition> FromDj(Func<int, int> djFunc)
        => (j, dj) => new(djFunc(dj));
    
    public override async UniTask WillCostEvent(CostDetails d)
    {
        if (Closures != null)
        {
            bool postCond = await PostCondDefinition.GetCond(d);
            if (postCond)
                foreach (StageClosure closure in Closures)
                    if (closure.EventId == StageClosureDict.WIL_MANA_COST)
                        await closure.Invoke(d.Skill, d);
            
            d.CostResult.Append(this, postCond);
        }
        
        await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_MANA_COST, d);
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

    public void PayWithMana(CostDetails costDetails, ref ManaConsumptionDetails d)
    {
        int manaRequirement = Value;
        int availableMana = costDetails.Entity.GetStackOfBuff("灵气");
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

    public void PayWithDuanTi(CostDetails costDetails, ref ManaConsumptionDetails d)
    {
        if (d.manaSufficient)
            return;

        int manaRequirement = Value;
        int suoHunStack = costDetails.Entity.GetStackOfBuff("塑魂");
        if (suoHunStack <= 0)
            return;

        int availableDuanTi = costDetails.Entity.GetStackOfBuff("锻体");
        int availableManaFromDuanTi = availableDuanTi / suoHunStack;
        int remainingRequirement = manaRequirement - d.manaConsumption;

        if (availableManaFromDuanTi >= remainingRequirement)
        {
            d.duanTiConsumption = remainingRequirement * suoHunStack;
            d.manaSufficient = true;
        }
    }

    public override async UniTask ApplyCost(CostDetails costDetails)
    {
        Assert.IsTrue(Value >= 0);
        ManaConsumptionDetails d = new ManaConsumptionDetails();
        d.Reset();

        PayWithMana(costDetails, ref d);
        PayWithDuanTi(costDetails, ref d);

        if (d.manaSufficient)
        {
            if (d.manaConsumption > 0)
                await costDetails.Entity.TryConsumeProcedure("灵气", d.manaConsumption);
            if (d.duanTiConsumption > 0)
                await costDetails.Entity.TryConsumeProcedure("锻体", d.duanTiConsumption);
        }
        else
        {
            await costDetails.Env.ManaShortageProcedure(costDetails);
            await costDetails.Entity.CastProcedure(costDetails.Entity.ManaShortageAction);
        }
        
        costDetails.Blocking = !d.manaSufficient;
    }

    public override async UniTask DidCostEvent(CostDetails d)
    {
        await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_MANA_COST, d);
    }

    public override CostDescription GetLiteralCostDescription()
        => new(CostType.Mana, CostState.Normal, Value);
}