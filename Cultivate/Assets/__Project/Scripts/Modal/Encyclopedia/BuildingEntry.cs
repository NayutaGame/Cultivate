using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEntry : Entry
{
    private string _description;
    public string Description;

    private int _cost;
    public int Cost => _cost;

    public BuildingEntry(string name, string description, int cost) : base(name)
    {
        _description = description;
        _cost = cost;
    }
}
