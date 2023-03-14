using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildEventDescriptor : EventDescriptor
{
    public Func<BuildDetails, RunTech, bool> _cond;

    public override void Register(RunTech runTech)
    {
        RunManager.Instance.BuildEvent += runTech.Build;
    }

    public override void Unregister(RunTech runTech)
    {
        RunManager.Instance.BuildEvent -= runTech.Build;
    }

    public BuildEventDescriptor(string description, Func<BuildDetails, RunTech, bool> cond) : base(description)
    {
        _cond = cond;
    }
}
