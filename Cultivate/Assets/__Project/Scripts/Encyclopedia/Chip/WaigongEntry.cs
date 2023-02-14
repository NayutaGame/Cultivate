using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaigongEntry : ChipEntry
{
    public readonly int ManaCost;
    public readonly Action Execute;

    public WaigongEntry(string name, string description, int manaCost = 0, Action execute = null) : base(name, description)
    {
        ManaCost = manaCost;
        Execute = execute;
    }
}
