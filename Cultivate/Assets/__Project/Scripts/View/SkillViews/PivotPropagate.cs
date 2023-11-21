
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PivotPropagate : MonoBehaviour, IInteractable,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler,
    IPointerClickHandler
{
    [SerializeField] private Image Image;

    public RectTransform IdlePivot;
    public RectTransform HoverPivot;
    public RectTransform MousePivot;

    public IInteractable BindingView;

    public Address GetAddress() => BindingView.GetAddress();
    public T Get<T>() => BindingView.Get<T>();
    public void Refresh() { }

    public bool RaycastTarget
    {
        get => Image.raycastTarget;
        set => Image.raycastTarget = value;
    }

    #region IInteractable

    public InteractDelegate GetDelegate() => BindingView.GetDelegate();
    public void SetDelegate(InteractDelegate interactDelegate) => BindingView.SetDelegate(interactDelegate);

    public virtual void OnPointerEnter(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_ENTER, BindingView, eventData);
    public virtual void OnPointerExit(PointerEventData eventData)  => GetDelegate()?.Handle(InteractDelegate.POINTER_EXIT, BindingView, eventData);
    public virtual void OnPointerMove(PointerEventData eventData)  => GetDelegate()?.Handle(InteractDelegate.POINTER_MOVE, BindingView, eventData);
    public virtual void OnBeginDrag(PointerEventData eventData)    => GetDelegate()?.Handle(InteractDelegate.BEGIN_DRAG, BindingView, eventData);
    public virtual void OnEndDrag(PointerEventData eventData)      => GetDelegate()?.Handle(InteractDelegate.END_DRAG, BindingView, eventData);
    public virtual void OnDrag(PointerEventData eventData)         => GetDelegate()?.Handle(InteractDelegate.DRAG, BindingView, eventData);
    public virtual void OnDrop(PointerEventData eventData)         => GetDelegate()?.DragDrop(eventData.pointerDrag.GetComponent<IInteractable>(), BindingView);
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        int? gestureId = null;

        if (eventData.button == PointerEventData.InputButton.Left) {
            gestureId = InteractDelegate.POINTER_LEFT_CLICK;
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            gestureId = InteractDelegate.POINTER_RIGHT_CLICK;
        }

        if (gestureId.HasValue)
            GetDelegate()?.Handle(gestureId.Value, BindingView, eventData);
    }

    #endregion
}
