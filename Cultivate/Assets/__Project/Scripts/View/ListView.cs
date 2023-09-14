
using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public abstract class ListView<T> : MonoBehaviour, IAddress, IInteractable where T : IAddress
{
    public Transform Container;
    public GameObject[] Prefabs;

    private Func<object, int> _prefabProvider;
    public void SetPrefabProvider(Func<object, int> prefabProvider)
        => _prefabProvider = prefabProvider;

    private GameObject GetPrefab(object item)
        => Prefabs[_prefabProvider?.Invoke(item) ?? 0];

    private List<T> _views;
    public List<T> Views => _views;

    private Address _address;
    public Address GetAddress() => _address;
    public T1 Get<T1>() => _address.Get<T1>();

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public virtual void SetDelegate(InteractDelegate interactDelegate)
    {
        InteractDelegate = interactDelegate;

        _views.Do(
            v => {
                if (v is IInteractable interactable)
                    interactable.SetDelegate(InteractDelegate);
            });
    }

    public virtual void Configure(Address address)
    {
        _address = address;
        _views = new List<T>();
        RegisterExists();
    }

    public virtual void Refresh()
    {
        PopulateList();
        foreach(T view in _views) view.Refresh();
    }

    private void PopulateList()
    {
        int current = Container.childCount;
        IList inventory = Get<IList>();
        int need = inventory.Count;

        (need, _) = Numeric.Negate(need, current);
        if (need <= 0) return;

        int length = Container.childCount;
        for (int i = length; i < need + length; i++)
        {
            GameObject prefab = GetPrefab(inventory[i]);
            T view = Instantiate(prefab, Container).GetComponent<T>();
            RegisterNew(view, i);
        }
    }

    private void RegisterExists()
    {
        for (int i = 0; i < Container.childCount; i++)
        {
            T view = Container.GetChild(i).GetComponent<T>();
            RegisterNew(view, i);
        }
    }

    private void RegisterNew(T view, int i)
    {
        if (!_views.Contains(view))
            _views.Add(view);

        view.Configure(_address.Append($"#{i}"));

        if (view is IInteractable interactable)
            interactable.SetDelegate(InteractDelegate);
    }
}
