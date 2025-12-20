
public abstract class GainRow
{
    public enum GainStyle
    {
        Inactive,
        Positive,
        Negative,
    }

    protected GainRow()
    {
    }

    public abstract void InvalidateCache();
    public abstract int CalculateGain();
    public abstract GainStyle GetGainStyle();
    public abstract Description GetDescriptionText();
    public abstract string GetScoreText();
}