using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeigongEntry : ChipEntry
{
    // events

    public NeigongEntry(string name, string description) : base(name, description)
    {
    }

    public override bool IsNeiGong => true;
}
