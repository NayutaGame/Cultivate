using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RunEventDescriptor
{
    private string _description;
    public string Description => _description;

    protected RunEventDescriptor(string description)
    {
        _description = description;
    }

    public abstract void Register(RunTech runTech);
    public abstract void Unregister(RunTech runTech);
}
