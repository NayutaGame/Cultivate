
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
    public KeywordEntry(string id, string rawDescription) : base(id)
    {
        _rawDescription = rawDescription;
    }
    
    public string GetName() => GetId();

    public void GenerateDescription()
        => _description = new Description(_rawDescription);

    public static KeywordEntry FromName(string name)
        => Encyclopedia.KeywordCategory.FirstObj(e => e.GetName() == name) ?? Encyclopedia.KeywordCategory.DefaultEntry();

    public bool CanShowAnnotation()
        => true;

    public string GetTitle()
        => GetName();

    public Description GetDescription()
        => _description;
}
