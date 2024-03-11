
using System.Threading.Tasks;

public class ManaCostResult : CostResult
{
    private int _value;
    private ManaCostDetails _costDetails;

    public ManaCostResult(int value)
    {
        _value = value;
    }

    public override async Task WillCostEvent()
    {
        _costDetails = new ManaCostDetails(Entity, Skill, _value);
        await Env.EventDict.SendEvent(StageEventDict.WILL_MANA_COST, _costDetails);
    }

    public override async Task ApplyCost()
    {
        bool manaSufficient = Entity.GetStackOfBuff("灵气") >= _costDetails.Cost;
        if (manaSufficient)
        {
            await Entity.TryConsumeProcedure("灵气", _costDetails.Cost);
        }
        else
        {
            await Entity.ManaShortageProcedure(Entity._p, Skill, _costDetails.Cost);
            await Entity.ManaShortageAction.Execute(Entity);
        }
        
        Blocking = !manaSufficient;
    }

    public override async Task DidCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.DID_MANA_COST, _costDetails);
        _costDetails = null;
    }
}
