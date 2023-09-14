using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreView : MonoBehaviour, IAddress
{
    private Address _address;
    public Address GetIndexPath() => _address;
    public T Get<T>() => _address.Get<T>();

    public void Configure(Address address)
    {
        _address = address;
    }

    public void Refresh() { }
}
