
using System.Collections.Generic;

public class DrawSkillReward : Reward
{
    private string _description;
    private List<SkillEntryQuery> _drawStrategies;
    private JingJie _jingJie;

    public DrawSkillReward(string description, List<SkillEntryQuery> drawStrategies, JingJie jingJie)
    {
        _description = description;
        _drawStrategies = drawStrategies;
        _jingJie = jingJie;
    }

    public override void Claim()
        => RunManager.Instance.Environment.DrawSkillsProcedure(_drawStrategies, _jingJie);

    public override string GetDescription() => _description;
}
