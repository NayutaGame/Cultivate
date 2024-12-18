
using System.Text;

public class ResourceReward : Reward
{
    public int _gold;
    public int _mingYuan;
    public int _health;

    public ResourceReward(int gold = 0, int mingYuan = 0, int health = 0)
    {
        _gold = gold;
        _mingYuan = mingYuan;
        _health = health;
    }

    public override void Claim()
    {
        RunManager.Instance.Environment.SetDGoldProcedure(_gold);
        RunManager.Instance.Environment.SetDMingYuanProcedure(_mingYuan);
        RunManager.Instance.Environment.SetDHealthProcedure(_health);
    }

    public override string GetDescription()
    {
        StringBuilder sb = new();
        if (_gold != 0)
        {
            sb.Append($"{_gold}金钱\t");
        }

        if (_mingYuan != 0)
        {
            sb.Append($"{_mingYuan}命元\t");
        }

        if (_health != 0)
        {
            sb.Append($"{_health}气血上限");
        }

        return sb.ToString();
    }
}
