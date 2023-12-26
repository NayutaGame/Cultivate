
using System.Text;

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
        return Gold <= RunManager.Instance.Environment.Gold &&
               MingYuan < RunManager.Instance.Environment.GetMingYuan().GetCurr() &&
               Health < RunManager.Instance.Environment.Home.GetFinalHealth();
    }

    public void Cost()
    {
        RunManager.Instance.Environment.SetDGold(-Gold);
        RunManager.Instance.Environment.SetDMingYuanProcedure(-MingYuan);
        RunManager.Instance.Environment.SetDDHealth(-Health);
    }

    public string GetDescription()
    {
        StringBuilder sb = new();
        if (Gold != 0)
        {
            sb.Append($"{Gold}金钱\t");
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
