using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XiulianEntry : ChipEntry
{
    private string _description;
    public string Description;

    public Func<Tile, bool> CanApply;
    public Action<Tile> Apply;

    public XiulianEntry(string name, JingJie jingJie, string description, Func<Tile, bool> canApply, Action<Tile> apply) : base(name, jingJie)
    {
        _description = description;
        CanApply = canApply;
        Apply = apply;
    }

    public override bool IsXiuLian => true;
}
