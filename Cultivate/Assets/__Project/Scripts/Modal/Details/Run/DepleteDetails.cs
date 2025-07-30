
using System.Collections.Generic;

public class DepleteDetails : RunClosureDetails
{
    public RunEntity Owner;
    public bool PreserveFirstDeplete;
    public List<RunSkill> DepletedSkills;

    public DepleteDetails(RunEntity owner)
    {
        Owner = owner;
        PreserveFirstDeplete = false;
        DepletedSkills = new();
    }
}
