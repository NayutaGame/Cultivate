
using System;
using UnityEngine.EventSystems;

public class PropagatePointerEnter : LegacyAddressBehaviour,
    IPointerEnterHandler
{
    public Action<PointerEventData> _onPointerEnter;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _onPointerEnter?.Invoke(eventData);
    }
}
