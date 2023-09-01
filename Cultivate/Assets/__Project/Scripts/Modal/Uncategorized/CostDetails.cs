using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CostDetails
{
    public int _xiuWei;
    public int _chanNeng;
    public int _mingYuan;
    public int _health;

    public CostDetails(int xiuWei = 0, int chanNeng = 0, int mingYuan = 0, int health = 0)
    {
        _xiuWei = xiuWei;
        _chanNeng = chanNeng;
        _mingYuan = mingYuan;
        _health = health;
    }

    public bool CanCost()
    {
        return _xiuWei <= RunManager.Instance.Battle.XiuWei;
    }

    public void Cost()
    {
        RunManager.Instance.Battle.RemoveXiuWei(_xiuWei);
        // RunManager.Instance.AddChanNeng(-_chanNeng);
        // RunManager.Instance.AddMingYuan(-_mingYuan);
        // RunManager.Instance.AddHealth(-_health);
    }

    public string GetDescription()
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

        if (_mingYuan != 0)
        {
            sb.Append($"{_mingYuan}命元");
        }

        if (_health != 0)
        {
            sb.Append($"{_health}生命上限");
        }

        return sb.ToString();
    }
}
