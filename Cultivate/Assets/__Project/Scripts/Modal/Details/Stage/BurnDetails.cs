
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

    public static BurnDetails FromHealthCost(CostDetails d)
        => new(d.Env, d.Entity, d.Value, true);

    public static BurnDetails FromBecomeLowHealth(StageEnvironment env, StageEntity owner, int value, bool induced)
        => new(env, owner, value, induced);
}
