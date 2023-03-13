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

    private string[] _prerequisiteNames;
    private TechEntry[] _prerequisites;
    public TechEntry[] Prerequisites => _prerequisites;

    private EventDescriptor _eureka;
    public EventDescriptor Eureka => _eureka;

    public TechEntry(string name, string description, Vector2Int position, int cost,
        string[] prerequisiteNames = null,
        EventDescriptor eureka = null,
        object[] rewards = null) : base(name)
    {
        _description = description;
        _cost = cost;
        _position = position;
        _prerequisiteNames = prerequisiteNames;
        _eureka = eureka;
    }

    public void Init()
    {
        int l = _prerequisiteNames?.Length ?? 0;
        _prerequisites = new TechEntry[l];
        for (int i = 0; i < l; i++)
            _prerequisites[i] = Encyclopedia.TechCategory[_prerequisiteNames[i]];
    }
}
