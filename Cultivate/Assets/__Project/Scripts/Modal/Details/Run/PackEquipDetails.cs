
using UnityEngine;

public class PackEquipDetails : ClosureDetails
{
    public ConfigPack Pack;
    public PackConstraint Constraint;
    
    public PackEquipDetails(ConfigPack pack, PackConstraint constraint)
    {
        Pack = pack;
        Constraint = constraint;
    }
}
