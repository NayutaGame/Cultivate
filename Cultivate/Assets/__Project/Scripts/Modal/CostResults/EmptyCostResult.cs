
public class EmptyCostResult : CostResult
{
    public EmptyCostResult() : base(0)
    {
    }

    public override CostDescription.CostType ToType()
        => CostDescription.CostType.Empty;
}
