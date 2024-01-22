
using System.Collections.Generic;

public class Category<T> where T : Entry
{
    private List<T> _list;
    protected List<T> List => _list;

    public void AddRange(IEnumerable<T> collection)
    {
        List.AddRange(collection);
        RefreshDict();
    }

    private Dictionary<string, T> _dict;

    public Category()
    {
        _list = new();
        _dict = new();
    }

    private void RefreshDict()
    {
        _dict.Clear();

        foreach (T item in _list)
            _dict[item.GetName()] = item;
    }

    public IEnumerable<T> Traversal
    {
        get
        {
            foreach (var item in _list)
                yield return item;
        }
    }

    public int GetCount() => _list.Count;

    public T this[int i] => _list[i];
    public T this[string key]
    {
        get
        {
            bool success = _dict.TryGetValue(key, out T value);
            if (success)
                return value;
            else
                return DefaultEntry();
        }
    }

    public virtual T DefaultEntry() => null;

    public int IndexOf(T item)
        => _list.IndexOf(item);
}
