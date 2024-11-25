using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropagateDrop : MonoBehaviour,
    IDropHandler
{
    public Action<LegacyInteractBehaviour, MonoBehaviour, PointerEventData> _onDrop;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == gameObject)
            return;

        LegacyInteractBehaviour dragged = eventData.pointerDrag.GetComponent<LegacyInteractBehaviour>();

        if (dragged != null)
            _onDrop.Invoke(dragged, this, eventData);
    }
}
