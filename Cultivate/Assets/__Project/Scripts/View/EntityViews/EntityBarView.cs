
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityBarView : EntityView, IPointerClickHandler
{
    [SerializeField] private Image SelectionImage;

    private bool _selected;
    public virtual bool IsSelected() => _selected;
    public virtual void SetSelected(bool selected)
    {
        _selected = selected;
        if (SelectionImage != null)
            SelectionImage.color = new Color(1, 1, 1, selected ? 1 : 0);
    }

    #region IInteractable

    // public virtual void OnPointerEnter(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_ENTER, this, eventData);
    // public virtual void OnPointerExit(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_EXIT, this, eventData);
    // public virtual void OnPointerMove(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_MOVE, this, eventData);
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        int? gestureId = null;

        if (eventData.button == PointerEventData.InputButton.Left) {
            gestureId = InteractDelegate.POINTER_LEFT_CLICK;
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            gestureId = InteractDelegate.POINTER_RIGHT_CLICK;
        }

        if (gestureId.HasValue)
            GetDelegate()?.Handle(gestureId.Value, this, eventData);
    }

    // public virtual void OnBeginDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.BEGIN_DRAG, this, eventData);
    // public virtual void OnEndDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.END_DRAG, this, eventData);
    // public virtual void OnDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.DRAG, this, eventData);
    // public virtual void OnDrop(PointerEventData eventData) => GetDelegate()?.DragDrop(eventData.pointerDrag.GetComponent<IInteractable>(), this);

    #endregion
}
