
public class PackUnequipDetails : ClosureDetails
{
    public PackConstraint Constraint;
    public ConfigPack Pack;
    
    public PackUnequipDetails(PackConstraint constraint, ConfigPack pack)
    {
        Constraint = constraint;
        Pack = pack;
    }
}
