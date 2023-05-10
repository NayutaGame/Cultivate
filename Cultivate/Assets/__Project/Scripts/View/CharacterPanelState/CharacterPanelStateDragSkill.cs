using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanelStateDragSkill : CharacterPanelState
{
    private ISkillModel _skill;
    public ISkillModel Skill => _skill;

    public CharacterPanelStateDragSkill(ISkillModel skill)
    {
        _skill = skill;
    }
}
