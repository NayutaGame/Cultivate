using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ChipPool : Pool<ChipEntry>
{
    public ChipPool()
    {
        PopulateChips();
    }

    public void PopulateChips()
    {
        for(int i = 0; i < 1; i++)
            Populate(Encyclopedia.ChipCategory.Traversal);
    }

    public bool TryPopFirst(JingJie jingJie, out ChipEntry item)
        => TryPopFirst(entry => entry.JingJie <= jingJie, out item);
}
