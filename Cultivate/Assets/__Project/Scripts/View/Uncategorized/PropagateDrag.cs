using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropagateDrag : MonoBehaviour, IIndexPath, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public IndexPath _indexPath;
    public Action<PointerEventData> _onPointerClick;
    public Action<PointerEventData> _onBeginDrag;
    public Action<PointerEventData> _onEndDrag;
    public Action<PointerEventData> _onDrag;

    public IndexPath GetIndexPath() => _indexPath;
    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;
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
