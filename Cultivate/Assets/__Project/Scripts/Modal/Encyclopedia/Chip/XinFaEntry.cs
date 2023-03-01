using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XinFaEntry : ChipEntry
{
    private string _description;
    public string Description;

    public XinFaEntry(string name, JingJie jingJie, string description) : base(name, jingJie)
    {
        _description = description;
    }

    public override bool IsXinFa => true;
}
