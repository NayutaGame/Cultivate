using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry
{
    private string _name;
    public string Name => _name;

    private string _description;
    public string Description;

    public Entry(string name, string description)
    {
        _name = name;
        _description = description;
    }
}
