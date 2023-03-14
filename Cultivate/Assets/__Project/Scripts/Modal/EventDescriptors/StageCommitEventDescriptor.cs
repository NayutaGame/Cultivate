using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCommitEventDescriptor : EventDescriptor
{
    public Func<StageCommitDetails, RunTech, bool> _cond;

    public override void Register(RunTech runTech)
    {
        RunManager.Instance.StageCommitEvent += runTech.StageCommit;
    }

    public override void Unregister(RunTech runTech)
    {
        RunManager.Instance.StageCommitEvent -= runTech.StageCommit;
    }

    public StageCommitEventDescriptor(string description, Func<StageCommitDetails, RunTech, bool> cond) : base(description)
    {
        _cond = cond;
    }
}
