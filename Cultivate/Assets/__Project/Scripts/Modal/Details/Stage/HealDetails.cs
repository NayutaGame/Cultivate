
public class HealDetails : EventDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Penetrate;

    public HealDetails(StageEntity src, StageEntity tgt, int value, bool penetrate = false)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Penetrate = penetrate;
    }

    public HealDetails Clone() => new(Src, Tgt, Value, Penetrate);
}
