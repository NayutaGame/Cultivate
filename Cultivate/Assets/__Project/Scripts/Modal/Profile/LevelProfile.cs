
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
}
