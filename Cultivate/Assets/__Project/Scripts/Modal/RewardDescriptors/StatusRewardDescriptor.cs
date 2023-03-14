using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StatusRewardDescriptor : RewardDescriptor
{
    public int _mingYuan;

    public StatusRewardDescriptor(int mingYuan = 0)
    {
        _mingYuan = mingYuan;
    }

    public override void Claim()
    {
        RunManager.Instance.AddMingYuan(_mingYuan);
    }

    public override string GetDescription()
    {
        StringBuilder sb = new();
        if (_mingYuan != 0)
        {
            sb.Append($"{_mingYuan}命元");
        }

        return sb.ToString();
    }
}
