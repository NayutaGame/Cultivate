
public class PackEquipDetails
{
    public ConfigPack Pack;
    public PackConstraint Constraint;

    public int SelectionIndex;
    public int ConstraintIndex;
    
    public PackEquipDetails(ConfigPack pack, PackConstraint constraint, int selectionIndex, int constraintIndex)
    {
        Pack = pack;
        Constraint = constraint;
        SelectionIndex = selectionIndex;
        ConstraintIndex = constraintIndex;
    }
}
