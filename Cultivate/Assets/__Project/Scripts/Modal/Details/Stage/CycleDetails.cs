
public class CycleDetails : ClosureDetails
{
    public StageEntity Owner;
    public WuXing WuXing;
    public int Step;
    public int Gain;
    public int Recover;

    public CycleDetails(StageEntity owner, WuXing wuXing, int gain, int recover, bool induced)
    {
        Owner = owner;
        WuXing = wuXing;
        Step = 1;
        Gain = gain;
        Recover = recover;
        Induced = induced;
    }

    public CycleDetails Clone() => new(Owner, WuXing, Gain, Recover, Induced);
}
