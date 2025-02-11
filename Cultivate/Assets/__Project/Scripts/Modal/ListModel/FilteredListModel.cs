
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class FilteredListModel<T> : IListModel
{
    public event Func<int, object, UniTask> InsertEvent;
    public event Func<int, UniTask> RemoveAtEvent;
    public event Func<int, UniTask> ModifiedEvent;
    public event Func<UniTask> ResyncEvent;

    public int Count()
        => _filteredList.Count;

    public object Get(int index)
        => _filteredList[index];

    private ListModel<T> _list;
    private Predicate<T> _filter;

    private List<T> _filteredList;

    public void SetFilter(Predicate<T> filter)
    {
        _filter = filter;
        Refresh();
    }

    public void Refresh()
    {
        _filteredList = _list.Filter(_filter);
        ResyncEvent?.Invoke();
    }

    public FilteredListModel(ListModel<T> list, Predicate<T> filter = null)
    {
        _list = list;
        SetFilter(filter ?? NullFilter);
    }

    private static Predicate<T> NullFilter
        => T => true;
}
