
public abstract class Reward
{
    public abstract void Claim();
    public abstract string GetDescription();

    public static Reward FromGold(int gold)
    {
        return new ResourceReward(gold: gold);
    }

    public static Reward FromHealth(int health)
    {
        return new ResourceReward(health: health);
    }

    public static Reward FromMingYuan(int mingYuan)
    {
        return new ResourceReward(mingYuan: mingYuan);
    }
}
