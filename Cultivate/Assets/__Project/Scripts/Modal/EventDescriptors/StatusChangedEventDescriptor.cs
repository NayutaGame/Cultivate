using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusChangedEventDescriptor : EventDescriptor
{
    private Func<object, bool> _pred;

    public StatusChangedEventDescriptor(Func<object, bool> pred)
    {
        _pred = pred;
    }
}
