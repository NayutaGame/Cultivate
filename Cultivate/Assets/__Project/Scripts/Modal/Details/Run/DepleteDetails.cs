
using System.Collections.Generic;

public class DepleteDetails : ClosureDetails
{
    public RunEntity Owner;
    public bool PreserveFirstDeplete;
    public List<ISkill> DepletedSkills;

    public DepleteDetails(RunEntity owner)
    {
        Owner = owner;
        PreserveFirstDeplete = false;
        DepletedSkills = new();
    }
}
