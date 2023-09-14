
using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ListView : MonoBehaviour, IAddress, IInteractable
{
    public Transform Container;
    public GameObject[] Prefabs;

    private Func<object, int> _prefabProvider;
    public void SetPrefabProvider(Func<object, int> prefabProvider)
        => _prefabProvider = prefabProvider;

    private GameObject GetPrefab(object item)
        => Prefabs[_prefabProvider?.Invoke(item) ?? 0];

    private List<IAddress> _views;
    public List<IAddress> Views => _views;

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
        _views = new List<IAddress>();
        RegisterExists();
    }

    public virtual void Refresh()
    {
        PopulateList();
        foreach(IAddress view in _views) view.Refresh();
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
            IAddress view = Instantiate(prefab, Container).GetComponent<IAddress>();
            RegisterNew(view, i);
        }
    }

    private void RegisterExists()
    {
        for (int i = 0; i < Container.childCount; i++)
        {
            IAddress view = Container.GetChild(i).GetComponent<IAddress>();
            RegisterNew(view, i);
        }
    }

    private void RegisterNew(IAddress view, int i)
    {
        if (!_views.Contains(view))
            _views.Add(view);

        view.Configure(_address.Append($"#{i}"));

        if (view is IInteractable interactable)
            interactable.SetDelegate(InteractDelegate);
    }
}
