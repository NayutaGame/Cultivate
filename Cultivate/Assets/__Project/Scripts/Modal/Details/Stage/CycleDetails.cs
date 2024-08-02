
public class CycleDetails : EventDetails
{
    public StageEntity Owner;
    public WuXing WuXing;
    public int Step;
    public int Gain;
    public int Recover;

    public CycleDetails(StageEntity owner, WuXing wuXing, int gain, int recover)
    {
        Owner = owner;
        WuXing = wuXing;
        Step = 1;
        Gain = gain;
        Recover = recover;
    }

    public CycleDetails Clone() => new(Owner, WuXing, Gain, Recover);
}
