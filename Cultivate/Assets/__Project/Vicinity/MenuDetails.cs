
using System;
using System.Collections.Generic;

public class MenuDetails: Addressable
{
    public ListModel<MenuOption> Options;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Options",            thisObject => ((MenuDetails)thisObject).Options },
    };
    public object Get(string s) => Accessor[s](this);
    public MenuDetails(List<MenuOption> options)
    {
        Options = new ListModel<MenuOption>();
        Options.AddRange(options);
    }
}