
public class BurnDetails : StageClosureDetails
{
    public StageEntity Owner;
    public int Value;

    public BurnDetails(StageEntity owner, int value, bool induced)
    {
        Owner = owner;
        Value = value;
        Induced = induced;
    }

    public BurnDetails Clone() => new(Owner, Value, Induced);
}
