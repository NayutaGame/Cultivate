using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventDescriptor
{
    public abstract void Register(RunTech runTech);
    public abstract void Unregister(RunTech runTech);
}
