
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropagateDrag : LegacyAddressBehaviour,
    IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Action<PointerEventData> _onPointerClick;
    public Action<PointerEventData> _onBeginDrag;
    public Action<PointerEventData> _onEndDrag;
    public Action<PointerEventData> _onDrag;

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
