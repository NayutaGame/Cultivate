using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileResourceEntry : Entry
{
    private string _description;
    public string Description;

    public TileResourceEntry(string name, string description) : base(name)
    {
        _description = description;
    }
}
