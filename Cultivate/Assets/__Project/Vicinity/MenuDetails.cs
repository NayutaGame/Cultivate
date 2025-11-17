
using System;
using System.Collections.Generic;

public class MenuDetails: Addressable
{
    public ListModel<string> Options;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Options",            thisObject => ((MenuDetails)thisObject).Options },
    };
    public object Get(string s) => Accessor[s](this);
    public MenuDetails(List<string> options)
    {
        Options = new ListModel<string>();
        Options.AddRange(options);
    }
}