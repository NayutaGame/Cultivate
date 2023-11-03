
using UnityEngine;
using UnityEngine.EventSystems;

public class HandPivot : MonoBehaviour, IInteractable,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler
{
    [SerializeField] private CanvasGroup CanvasGroup;

    public RectTransform IdlePivot;
    public RectTransform HoverPivot;
    public RectTransform MousePivot;

    public IInteractable BindingView;

    public bool BlockRaycasts
    {
        get => CanvasGroup.blocksRaycasts;
        set => CanvasGroup.blocksRaycasts = value;
    }

    #region IInteractable

    public InteractDelegate GetDelegate() => BindingView.GetDelegate();
    public void SetDelegate(InteractDelegate interactDelegate) { }
    public Address GetAddress() => BindingView.GetAddress();
    public T Get<T>() => BindingView.Get<T>();
    public void Refresh() { }

    public virtual void OnPointerEnter(PointerEventData eventData) => BindingView.GetDelegate()?.Handle(InteractDelegate.POINTER_ENTER, BindingView, eventData);
    public virtual void OnPointerExit(PointerEventData eventData) => BindingView.GetDelegate()?.Handle(InteractDelegate.POINTER_EXIT, BindingView, eventData);
    public virtual void OnPointerMove(PointerEventData eventData) => BindingView.GetDelegate()?.Handle(InteractDelegate.POINTER_MOVE, BindingView, eventData);
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        int? gestureId = null;

        if (eventData.button == PointerEventData.InputButton.Left) {
            gestureId = InteractDelegate.POINTER_LEFT_CLICK;
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            gestureId = InteractDelegate.POINTER_RIGHT_CLICK;
        }

        if (gestureId.HasValue)
            BindingView.GetDelegate()?.Handle(gestureId.Value, BindingView, eventData);
    }

    public virtual void OnBeginDrag(PointerEventData eventData) => BindingView.GetDelegate()?.Handle(InteractDelegate.BEGIN_DRAG, BindingView, eventData);
    public virtual void OnEndDrag(PointerEventData eventData) => BindingView.GetDelegate()?.Handle(InteractDelegate.END_DRAG, BindingView, eventData);
    public virtual void OnDrag(PointerEventData eventData) => BindingView.GetDelegate()?.Handle(InteractDelegate.DRAG, BindingView, eventData);
    public virtual void OnDrop(PointerEventData eventData) => BindingView.GetDelegate()?.DragDrop(eventData.pointerDrag.GetComponent<IInteractable>(), BindingView);

    #endregion
}
