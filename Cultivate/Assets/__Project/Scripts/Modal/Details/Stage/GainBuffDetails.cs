using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainBuffDetails : EventDetails
{
    public BuffEntry _entry;
    public int _initialStack;

    public GainBuffDetails(BuffEntry entry, int initialStack)
    {
        _entry = entry;
        _initialStack = initialStack;
    }
}
