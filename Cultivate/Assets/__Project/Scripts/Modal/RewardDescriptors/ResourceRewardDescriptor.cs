using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ResourceRewardDescriptor : RewardDescriptor
{
    public int _xiuWei;
    public int _chanNeng;

    public ResourceRewardDescriptor(int xiuWei = 0, int chanNeng = 0)
    {
        _xiuWei = xiuWei;
        _chanNeng = chanNeng;
    }

    public override void Claim()
    {
        RunManager.Instance.AddXiuWei(_xiuWei);
        RunManager.Instance.AddChanNeng(_chanNeng);
    }

    public override string GetDescription()
    {
        StringBuilder sb = new();
        if (_xiuWei != 0)
        {
            sb.Append($"{_xiuWei}修为  ");
        }

        if (_chanNeng != 0)
        {
            sb.Append($"{_chanNeng}产能");
        }

        return sb.ToString();
    }
}
