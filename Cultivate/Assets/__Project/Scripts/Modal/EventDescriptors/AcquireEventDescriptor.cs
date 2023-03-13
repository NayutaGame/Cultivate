using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquireEventDescriptor : EventDescriptor
{
    public Func<AcquireDetails, RunTech, bool> _cond;

    public override void Register(RunTech runTech)
    {
        RunManager.Instance.AcquireEvent += runTech.Acquire;
    }

    public override void Unregister(RunTech runTech)
    {
        RunManager.Instance.AcquireEvent -= runTech.Acquire;
    }

    public AcquireEventDescriptor(Func<AcquireDetails, RunTech, bool> cond)
    {
        _cond = cond;
    }
}
