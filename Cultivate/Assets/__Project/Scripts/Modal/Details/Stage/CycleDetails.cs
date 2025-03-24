
public class CycleDetails : StageClosureDetails
{
    public StageEntity Owner;
    public bool Rotate;
    public WuXing WuXing;
    public int Step;
    public int Gain;
    public int Recover;

    public int Flow;

    public CycleDetails(StageEntity owner, bool rotate, WuXing wuXing, int gain, int recover, bool induced)
    {
        Owner = owner;
        Rotate = rotate;
        WuXing = wuXing;
        Step = 1;
        Gain = gain;
        Recover = recover;
        Induced = induced;
    }

    public CycleDetails Clone() => new(Owner, Rotate, WuXing, Gain, Recover, Induced);
}
