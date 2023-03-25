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

    private IInventory _inventory;
    private List<T> _views;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath() => _indexPath;

    public virtual void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;

        _inventory = RunManager.Get<IInventory>(_indexPath);
        _views = new List<T>();
    }

    public virtual void Refresh()
    {
        PopulateList();
        foreach(T view in _views) view.Refresh();
    }

    private void PopulateList()
    {
        int current = Container.childCount;
        int need = _inventory.GetCount();

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
