
public class PackUnequipDetails
{
    public PackConstraint Constraint;
    public ConfigPack Pack;

    public int ConstraintIndex;
    public int SelectionIndex;
    
    public PackUnequipDetails(PackConstraint constraint, ConfigPack pack, int constraintIndex, int selectionIndex)
    {
        Constraint = constraint;
        Pack = pack;
        ConstraintIndex = constraintIndex;
        SelectionIndex = selectionIndex;
    }
}
