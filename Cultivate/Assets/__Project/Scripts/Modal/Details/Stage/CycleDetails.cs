
public class CycleDetails : EventDetails
{
    public StageEntity Owner;
    public WuXing WuXing;
    public int Gain;
    public int Recover;

    public CycleDetails(StageEntity owner, WuXing wuXing, int gain, int recover)
    {
        Owner = owner;
        WuXing = wuXing;
        Gain = gain;
        Recover = recover;
    }

    public CycleDetails Clone() => new(Owner, WuXing, Gain, Recover);
}
