using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipEntry : Entry
{
    public ChipEntry(string name, string description) : base(name, description)
    {
    }

    public virtual bool IsNeiGong => false;
    public virtual bool IsWaiGong => false;
}
