using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusChangedEventDescriptor : EventDescriptor
{
    public Func<StatusChangedDetails, RunTech, bool> _cond;

    public override void Register(RunTech runTech)
    {
        RunManager.Instance.StatusChangedEvent += runTech.StatusChanged;
    }

    public override void Unregister(RunTech runTech)
    {
        RunManager.Instance.StatusChangedEvent -= runTech.StatusChanged;
    }
}
