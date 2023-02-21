using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XinfaEntry : ChipEntry
{
    private string _description;
    public string Description;

    public XinfaEntry(string name, string description) : base(name)
    {
        _description = description;
    }
}
