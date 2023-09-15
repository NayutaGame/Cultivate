
using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class ListView : MonoBehaviour, IAddress, IInteractable
{
    public Transform Container;
    public GameObject[] Prefabs;

    private List<ItemView> _views;
    public List<ItemView> Views => _views;

    private Func<object, int> _prefabProvider;
    public void SetPrefabProvider(Func<object, int> prefabProvider) => _prefabProvider = prefabProvider;
    private GameObject GetPrefab(object item) => Prefabs[_prefabProvider?.Invoke(item) ?? 0];

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public virtual void SetDelegate(InteractDelegate interactDelegate)
    {
        InteractDelegate = interactDelegate;
        _views.Do(v =>
        {
            if (v is IInteractable interactable) interactable.SetDelegate(InteractDelegate);
        });
    }

    public virtual void SetAddress(Address address)
    {
        _address = address;
        _views = new List<ItemView>();
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
            ItemView itemView = Instantiate(prefab, Container).GetComponent<ItemView>();
            RegisterNew(itemView, i);
        }
    }

    private void RegisterExists()
    {
        for (int i = 0; i < Container.childCount; i++)
        {
            ItemView itemView = Container.GetChild(i).GetComponent<ItemView>();
            RegisterNew(itemView, i);
        }
    }

    private void RegisterNew(ItemView itemView, int i)
    {
        if (!_views.Contains(itemView)) _views.Add(itemView);
        itemView.SetAddress(_address.Append($"#{i}"));
        if (itemView is IInteractable interactable) interactable.SetDelegate(InteractDelegate);
    }
}
