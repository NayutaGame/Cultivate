
public class EmptyCostDefinition : CostDefinition
{
    public EmptyCostDefinition() : base(0) { }

    public override CostDescription GetLiteralCostDescription()
        => new(CostType.Empty, CostState.Normal, Value);
}