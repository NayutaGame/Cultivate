using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropagateDrop : MonoBehaviour,
    IDropHandler
{
    public Action<InteractBehaviour, MonoBehaviour, PointerEventData> _onDrop;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == gameObject)
            return;

        InteractBehaviour dragged = eventData.pointerDrag.GetComponent<InteractBehaviour>();

        if (dragged != null)
            _onDrop.Invoke(dragged, this, eventData);
    }
}
