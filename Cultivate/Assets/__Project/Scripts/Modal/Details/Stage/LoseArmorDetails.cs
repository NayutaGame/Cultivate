
public class LoseArmorDetails : ClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;

    public LoseArmorDetails(StageEntity src, StageEntity tgt, int value, bool induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Induced = induced;
    }
    
    public LoseArmorDetails Clone() => new(Src, Tgt, Value, Induced);
}
