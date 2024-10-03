
using System.Collections.Generic;

public class DepleteDetails : ClosureDetails
{
    public RunEntity Owner;
    public List<ISkill> DepletedSkills;

    public DepleteDetails(RunEntity owner)
    {
        Owner = owner;
        DepletedSkills = new();
    }
}
