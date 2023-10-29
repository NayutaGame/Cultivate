
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunSkillView : SkillView, IInteractable,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler
{
    [SerializeField] protected CanvasGroup CanvasGroup;

    [SerializeField] private RectTransform ContentTransform;

    [SerializeField] private RectTransform IdlePivot;
    [SerializeField] private RectTransform HoverPivot;
    [SerializeField] private RectTransform MousePivot;

    private Tween _animationHandle;

    private void OnEnable()
    {
        ContentTransform.anchoredPosition = IdlePivot.anchoredPosition;
        if (CanvasGroup != null)
            CanvasGroup.blocksRaycasts = true;
    }

    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        _animationHandle?.Kill();
        _animationHandle = ContentTransform.DOAnchorPos(HoverPivot.anchoredPosition, 0.15f);
        _animationHandle.Restart();

        CanvasManager.Instance.SkillPreview.SetAddress(GetAddress());
        CanvasManager.Instance.SkillPreview.Refresh();
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        _animationHandle?.Kill();
        _animationHandle = ContentTransform.DOAnchorPos(IdlePivot.anchoredPosition, 0.15f);
        _animationHandle.Restart();

        CanvasManager.Instance.SkillPreview.SetAddress(null);
        CanvasManager.Instance.SkillPreview.Refresh();
    }

    public void PointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.SkillPreview.UpdateMousePos(eventData.position);
    }

    public void BeginDrag(PointerEventData eventData)
    {
        CanvasGroup.blocksRaycasts = false;
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = MousePivot };
        _animationHandle = f.GetHandle();
        _animationHandle.Restart();

        // RunCanvas.Instance.SetIndexPathForSkillPreview(null);
        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);
    }

    public void EndDrag(PointerEventData eventData)
    {
        CanvasGroup.blocksRaycasts = true;
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = IdlePivot };
        _animationHandle = f.GetHandle();
        _animationHandle.Restart();

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();
    }

    public void Drag(PointerEventData eventData)
    {
        MousePivot.position = eventData.position;
        if (_animationHandle != null && _animationHandle.active)
            return;
        ContentTransform.position = MousePivot.position;
    }

    #region IInteractable

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate) => InteractDelegate = interactDelegate;

    public virtual void OnPointerEnter(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_ENTER, this, eventData);
    public virtual void OnPointerExit(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_EXIT, this, eventData);
    public virtual void OnPointerMove(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.POINTER_MOVE, this, eventData);
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

    public virtual void OnBeginDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.BEGIN_DRAG, this, eventData);
    public virtual void OnEndDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.END_DRAG, this, eventData);
    public virtual void OnDrag(PointerEventData eventData) => GetDelegate()?.Handle(InteractDelegate.DRAG, this, eventData);
    public virtual void OnDrop(PointerEventData eventData) => GetDelegate()?.DragDrop(eventData.pointerDrag.GetComponent<IInteractable>(), this);

    #endregion
}
