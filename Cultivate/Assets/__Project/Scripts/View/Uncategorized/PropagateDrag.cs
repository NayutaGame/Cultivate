using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropagateDrag : MonoBehaviour, IAddress, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Address _address;
    public Address GetIndexPath() => _address;
    public T Get<T>() => _address.Get<T>();

    public Action<PointerEventData> _onPointerClick;
    public Action<PointerEventData> _onBeginDrag;
    public Action<PointerEventData> _onEndDrag;
    public Action<PointerEventData> _onDrag;

    public void Configure(Address address)
    {
        _address = address;
    }

    public void Refresh() { }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onPointerClick?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _onBeginDrag?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _onEndDrag?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _onDrag?.Invoke(eventData);
    }
}
