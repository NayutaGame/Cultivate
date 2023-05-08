using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StepDetails
{
    public StageEntity Owner;
    public StageSkill Skill;

    public StepDetails(StageEntity owner, StageSkill skill)
    {
        Owner = owner;
        Skill = skill;
    }
}
