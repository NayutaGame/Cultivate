
using System;

public class StatusChangedTechEventDescriptor : TechEventDescriptor
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

    public StatusChangedTechEventDescriptor(string description, Func<StatusChangedDetails, RunTech, bool> cond) : base(description)
    {
        _cond = cond;
    }
}
