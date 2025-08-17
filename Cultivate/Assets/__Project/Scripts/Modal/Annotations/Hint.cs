
using System;
using System.Collections.Generic;

public class Hint : AnnotatableLine
{
    private string _rawDescription;
    private Description _description;
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public Hint(string rawDescription)
    {
        _rawDescription = rawDescription;
    }

    public bool CanShowAnnotation() => true;

    public Description GetDescription()
    {
        if (_description != null)
            return _description;

        _description = new(_rawDescription);
        return _description;
    }
}
