
using System;
using System.Collections.Generic;

public class ScribbleCell : Cell
{
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "Guide",                      thisObject => ((DialogCell)thisObject).GetGuideDescriptor() },
    };
    public override object Get(string s) => Accessor[s](this);
    public ScribbleCell()
    {
    }
}