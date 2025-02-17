
using System;
using UnityEngine;

[Serializable]
public class LevelProfile
{
    [SerializeField] private int _level;
    [SerializeField] private int _experience;

    public int Level => _level;
    public int Experience => _experience;

    private LevelProfile(int level, int experience)
    {
        _level = level;
        _experience = experience;
    }

    public static LevelProfile Default()
        => new(1, 0);
    
    public static LevelProfile Developer()
        => new(10, 1000);

    public void GainExperience(int experienceGain)
    {
        _experience += experienceGain;

        const int EXPERIENCE_PER_LEVEL = 1000;

        int levelUpCount = _experience / EXPERIENCE_PER_LEVEL;
        _experience = _experience % EXPERIENCE_PER_LEVEL;

        _level += levelUpCount;
    }

    public (int, int) GainExperienceDryRun(int experienceGain)
    {
        const int EXPERIENCE_PER_LEVEL = 1000;

        int finalExperience = (_experience + experienceGain) % EXPERIENCE_PER_LEVEL;
        int finalLevel = _level + (_experience + experienceGain) / EXPERIENCE_PER_LEVEL;

        return (finalExperience, finalLevel);
    }
}
