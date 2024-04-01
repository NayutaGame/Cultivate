
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

    public BattleRunNode(RunEntity entity) : base("战斗")
    {
        _entity = entity;
        _isBoss = entity.IsBoss();
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

    public int LadderFromLevelAndStep(JingJie jingJie, int step)
    {
        return jingJie * 3 + step;
    }

    private int[] GoldRewardTable = new int[]
    {
        5, 11, 31,
        11, 21, 61,
        15, 31, 91,
        21, 41, 121,
        25, 51, 151,
    };

    public int BaseGoldReward()
    {
        JingJie jingJie = _entity.GetJingJie();
        int step = 0;
        if (_entity.IsBoss())
            step = 2;
        else if (_entity.IsElite())
            step = 1;
        return GoldRewardTable[LadderFromLevelAndStep(jingJie, step)];
    }
}
