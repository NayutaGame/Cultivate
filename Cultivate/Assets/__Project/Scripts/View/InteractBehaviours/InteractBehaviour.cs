
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InteractBehaviour : MonoBehaviour
    // IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    // IBeginDragHandler, IEndDragHandler, IDragHandler,
    // IDropHandler,
    // IPointerClickHandler
{
    public ComplexView ComplexView;

    public virtual void SetRaycastable(bool value) { }
    public virtual void SetOpaque(bool value) { }

    private void OnEnable()
    {
        SetRaycastable(true);
        SetOpaque(true);
    }

    private InteractHandler _interactHandler;
    public InteractHandler GetHandler() => _interactHandler;
    public void SetHandler(InteractHandler interactHandler) => _interactHandler = interactHandler;

    public event Action<PointerEventData> PointerEnterEvent;
    public event Action<PointerEventData> PointerExitEvent;
    public event Action<PointerEventData> PointerMoveEvent;
    public event Action<PointerEventData> BeginDragEvent;
    public event Action<PointerEventData> EndDragEvent;
    public event Action<PointerEventData> DragEvent;
    public event Action<PointerEventData> LeftClickEvent;
    public event Action<PointerEventData> RightClickEvent;
    public event Action<PointerEventData, InteractBehaviour> DropEvent;

    public virtual void OnPointerEnter(PointerEventData eventData) => PointerEnterEvent?.Invoke(eventData);
    public virtual void OnPointerExit(PointerEventData eventData) => PointerExitEvent?.Invoke(eventData);
    public virtual void OnPointerMove(PointerEventData eventData) => PointerMoveEvent?.Invoke(eventData);
    public virtual void OnBeginDrag(PointerEventData eventData) => BeginDragEvent?.Invoke(eventData);
    public virtual void OnEndDrag(PointerEventData eventData) => EndDragEvent?.Invoke(eventData);
    public virtual void OnDrag(PointerEventData eventData) => DragEvent?.Invoke(eventData);
    public virtual void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == gameObject)
            return;

        InteractBehaviour dragged = eventData.pointerDrag.GetComponent<InteractBehaviour>();

        if (dragged != null)
            DropEvent?.Invoke(eventData, dragged);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {
            LeftClickEvent?.Invoke(eventData);
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            RightClickEvent?.Invoke(eventData);
        }
    }
}
