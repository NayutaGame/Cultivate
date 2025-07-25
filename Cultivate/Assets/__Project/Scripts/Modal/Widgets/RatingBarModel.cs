
public class RatingBarModel
{
    public int MaxValue;
    public int Value;
    public int[] CriticalValues;

    public RatingBarModel(int maxValue, int value, int[] criticalValues)
    {
        MaxValue = maxValue;
        Value = value;
        CriticalValues = criticalValues;
    }
}