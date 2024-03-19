
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using UnityEngine;

public class BattleRunNode : RunNode
{
    private RunEntity _entity;
    public RunEntity Entity => _entity;
    private bool _isBoss;
    public bool IsBoss => _isBoss;
    private List<Reward> _rewards;

    public BattleRunNode(int level, int step, int choice, RunEntity entity) : base(level, step, choice, "战斗")
    {
        _entity = entity;
        _isBoss = entity.IsBoss();
        _rewards = new();

        _spriteEntry = _isBoss ? "Boss" : "战斗";
    }

    public override string GetTitle()
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

    public int Ladder()
    {
        int level = GetLevel();
        int step = GetStep();
        return level * 3 + (4 <= step && step <= 6 ? 1 : 0) + (step >= 8 ? 2 : 0);
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
