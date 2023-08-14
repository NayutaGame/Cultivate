using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public interface EmulatedSkill
{
    SkillSlot GetSkillSlot();
    void SetSkillSlot(SkillSlot value);

    SkillEntry GetEntry();
    JingJie GetJingJie();
    int GetManaCost();
    int GetChannelTime();

    int GetRunUsedTimes();
    void SetRunUsedTimes(int value);

    int GetRunEquippedTimes();
    void SetRunEquippedTimes(int value);
}
