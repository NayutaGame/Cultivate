
using System;
using System.Collections.Generic;

public class TagComposite : Addressable
{
    private long _value;
    public long Value => _value;

    private ListModel<TagEntry> _tagList;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "TagList",                    thisObject => ((TagComposite)thisObject)._tagList },
    };
    public object Get(string s) => Accessor[s](this);
    public TagComposite(long value)
    {
        _value = value;

        _tagList = new ListModel<TagEntry>();
        foreach (TagEntry tagEntry in Encyclopedia.TagCategory)
        {
            if (Contains(tagEntry))
                _tagList.Add(tagEntry);
        }
    }

    public bool Contains(TagComposite other)
    {
        return ((this & other) == other) &&
               (this | other) == this;
    }

    public bool Contains(TagEntry other)
    {
        return ((this & other.Value) == other.Value) &&
               (this | other.Value) == this;
    }

    public static implicit operator long(TagComposite tagComposite) => tagComposite._value;
    public static implicit operator TagComposite(long value) => new(value);
    public static implicit operator TagComposite(TagEntry tagEntry) => tagEntry.Value;

    public TagComposite Clone() => _value;

    public static TagComposite FromWuXing(WuXing? wuXing)
    {
        return (1 << (wuXing?._index)) ?? 0;
    }
}
