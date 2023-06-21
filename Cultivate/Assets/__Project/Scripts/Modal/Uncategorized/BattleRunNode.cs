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

    public BattleRunNode(Vector2Int position, JingJie jingJie, BattleNodeEntry entry, CreateEntityDetails createEntityDetails) : base(position, jingJie, entry)
    {
        CreateEntityDetails = createEntityDetails;
        _rewards = new();
        RunManager.Instance.EntityPool.ForceDrawEntityEntry(out EntityEntry entityEntry, CreateEntityDetails);
        EntityEntry = entityEntry;

        _spriteEntry = CreateEntityDetails.AllowBoss ? "Boss" : "战斗";
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

    public int Ladder()
    {
        int x = Position.x;
        return JingJie * 3 + (4 <= x && x <= 6 ? 1 : 0) + (x >= 8 ? 2 : 0);
    }

    private int[] XiuWeiRewardTable = new int[]
    {
        5, 11, 31, 11, 21, 61, 15, 31, 91, 21, 41, 121, 25, 51, 151,
    };

    public int BaseXiuWeiReward()
    {
        return XiuWeiRewardTable[Ladder()];
    }
}
