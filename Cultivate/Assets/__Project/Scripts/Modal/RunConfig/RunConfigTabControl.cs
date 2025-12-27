
using System;
using System.Collections.Generic;

public abstract class RunConfigTabControl : Addressable
{
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "Slots",                      thisObject => ((RunConfigTabControl)thisObject)._filteredSlots },
    };
    public abstract object Get(string s);
    public RunConfigTabControl()
    {
    }

    public abstract void WriteRecord();
    public abstract void ReadRecord();
    public abstract bool IsValid();
}