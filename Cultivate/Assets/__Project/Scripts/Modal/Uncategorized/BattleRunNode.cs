using System.Collections;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using UnityEngine;

public class BattleRunNode : RunNode
{
    public CreateEntityDetails CreateEntityDetails;
    private List<RewardDescriptor> _rewards;
    public EntityEntry EntityEntry;

    public BattleRunNode(Vector2Int position, BattleNodeEntry entry, CreateEntityDetails createEntityDetails) : base(position, entry)
    {
        CreateEntityDetails = createEntityDetails;
        _rewards = new();
        RunManager.Instance.EntityPool.TryDrawEntityEntry(out EntityEntry entityEntry, CreateEntityDetails);
        EntityEntry = entityEntry;
    }

    public override string GetTitle()
    {
        return EntityEntry.Name;
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
