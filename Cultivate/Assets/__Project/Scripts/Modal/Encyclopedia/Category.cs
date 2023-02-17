using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Category<T> where T : Entry
{
    protected List<T> _list;
    public IEnumerable<T> Traversal
    {
        get
        {
            foreach (var item in _list)
            {
                yield return item;
            }
        }
    }

    public int GetCount() => _list.Count;
    public T Get(int i) => _list[i];

    public T Find(string name) => _list.Find(item => item.Name == name);
}
