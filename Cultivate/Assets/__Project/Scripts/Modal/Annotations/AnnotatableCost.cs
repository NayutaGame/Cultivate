
public interface AnnotatableCost : Annotatable
{
    JingJie GetJingJie();
    CostDescription GetLiteralCostDescription(JingJie showingJingJie);
}