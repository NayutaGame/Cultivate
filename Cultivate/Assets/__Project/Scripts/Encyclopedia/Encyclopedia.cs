using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class Encyclopedia : Singleton<Encyclopedia>
{
    public static ChipCategory ChipCategory;

    public override void DidAwake()
    {
        base.DidAwake();

        ChipCategory = new ChipCategory();
    }
}
