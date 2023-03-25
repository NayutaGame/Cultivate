using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InventoryView<T> : MonoBehaviour where T : ItemView
{
    public Transform Container;
    public GameObject Prefab;

    private IInventory _inventory;
    private List<T> _views;

    private IndexPath _parentIndexPath;

    public virtual void Configure(IInventory inventory, IndexPath parentIndexPath = null)
    {
        _inventory = inventory;
        _views = new List<T>();

        _parentIndexPath = parentIndexPath;
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

            IndexPath indexPath;
            if (_parentIndexPath == null)
                indexPath = new IndexPath(_inventory.GetIndexPathString(), i);
            else
                indexPath = new IndexPath(_inventory.GetIndexPathString(), _parentIndexPath._ints, i);
            v.Configure(indexPath);
        }
    }
}
