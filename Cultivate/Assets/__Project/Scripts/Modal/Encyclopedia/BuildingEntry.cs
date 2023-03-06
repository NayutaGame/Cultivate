using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEntry : Entry, ILock
{
    private string _description;
    public string Description;

    private int _cost;
    public int Cost => _cost;

    private Func<BuildingProduct, Tile, bool> _canDrop;

    private Action<BuildingProduct, Tile> _drop;

    public BuildingEntry(string name, string description, int cost, Func<BuildingProduct, Tile, bool> canDrop = null, Action<BuildingProduct, Tile> drop = null) : base(name)
    {
        _description = description;
        _cost = cost;
        _canDrop = canDrop ?? ((bp, tile) => true);
        _drop = drop ?? ((bp, tile) => { });
    }

    public bool CanDrop(BuildingProduct bp, Tile tile) => _canDrop(bp, tile);
    public void Drop(BuildingProduct bp, Tile tile) => _drop(bp, tile);
}
