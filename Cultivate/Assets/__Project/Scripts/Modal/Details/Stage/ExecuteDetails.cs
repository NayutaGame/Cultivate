using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteDetails : StageEventDetails
{
    public StageEntity Caster;
    public StageSkill Skill;

    public ExecuteDetails(StageEntity caster, StageSkill skill)
    {
        Caster = caster;
        Skill = skill;
    }
}
