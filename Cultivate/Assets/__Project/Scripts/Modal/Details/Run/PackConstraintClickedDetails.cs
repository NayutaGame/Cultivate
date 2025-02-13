
public class PackConstraintClickedDetails : ClosureDetails
{
    public PackConstraint Constraint;
    
    // public ConfigPack Pack;
    // public int PackIndex;
    
    public PackConstraintClickedDetails(PackConstraint constraint)
    {
        Constraint = constraint;
    }
}
