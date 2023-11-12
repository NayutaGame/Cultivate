using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropagatePointer : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public Action<PointerEventData> _onPointerEnter;
    public Action<PointerEventData> _onPointerExit;
    public Action<PointerEventData> _onPointerMove;

    public void OnPointerEnter(PointerEventData eventData) => _onPointerEnter?.Invoke(eventData);
    public void OnPointerExit(PointerEventData eventData) => _onPointerExit?.Invoke(eventData);
    public void OnPointerMove(PointerEventData eventData) => _onPointerMove?.Invoke(eventData);
}
