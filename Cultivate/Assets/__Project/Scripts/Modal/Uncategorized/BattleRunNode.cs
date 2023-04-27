using System.Collections;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using UnityEngine;

public class BattleRunNode : RunNode
{
    public CreateEnemyDetails CreateEnemyDetails;
    private List<RewardDescriptor> _rewards;
    public EnemyEntry _enemyEntry;

    public BattleRunNode(Vector2Int position, BattleNodeEntry entry, CreateEnemyDetails createEnemyDetails) : base(position, entry)
    {
        CreateEnemyDetails = createEnemyDetails;
        _rewards = new();
        _enemyEntry = RunManager.Instance.DrawEnemy(CreateEnemyDetails);
    }

    public override string GetTitle()
    {
        return _enemyEntry.Name;
    }

    public void AddReward(RewardDescriptor rewardDescriptor)
    {
        _rewards.Add(rewardDescriptor);
    }

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
