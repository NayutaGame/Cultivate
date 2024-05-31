
using System.Collections.Generic;

public class DepleteDetails : EventDetails
{
    public RunEntity Owner;
    public List<ISkillModel> DepletedSkills;

    public DepleteDetails(RunEntity owner)
    {
        Owner = owner;
        DepletedSkills = new();
    }
}
