
using System.Text;

public class RunCostDetails
{
    public int Gold;
    public int MingYuan;
    public int Health;

    public RunCostDetails(int gold = 0, int mingYuan = 0, int health = 0)
    {
        Gold = gold;
        MingYuan = mingYuan;
        Health = health;
    }

    public bool CanCost()
    {
        return Gold <= RunManager.Instance.Environment.GetGold().Curr &&
               MingYuan < RunManager.Instance.Environment.GetCurrMingYuan() &&
               Health < RunManager.Instance.Environment.Home.GetHealth();
    }

    public void Cost()
    {
        RunManager.Instance.Environment.LoseGoldProcedure(Gold);
        RunManager.Instance.Environment.LoseMingYuanProcedure(MingYuan);
        RunManager.Instance.Environment.LoseHealthProcedure(Health);
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
            sb.Append($"{Health}气血上限");
        }

        return sb.ToString();
    }

    public static RunCostDetails Default => new();
}
