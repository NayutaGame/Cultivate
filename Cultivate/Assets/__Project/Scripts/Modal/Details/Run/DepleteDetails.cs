
using System.Collections.Generic;

public class DepleteDetails : EventDetails
{
    public RunEntity Owner;
    public List<EmulatedSkill> DepletedSkills;

    public DepleteDetails(RunEntity owner)
    {
        Owner = owner;
        DepletedSkills = new();
    }
}
