using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeDetails
{
    public StageEntity Owner;
    public StageSkill Skill;
    public bool ForRun;

    public ConsumeDetails(StageEntity owner, StageSkill skill, bool forRun)
    {
        Owner = owner;
        Skill = skill;
        ForRun = forRun;
    }
}
