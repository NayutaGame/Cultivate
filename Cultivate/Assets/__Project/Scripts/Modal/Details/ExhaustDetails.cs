using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustDetails
{
    public StageEntity Owner;
    public StageSkill Skill;
    public bool ForRun;

    public ExhaustDetails(StageEntity owner, StageSkill skill, bool forRun)
    {
        Owner = owner;
        Skill = skill;
        ForRun = forRun;
    }
}
