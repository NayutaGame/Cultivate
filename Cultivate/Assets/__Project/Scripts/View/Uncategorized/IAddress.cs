using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAddress
{
    Address GetAddress();
    T Get<T>();
    void SetAddress(Address address);
    void Refresh();
}
