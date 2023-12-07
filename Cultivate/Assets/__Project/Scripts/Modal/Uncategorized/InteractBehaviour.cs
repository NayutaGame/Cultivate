
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InteractBehaviour : MonoBehaviour
    // IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    // IBeginDragHandler, IEndDragHandler, IDragHandler,
    // IDropHandler,
    // IPointerClickHandler
{
    public AddressBehaviour AddressBehaviour;
    public PivotBehaviour PivotBehaviour;

    public virtual bool IsEnabled() => enabled;
    public virtual void SetEnabled(bool value)
    {
        enabled = value;
        PivotBehaviour.gameObject.SetActive(value);

        // if (!value)
        //     PivotBehaviour.transform.SetAsLastSibling();
        // SetPivot(PivotBehaviour.IdlePivot);
        // SetPivotWithoutAnimation(PivotBehaviour.IdlePivot);
    }

    // [SerializeField] private Image Image;
    //
    // public bool RaycastTarget
    // {
    //     get => Image.raycastTarget;
    //     set => Image.raycastTarget = value;
    // }
    //
    // private void OnEnable()
    // {
    //     RaycastTarget = true;
    // }

    private InteractHandler _interactHandler;
    public InteractHandler GetHandler() => _interactHandler;
    public void SetHandler(InteractHandler interactHandler) => _interactHandler = interactHandler;

    public virtual void OnPointerEnter(PointerEventData eventData) => GetHandler()?.Handle(InteractHandler.POINTER_ENTER, this, eventData);
    public virtual void OnPointerExit(PointerEventData eventData)  => GetHandler()?.Handle(InteractHandler.POINTER_EXIT, this, eventData);
    public virtual void OnPointerMove(PointerEventData eventData)  => GetHandler()?.Handle(InteractHandler.POINTER_MOVE, this, eventData);
    public virtual void OnBeginDrag(PointerEventData eventData)    => GetHandler()?.Handle(InteractHandler.BEGIN_DRAG, this, eventData);
    public virtual void OnEndDrag(PointerEventData eventData)      => GetHandler()?.Handle(InteractHandler.END_DRAG, this, eventData);
    public virtual void OnDrag(PointerEventData eventData)         => GetHandler()?.Handle(InteractHandler.DRAG, this, eventData);
    public virtual void OnDrop(PointerEventData eventData)         => GetHandler()?.DragDrop(eventData.pointerDrag.GetComponent<InteractBehaviour>(), this);
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        int? gestureId = null;

        if (eventData.button == PointerEventData.InputButton.Left) {
            gestureId = InteractHandler.POINTER_LEFT_CLICK;
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            gestureId = InteractHandler.POINTER_RIGHT_CLICK;
        }

        if (gestureId.HasValue)
            GetHandler()?.Handle(gestureId.Value, this, eventData);
    }

    private Tween _animationHandle;

    public void SetPivot(RectTransform pivot)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(AddressBehaviour.RectTransform, pivot);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }

    public void SetPivotWithoutAnimation(RectTransform pivot)
    {
        _animationHandle?.Kill();
        AddressBehaviour.RectTransform.position = pivot.position;
        AddressBehaviour.RectTransform.localScale = pivot.localScale;
    }

    public void SetStartAndPivot(RectTransform start, RectTransform pivot)
    {
        SetPivotWithoutAnimation(start);
        SetPivot(pivot);
    }
}
