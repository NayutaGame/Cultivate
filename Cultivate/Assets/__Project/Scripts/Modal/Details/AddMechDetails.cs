using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class AddMechDetails
{
    [CanBeNull] public MechType _mechType;
    public int _count;

    public AddMechDetails([CanBeNull] MechType mechType = null, int count = 1)
    {
        _mechType = mechType;
        _count = count;
    }
}
