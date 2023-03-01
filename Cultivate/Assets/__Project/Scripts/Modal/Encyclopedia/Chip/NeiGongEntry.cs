using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeiGongEntry : ChipEntry
{
    // events

    private string _description;
    public string Description;

    public NeiGongEntry(string name, JingJie jingJie, string description) : base(name, jingJie)
    {
        _description = description;
    }

    public override bool IsNeiGong => true;
}
