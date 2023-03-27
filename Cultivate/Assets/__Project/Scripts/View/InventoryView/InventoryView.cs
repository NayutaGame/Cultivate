using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InventoryView<T> : MonoBehaviour, IIndexPath where T : ItemView
{
    public Transform Container;
    public GameObject Prefab;

    private List<T> _views;
    public List<T> Views => _views;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    public virtual void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;

        _views = new List<T>();

        RegisterExists();
    }

    private void RegisterExists()
    {
        for (int i = 0; i < Container.childCount; i++)
        {
            T v = Container.GetChild(i).GetComponent<T>();
            _views.Add(v);
            v.Configure(new IndexPath($"{_indexPath}#{i}"));
        }
    }

    public virtual void Refresh()
    {
        PopulateList();
        foreach(T view in _views) view.Refresh();
    }

    private void PopulateList()
    {
        int current = Container.childCount;
        IList inventory = RunManager.Get<IList>(_indexPath);
        int need = inventory.Count;

        (need, _) = Numeric.Negate(need, current);
        if (need <= 0) return;

        int length = Container.childCount;
        for (int i = length; i < need + length; i++)
        {
            T v = Instantiate(Prefab, Container).GetComponent<T>();
            _views.Add(v);
            v.Configure(new IndexPath($"{_indexPath}#{i}"));
        }
    }
}
