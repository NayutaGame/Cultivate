
using System;
using System.Collections.Generic;

public class TagComposite : Addressable
{
    private long _value;
    private ListModel<TagEntry> _tagList;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "TagList",                    thisObject => ((TagComposite)thisObject).GetTagList() },
    };
    public object Get(string s) => Accessor[s](this);
    public TagComposite(long value)
    {
        _value = value;
    }
    
    public long Value => _value;
    
    public ListModel<TagEntry> GetTagList()
    {
        if (_tagList != null)
            return _tagList;

        _tagList = new ListModel<TagEntry>();
        foreach (TagEntry tagEntry in Encyclopedia.TagCategory)
        {
            if (Contains(tagEntry))
                _tagList.Add(tagEntry);
        }
        
        return _tagList;
    }

    public bool Contains(TagComposite other)
    {
        return ((this & other) == other) &&
               (this | other) == this;
    }

    public bool Contains(TagEntry other)
    {
        return ((_value & other.Value) == other.Value) &&
               (this | other.Value)._value == _value;
    }

    public static implicit operator long(TagComposite tagComposite) => tagComposite._value;
    public static implicit operator TagComposite(long value) => new(value);
    public static implicit operator TagComposite(TagEntry tagEntry) => tagEntry.Value;

    public static TagComposite operator |(TagComposite left, TagComposite right) => new(left._value | right._value);
    public static TagComposite operator |(TagComposite left, long right) => new(left._value | right);
    public static TagComposite operator |(long left, TagComposite right) => new(left | right._value);
    public static TagComposite operator |(TagComposite left, TagEntry right) => new(left._value | right.Value);
    public static TagComposite operator |(TagEntry left, TagComposite right) => new(left.Value | right._value);

    public TagComposite Clone() => _value;
}
