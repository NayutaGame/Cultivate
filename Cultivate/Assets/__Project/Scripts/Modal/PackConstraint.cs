
using System;
using System.Collections.Generic;

public class PackConstraint : Addressable
{
    public PackDescriptor Descriptor;
    public ConfigPack Pack;

    public bool IsEmpty => Pack == null;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public PackConstraint(PackDescriptor descriptor)
    {
        _accessors = new()
        {
            { "Pack", () => Pack },
        };

        Descriptor = descriptor;
        Pack = null;
    }
}
