
public class BurnDetails : StageClosureDetails
{
    public StageEntity Owner;
    public int Value;

    public BurnDetails(StageEnvironment env, StageEntity owner, int value, bool induced) : base(env)
    {
        Owner = owner;
        Value = value;
        Induced = induced;
    }

    public BurnDetails Clone() => new(Env, Owner, Value, Induced);
}
