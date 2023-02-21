using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeigongEntry : ChipEntry
{
    // events

    private string _description;
    public string Description;

    public NeigongEntry(string name, string description) : base(name)
    {
        _description = description;
    }

    public override bool IsNeiGong => true;
}
