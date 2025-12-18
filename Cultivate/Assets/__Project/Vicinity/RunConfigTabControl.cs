
using System;
using System.Collections.Generic;

public abstract class RunConfigTabControl : Addressable
{
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "Slots",                      thisObject => ((RunConfigTabControl)thisObject)._filteredSlots },
    };
    public object Get(string s) => Accessor[s](this);
    public RunConfigTabControl()
    {
    }
}