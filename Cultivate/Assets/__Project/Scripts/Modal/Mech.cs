using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech
{
    private MechType _mechType;
    public MechType GetMechType()
        => _mechType;

    public int Count;

    public Mech(MechType mechType)
    {
        _mechType = mechType;
        Count = 0;
    }
}
