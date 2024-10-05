
public class GainArmorDetails : ClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;

    public GainArmorDetails(StageEntity src, StageEntity tgt, int value, bool induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Induced = induced;
    }
    
    public GainArmorDetails Clone() => new(Src, Tgt, Value, Induced);
}
