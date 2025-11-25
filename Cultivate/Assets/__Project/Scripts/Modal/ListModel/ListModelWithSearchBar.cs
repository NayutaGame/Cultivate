
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class ListModelWithSearchBar<T> : IListModel, IListModel<T>, Addressable where T : ISearchable
{
    [NonSerialized] private string _searchText;
    [NonSerialized] private ListModel<T> _original;
    [NonSerialized] private FilteredListModel<T> _filtered;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Browser",                      thisObject => ((ListModelWithSearchBar<T>)thisObject)._filtered },
    };
    public object Get(string s) => Accessor[s](this);
    public ListModelWithSearchBar(ListModel<T> original)
    {
        _filtered = new(original, item => item.MatchSearchText(GetSearchText()));
        SetSearchText("");
    }
    
    private string GetSearchText() => _searchText;
    public void SetSearchText(string value)
    {
        _searchText = value;
        _filtered.Refresh();
    }

    public event Func<int, object, UniTask> InsertEvent;
    public event Func<int, UniTask> RemoveAtEvent;
    public event Func<int, UniTask> ModifiedEvent;
    public event Func<UniTask> ResyncEvent;

    public int Count()
        => _filtered.Count();

    public object Get(int index)
        => _filtered.Get(index);
}
