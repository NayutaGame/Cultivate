using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CostDetails
{
    public int Gold;
    public int MingYuan;
    public int Health;

    public CostDetails(int gold = 0, int mingYuan = 0, int health = 0)
    {
        Gold = gold;
        MingYuan = mingYuan;
        Health = health;
    }

    public bool CanCost()
    {
        return Gold <= RunManager.Instance.Battle.XiuWei &&
               MingYuan < RunManager.Instance.Battle.GetMingYuan().GetCurr() &&
               Health < RunManager.Instance.Battle.Home.GetFinalHealth();
    }

    public void Cost()
    {
        RunManager.Instance.Battle.RemoveXiuWei(Gold);
        RunManager.Instance.Battle.SetDMingYuan(-MingYuan);
        RunManager.Instance.Battle.Home.SetDHealth(-Health);
    }

    public string GetDescription()
    {
        StringBuilder sb = new();
        if (Gold != 0)
        {
            sb.Append($"{Gold}修为\t");
        }

        if (MingYuan != 0)
        {
            sb.Append($"{MingYuan}命元\t");
        }

        if (Health != 0)
        {
            sb.Append($"{Health}生命上限");
        }

        return sb.ToString();
    }

    public static CostDetails Default => new();
}
