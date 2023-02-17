using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class Encyclopedia : Singleton<Encyclopedia>
{
    public static ChipCategory ChipCategory;
    public static BuffCategory BuffCategory;

    public override void DidAwake()
    {
        base.DidAwake();

        BuffCategory = new BuffCategory();
        ChipCategory = new ChipCategory();
    }
}
