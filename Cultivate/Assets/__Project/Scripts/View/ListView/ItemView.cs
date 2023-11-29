
using System;
using UnityEngine;

public class ItemView : MonoBehaviour, IAddress
{
    [SerializeField] public RectTransform RectTransform;
    [NonSerialized] public int PrefabIndex;

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    public virtual void SetAddress(Address address)
    {
        _address = address;
    }

    public virtual void Refresh()
    {
    }
}
