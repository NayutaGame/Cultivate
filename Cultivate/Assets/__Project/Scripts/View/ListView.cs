
using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class ListView : MonoBehaviour, IAddress, IInteractable
{
    public Transform Container;
    public GameObject[] Prefabs;

    private List<ItemView> _activePool;
    public List<ItemView> ActivePool => _activePool;
    private List<ItemView>[] _inactivePools;

    private Func<object, int> _prefabProvider;
    public void SetPrefabProvider(Func<object, int> prefabProvider) => _prefabProvider = prefabProvider;
    private GameObject GetPrefab(object item) => Prefabs[_prefabProvider?.Invoke(item) ?? 0];
    private int GetPrefabIndex(object model) => _prefabProvider?.Invoke(model) ?? 0;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;

    public virtual void SetDelegate(InteractDelegate interactDelegate)
    {
        InteractDelegate = interactDelegate;
        _activePool.Do(v =>
        {
            if (v is IInteractable interactable) interactable.SetDelegate(InteractDelegate);
        });
        _inactivePools.Do(p => p.Do(v =>
        {
            if (v is IInteractable interactable) interactable.SetDelegate(InteractDelegate);
        }));
    }

    public virtual void SetAddress(Address address)
    {
        _address = address;
        _activePool = new List<ItemView>();
        _inactivePools = new List<ItemView>[Prefabs.Length];
        for (int i = 0; i < _inactivePools.Length; i++)
            _inactivePools[i] = new List<ItemView>();
        RegisterExists();
    }

    private void RegisterExists()
    {
        for (int i = 0; i < Container.childCount; i++)
        {
            ItemView itemView = Container.GetChild(i).GetComponent<ItemView>();
            itemView.gameObject.SetActive(false);

            BindItemView(itemView);
        }
    }

    public virtual void Refresh()
    {
        RefreshList();
        _activePool.Do(v => v.Refresh());
    }

    private void RefreshList()
    {
        PutAllIntoPool();

        IList list = Get<IList>();
        for (int i = 0; i < list.Count; i++)
        {
            Address address = _address.Append($"#{i}");
            int prefabIndex = GetPrefabIndex(address.Get<object>());
            FetchItemView(out ItemView itemView, prefabIndex);
            EnableItemView(itemView, i);
            itemView.SetAddress(address);
        }
    }

    private void PutAllIntoPool()
    {
        for (int i = 0; i < Container.childCount; i++)
            while (_activePool.Count != 0)
            {
                ItemView itemView = _activePool[0];
                _activePool.RemoveAt(0);

                DisableItemView(itemView);
            }
    }

    private bool FetchItemView(out ItemView itemView, int prefabIndex)
    {
        var pool = _inactivePools[prefabIndex];
        if (pool.Count != 0)
        {
            itemView = pool[0];
            pool.RemoveAt(0);
            return true;
        }

        GameObject prefab = Prefabs[prefabIndex];
        itemView = Instantiate(prefab, Container).GetComponent<ItemView>();
        BindItemView(itemView, prefabIndex);
        return false;
    }

    private void BindItemView(ItemView itemView, int prefabIndex = 0)
    {
        itemView.PrefabIndex = prefabIndex;
        if (itemView is IInteractable interactable) interactable.SetDelegate(InteractDelegate);
    }

    private void EnableItemView(ItemView itemView, int siblingIndex)
    {
        itemView.transform.SetSiblingIndex(siblingIndex);
        itemView.SetSiblingIndex(itemView.transform.GetSiblingIndex());
        _activePool.Add(itemView);
        itemView.gameObject.SetActive(true);
    }

    private void DisableItemView(ItemView itemView)
    {
        itemView.gameObject.SetActive(false);
        _inactivePools[itemView.PrefabIndex].Add(itemView);
        itemView.transform.SetAsLastSibling();
        itemView.SetSiblingIndex(itemView.transform.GetSiblingIndex());
    }
}
