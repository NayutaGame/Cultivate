using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemView : MonoBehaviour, IAddress
{
    [SerializeField] public int PrefabIndex;

    private int _siblingIndex;
    public virtual int GetSiblingIndex() => _siblingIndex;
    public virtual void SetSiblingIndex(int value) => _siblingIndex = value;

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
