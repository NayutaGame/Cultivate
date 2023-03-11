using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechEntry : Entry
{
    private string _description;
    public string Description;

    private int _cost;
    public int Cost => _cost;

    private Vector2Int _position;
    public Vector2Int Position => _position;

    private string[] _prerequisiteStrings;
    private TechEntry[] _prerequisites;
    public TechEntry[] Prerequisites => _prerequisites;

    public TechEntry(string name, string description, Vector2Int position, int cost, string[] prerequisiteStrings = null, string eurekaEvent = null, string eurekaCondition = null, object[] rewards = null) : base(name)
    {
        _description = description;
        _cost = cost;
        _position = position;
        _prerequisiteStrings = prerequisiteStrings;
    }

    public void Init()
    {
        int l = _prerequisiteStrings?.Length ?? 0;
        _prerequisites = new TechEntry[l];
        for (int i = 0; i < l; i++)
            _prerequisites[i] = Encyclopedia.TechCategory[_prerequisiteStrings[i]];
    }
}
