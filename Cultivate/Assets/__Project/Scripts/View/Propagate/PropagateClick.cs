using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropagateClick : MonoBehaviour,
    IPointerClickHandler
{
    public Action<PointerEventData> _onPointerClick;

    public void OnPointerClick(PointerEventData eventData) => _onPointerClick?.Invoke(eventData);
}
