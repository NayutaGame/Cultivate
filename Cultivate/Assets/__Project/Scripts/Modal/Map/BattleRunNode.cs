
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using UnityEngine;

public class BattleRunNode : RunNode
{
    private RunEntity _entity;
    public RunEntity Entity => _entity;
    private List<Reward> _rewards;

    public BattleRunNode(RunEntity entity) : base("战斗")
    {
        _entity = entity;
        _rewards = new();
    }

    public override string GetName()
        => _entity.GetEntry().GetName();

    public void AddReward(Reward reward)
        => _rewards.Add(reward);

    public string GetRewardsString()
    {
        StringBuilder sb = new();

        _rewards.Do(r =>
        {
            sb.Append($"{r.GetDescription()}; ");
        });

        return sb.ToString();
    }

    public void ClaimRewards()
    {
        _rewards.Do(reward => reward.Claim());
    }
}
