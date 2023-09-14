using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAddress
{
    Address GetIndexPath();
    T Get<T>();
    void Configure(Address address);
    void Refresh();
}
