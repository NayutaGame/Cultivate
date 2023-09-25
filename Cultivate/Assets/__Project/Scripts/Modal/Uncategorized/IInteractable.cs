using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
    // , IInteractable,
    // IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    // IPointerClickHandler,
    // IBeginDragHandler, IEndDragHandler, IDragHandler,
    // IDropHandler
{
    InteractDelegate GetDelegate();
    void SetDelegate(InteractDelegate interactDelegate);

    Address GetAddress();
    T Get<T>();
    void Refresh();

    // #region IInteractable
    //
    // private InteractDelegate InteractDelegate;
    // public InteractDelegate GetDelegate() => InteractDelegate;
    // public void SetDelegate(InteractDelegate interactDelegate) => InteractDelegate = interactDelegate;
    //
    // public virtual void OnPointerEnter(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_ENTER, this, eventData);
    // public virtual void OnPointerExit(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_EXIT, this, eventData);
    // public virtual void OnPointerMove(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_MOVE, this, eventData);
    // public virtual void OnPointerClick(PointerEventData eventData)
    // {
    //     int? gestureId = null;
    //
    //     if (eventData.button == PointerEventData.InputButton.Left) {
    //         gestureId = InteractDelegate.POINTER_LEFT_CLICK;
    //     } else if (eventData.button == PointerEventData.InputButton.Right) {
    //         gestureId = InteractDelegate.POINTER_RIGHT_CLICK;
    //     }
    //
    //     if (gestureId.HasValue)
    //         GetDelegate()?.Handle(gestureId.Value, this, eventData);
    // }
    //
    // public virtual void OnBeginDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.BEGIN_DRAG, this, eventData);
    // public virtual void OnEndDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.END_DRAG, this, eventData);
    // public virtual void OnDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.DRAG, this, eventData);
    // public virtual void OnDrop(PointerEventData eventData) => GetDelegate()?.DragDrop(eventData.pointerDrag.GetComponent<IInteractable>(), this);
    //
    // #endregion
}
