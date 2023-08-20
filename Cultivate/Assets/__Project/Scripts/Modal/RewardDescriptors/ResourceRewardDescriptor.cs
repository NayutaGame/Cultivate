using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ResourceRewardDescriptor : RewardDescriptor
{
    public int _xiuWei;
    public int _mingYuan;
    public int _health;

    public ResourceRewardDescriptor(int xiuWei = 0, int mingYuan = 0, int health = 0)
    {
        _xiuWei = xiuWei;
        _mingYuan = mingYuan;
        _health = health;
    }

    public override void Claim()
    {
        RunManager.Instance.AddXiuWei(_xiuWei);
        RunManager.Instance.SetDMingYuan(_mingYuan);
        RunManager.Instance.AddHealth(_health);
    }

    public override string GetDescription()
    {
        StringBuilder sb = new();
        if (_xiuWei != 0)
        {
            sb.Append($"{_xiuWei}修为\t");
        }

        if (_mingYuan != 0)
        {
            sb.Append($"{_mingYuan}命元\t");
        }

        if (_health != 0)
        {
            sb.Append($"{_health}生命上限");
        }

        return sb.ToString();
    }
}
