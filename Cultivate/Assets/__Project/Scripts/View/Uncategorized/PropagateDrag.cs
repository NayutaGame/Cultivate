
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropagateDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<PointerEventData> _onBeginDrag;
    public Action<PointerEventData> _onDrag;
    public Action<PointerEventData> _onEndDrag;

    public void OnBeginDrag(PointerEventData eventData) => _onBeginDrag?.Invoke(eventData);
    public void OnDrag(PointerEventData eventData) => _onDrag?.Invoke(eventData);
    public void OnEndDrag(PointerEventData eventData) => _onEndDrag?.Invoke(eventData);
}
