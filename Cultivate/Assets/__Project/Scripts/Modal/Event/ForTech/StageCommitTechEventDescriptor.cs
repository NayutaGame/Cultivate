
using System;

public class StageCommitTechEventDescriptor : TechEventDescriptor
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

    public StageCommitTechEventDescriptor(string description, Func<StageCommitDetails, RunTech, bool> cond) : base(description)
    {
        _cond = cond;
    }
}
