
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropagatePointerEnter : MonoBehaviour,
    IPointerEnterHandler
{
    public Action<PointerEventData> _onPointerEnter;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _onPointerEnter?.Invoke(eventData);
    }
}
