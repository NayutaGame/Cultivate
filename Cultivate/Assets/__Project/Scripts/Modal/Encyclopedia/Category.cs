using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Category<T> where T : Entry
{
    protected List<T> _list;

    public T Find(string name) => _list.Find(item => item.Name == name);
}
