using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAddress
{
    Address GetAddress();
    T Get<T>();
    void SetAddress(Address address);
    void Refresh();

    // private Address _address;
    // public Address GetAddress() => _address;
    // public T Get<T>() => _address.Get<T>();
    //
    // public virtual void SetAddress(Address address)
    // {
    //     _address = address;
    // }
    //
    // public virtual void Refresh()
    // {
    // }
}
