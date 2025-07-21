
using System;
using System.Collections.Generic;
using CLLibrary;

public class KeywordEntry : Entry, AnnotatableText, LegacyAnnotatable
{
    private string _description;
    private AnnotationArray _cascade;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public KeywordEntry(string id, string description) : base(id)
    {
        _description = description;
    }
    
    public string GetName() => GetId();
    public Description GetLiteralDescription() => _description;

    public string GetHighlight()
        => GetLiteralDescription().GetHighlight(_cascade);
    
    public string GetCascadeAnnotated()
        => _cascade.GetCascadeAnnotated();
    
    public void GenerateCascade()
        => _cascade = AnnotationArray.FromDescription(GetLiteralDescription());

    public static KeywordEntry FromName(string name)
        => Encyclopedia.KeywordCategory.FirstObj(e => e.GetName() == name) ?? Encyclopedia.KeywordCategory.DefaultEntry();

    public bool CanShowAnnotation()
        => true;

    public string GetTitle()
        => GetName();

    public string GetDescription()
        => _description;
}
