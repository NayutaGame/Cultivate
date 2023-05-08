using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropagateDrag : MonoBehaviour, IIndexPath, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public IndexPath _indexPath;
    public Action<PointerEventData> _onPointerDown;
    public Action<PointerEventData> _onBeginDrag;
    public Action<PointerEventData> _onEndDrag;
    public Action<PointerEventData> _onDrag;

    public IndexPath GetIndexPath() => _indexPath;
    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
    }

    public void Refresh() { }

    public void OnPointerDown(PointerEventData eventData)
    {
        _onPointerDown?.Invoke(eventData);
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
