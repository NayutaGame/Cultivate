using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustDetails : EventDetails
{
    public StageEntity Owner;
    public StageSkill Skill;

    public ExhaustDetails(StageEntity owner, StageSkill skill)
    {
        Owner = owner;
        Skill = skill;
    }
}
