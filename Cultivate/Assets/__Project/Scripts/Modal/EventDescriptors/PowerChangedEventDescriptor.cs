using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerChangedEventDescriptor : EventDescriptor
{
    public Func<PowerChangedDetails, RunTech, bool> _cond;

    public override void Register(RunTech runTech)
    {
        RunManager.Instance.PowerChangedEvent += runTech.PowerChanged;
    }

    public override void Unregister(RunTech runTech)
    {
        RunManager.Instance.PowerChangedEvent -= runTech.PowerChanged;
    }

    public PowerChangedEventDescriptor(string description, Func<PowerChangedDetails, RunTech, bool> cond) : base(description)
    {
        _cond = cond;
    }
}
