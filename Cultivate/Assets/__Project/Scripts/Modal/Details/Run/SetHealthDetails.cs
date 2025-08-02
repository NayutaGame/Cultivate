
public class SetHealthDetails : RunClosureDetails
{
    public int Value;
    public int Diff;
    public bool IsFromIncreaseJingJie;

    private SetHealthDetails(int value, int diff, bool isFromIncreaseJingJie)
    {
        Value = value;
        Diff = diff;
        IsFromIncreaseJingJie = isFromIncreaseJingJie;
    }
    
    public static SetHealthDetails FromGain(int diff)
    {
        return new SetHealthDetails(
            value: RunManager.Instance.Environment.Home.GetHealth() + diff,
            diff: diff,
            isFromIncreaseJingJie: false
        );
    }

    public static SetHealthDetails FromLose(int diff)
    {
        return new SetHealthDetails(
            value: RunManager.Instance.Environment.Home.GetHealth() - diff,
            diff: -diff,
            isFromIncreaseJingJie: false
        );
    }

    public static SetHealthDetails FromJingJieChange(JingJie fromJingJie, JingJie toJingJie)
    {
        int baseHealth = RunManager.Instance.Environment.Home.GetHealth();
        int healthDiff = GetHealthDiffFromJingJieChange(fromJingJie, toJingJie);
        
        return new SetHealthDetails(
            value: baseHealth + healthDiff,
            diff: healthDiff,
            isFromIncreaseJingJie: true
        );
    }

    public static SetHealthDetails FromDirect(int value)
    {
        int currentHealth = RunManager.Instance.Environment.Home.GetHealth();
        return new SetHealthDetails(
            value: value,
            diff: value - currentHealth,
            isFromIncreaseJingJie: false
        );
    }

    private static int GetHealthDiffFromJingJieChange(JingJie fromJingJie, JingJie toJingJie)
    {
        if (fromJingJie == null || toJingJie == null)
        {
            ;
        }
        int diff = RunEntity.HealthFromJingJie[toJingJie] - RunEntity.HealthFromJingJie[fromJingJie];
        return diff;
    }
}