using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordEntry : Entry
{
    private string _description;
    public string Description;

    public KeywordEntry(string name, string description) : base(name)
    {
        _description = description;
    }
}
