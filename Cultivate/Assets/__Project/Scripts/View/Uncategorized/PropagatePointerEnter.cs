
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropagatePointerEnter : MonoBehaviour, IAddress, IPointerEnterHandler
{
    public Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    public Action<PointerEventData> _onPointerEnter;

    public void SetAddress(Address address)
    {
        _address = address;
    }

    public void Refresh() { }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _onPointerEnter?.Invoke(eventData);
    }
}
