using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public interface EmulatedSkill
{
    SkillEntry GetEntry();
    JingJie GetJingJie();
    int GetManaCost();

    int GetRunUsedTimes();
    void SetRunUsedTimes(int value);

    int GetRunEquippedTimes();
    void SetRunEquippedTimes(int value);
}
