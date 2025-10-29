
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    public static TagComposite FromTagType(TagType tagType)
    {
        return (TagType.None == tagType) ? new(0) : new((int)tagType << 6);
    }
    
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

    public string GetTagListString()
    {
        ListModel<TagEntry> tagList = GetTagList();
        StringBuilder sb = new();
        for (int i = 0; i < tagList.Count(); i++)
        {
            sb.Append(tagList[i].GetName());
            if (i != tagList.Count() - 1)
                sb.Append("&");
        }

        return sb.ToString();
    }

    public bool Contains(TagComposite other)
    {
        return ((_value & other._value) == other._value) &&
               ((_value | other._value) == _value);
    }

    public bool Contains(TagEntry other)
    {
        return ((_value & other.Value) == other.Value) &&
               (_value | other.Value) == _value;
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
