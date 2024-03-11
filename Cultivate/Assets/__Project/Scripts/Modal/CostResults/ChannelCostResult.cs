
using System.Threading.Tasks;

public class ChannelCostResult : CostResult
{
    private int _value;
    private ChannelCostDetails _costDetails;
    
    public ChannelCostResult(int value)
    {
        _value = value;
    }

    public override async Task WillCostEvent()
    {
        _costDetails = new ChannelCostDetails(Entity, Skill, _value);
        await Env.EventDict.SendEvent(StageEventDict.WILL_CHANNEL, _costDetails);
    }
    
    public override async Task ApplyCost()
    {
        _costDetails.IncrementProgress();
        bool finished = _costDetails.FinishedChannelling();
        if (finished)
        {
            await _costDetails.Skill.ChannelWithoutTween(Entity, _costDetails);
        }
        else
        {
            await _costDetails.Skill.Channel(Entity, _costDetails);
        }

        Blocking = !finished;
    }

    public override async Task DidCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.DID_CHANNEL, _costDetails);
        _costDetails = null;
    }
}
