
public class HealDetails : ClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Penetrate;

    public HealDetails(StageEntity src, StageEntity tgt, int value, bool penetrate, bool induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Penetrate = penetrate;
        Induced = induced;
    }

    public HealDetails Clone() => new(Src, Tgt, Value, Penetrate, Induced);
}
