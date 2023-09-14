using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class TechTreeView : MonoBehaviour, IAddress
{
    private static readonly int HEIGHT = 8;

    public Transform Container;
    public GameObject Prefab;

    private List<Transform> _holders;
    private List<TechView> _views;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    public virtual void Configure(Address address)
    {
        _address = address;
        InitHolders();
        _views = new ();
        PopulateList();
    }

    public virtual void Refresh()
    {
        foreach(TechView view in _views) view.Refresh();
    }

    private void InitHolders()
    {
        _holders = new();
        for (int x = 0; x < Container.childCount; x++)
        {
            Transform childTransform = Container.GetChild(x);
            if (childTransform.name != "VLayout") continue;

            for (int y = 0; y < childTransform.childCount; y++)
            {
                Transform grandchildTransform = childTransform.GetChild(y);

                _holders.Add(grandchildTransform);
            }
        }
    }

    private void PopulateList()
    {
        IList inventory = Get<IList>();
        for (int i = 0; i < inventory.Count; i++)
        {
            Address address = _address.Append("#{i}");
            RunTech tech = address.Get<RunTech>();
            Vector2Int position = tech.GetPosition();
            int index = position.x * HEIGHT + position.y;
            Transform parent = _holders[index];

            TechView v = Instantiate(Prefab, parent).GetComponent<TechView>();
            _views.Add(v);
            v.Configure(address);
        }
    }
}
