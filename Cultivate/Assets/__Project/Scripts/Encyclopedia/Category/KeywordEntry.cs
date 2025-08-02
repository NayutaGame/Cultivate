
using System;
using System.Collections.Generic;
using CLLibrary;

public class KeywordEntry : Entry, AnnotatableText
{
    private string _rawDescription;
    private Description _description;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public KeywordEntry(string id, string name, string rawDescription) : base(id, name)
    {
        _rawDescription = rawDescription;
    }

    public override void Init()
        => _description = new Description(_rawDescription);

    public bool CanShowAnnotation()
        => true;

    public string GetTitle()
        => GetName();

    public Description GetDescription()
        => _description;
}
