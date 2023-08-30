using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaShortageDetails : EventDetails
{
    public StageEntity Owner;
    public int Position;
    public StageSkill Skill;

    public ManaShortageDetails(StageEntity owner, int position, StageSkill skill)
    {
        Owner = owner;
        Position = position;
        Skill = skill;
    }
}
