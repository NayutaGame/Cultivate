
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldSlotView : SlotView, IInteractable,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler
{
    [SerializeField] private Image SlotBackground;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SkillView.SetAddress(GetAddress().Append(".Skill"));
    }

    public override void Refresh()
    {
        SkillSlot slot = Get<SkillSlot>();

        // int locked = slot.State == SkillSlot.SkillSlotState.Locked ? 1 : 0;
        // SlotBackground.sprite = CanvasManager.Instance.SlotBackgrounds[locked];

        bool locked = slot.State == SkillSlot.SkillSlotState.Locked;
        gameObject.SetActive(!locked);
        if (locked)
            return;

        bool occupied = slot.State == SkillSlot.SkillSlotState.Occupied;
        SkillView.gameObject.SetActive(occupied);
        if (!occupied)
            return;

        SkillView.Refresh();
        SkillView.SetManaCostState(slot.ManaIndicator.State);
    }

    private void OnEnable()
    {
        ContentTransform.anchoredPosition = IdlePivot.anchoredPosition;
        CanvasGroup.blocksRaycasts = true;
    }

    [SerializeField] private CanvasGroup CanvasGroup;

    [SerializeField] private RectTransform ContentTransform;

    [SerializeField] private RectTransform IdlePivot;
    [SerializeField] private RectTransform HoverPivot;
    [SerializeField] private RectTransform MousePivot;

    private Tween _animationHandle;

    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        _animationHandle?.Kill();
        _animationHandle = DOTween.Sequence()
            .Append(ContentTransform.DOAnchorPos(HoverPivot.anchoredPosition, 0.15f))
            .Join(ContentTransform.DOScale(HoverPivot.localScale, 0.15f).SetEase(Ease.OutQuad));
        _animationHandle.Restart();

        CanvasManager.Instance.SkillPreview.SetAddress(SkillView.GetAddress());
        CanvasManager.Instance.SkillPreview.Refresh();
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        _animationHandle?.Kill();
        _animationHandle = DOTween.Sequence()
            .Append(ContentTransform.DOAnchorPos(IdlePivot.anchoredPosition, 0.15f))
            .Join(ContentTransform.DOScale(IdlePivot.localScale, 0.15f).SetEase(Ease.InQuad));
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
        SkillSlot slot = Get<SkillSlot>();
        if (slot.Skill == null)
        {
            eventData.pointerDrag = null;
            return;
        }

        CanvasManager.Instance.SkillPreview.SetAddress(null);
        CanvasManager.Instance.SkillPreview.Refresh();

        CanvasGroup.blocksRaycasts = false;
        _animationHandle?.Kill();
        FollowAnimationAnchored f = new FollowAnimationAnchored() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = MousePivot };
        _animationHandle = f.GetHandle();
        _animationHandle.Restart();

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);
    }

    public void EndDrag(PointerEventData eventData)
    {
        CanvasGroup.blocksRaycasts = true;
        _animationHandle?.Kill();
        FollowAnimationAnchored f = new FollowAnimationAnchored() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = IdlePivot };
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
