using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ChipPool : Pool<ChipEntry>
{
    public ChipPool()
    {
        for(int i = 0; i < 8; i++)
            Populate(Encyclopedia.ChipCategory.Traversal);
        Shuffle();
    }

    public bool TryPopFirst(JingJie jingJie, out ChipEntry item)
    {
        return TryPopFirst(entry => entry.JingJie <= jingJie, out item);
    }
}
