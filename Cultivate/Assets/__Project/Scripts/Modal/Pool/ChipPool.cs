using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class ChipPool : AutoPool<ChipEntry>
{
    public ChipPool(List<ChipEntry> toPopulate) : base(toPopulate)
    {
    }
}
