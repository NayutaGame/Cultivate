using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using UnityEngine;

public class TechEntry : Entry
{
    public string GetName() => GetId();
    
    private string _description;
    public string Description;

    private int _cost;
    public int Cost => _cost;

    private Vector2Int _position;
    public Vector2Int Position => _position;

    private string[] _prerequisiteNames;
    private TechEntry[] _prerequisites;
    public TechEntry[] Prerequisites => _prerequisites;

    private TechEventDescriptor _eureka;
    public TechEventDescriptor Eureka => _eureka;

    private Reward[] _rewards;
    public Reward[] Rewards => _rewards;

    public TechEntry(string id, string description, Vector2Int position, int cost,
        string[] prerequisiteNames = null,
        TechEventDescriptor eureka = null,
        Reward[] rewards = null) : base(id)
    {
        _description = description;
        _cost = cost;
        _position = position;
        _prerequisiteNames = prerequisiteNames;
        _eureka = eureka;
        _rewards = rewards ?? Array.Empty<Reward>();
    }

    public void Init()
    {
        int l = _prerequisiteNames?.Length ?? 0;
        _prerequisites = new TechEntry[l];
        for (int i = 0; i < l; i++)
            _prerequisites[i] = Encyclopedia.TechCategory[_prerequisiteNames[i]];
    }

    public void Claim()
    {
        _rewards.Do(reward => reward.Claim());
    }

    public string GetEurekaString()
    {
        if (_eureka == null)
            return "";

        if (_eureka.Description == null)
            return "无文本";

        return _eureka.Description;
    }

    public string GetRewardsString()
    {
        StringBuilder sb = new();

        _rewards.Do(r =>
        {
            sb.Append($"{r.GetDescription()}; ");
        });

        return sb.ToString();
    }
}
