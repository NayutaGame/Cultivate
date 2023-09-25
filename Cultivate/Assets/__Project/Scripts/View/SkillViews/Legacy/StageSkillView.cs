
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageSkillView : SkillView, IInteractable,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler
{
    [SerializeField] protected RectTransform _rectTransform;

    public Tween GetExpandTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(_rectTransform.DOScale(1, 0.6f).SetEase(Ease.InOutQuad));
        return seq;
    }

    public Tween GetShrinkTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(_rectTransform.DOScale(0.5f, 0.6f).SetEase(Ease.InOutQuad));
        return seq;
    }

    // public override void OnPointerEnter(PointerEventData eventData)
    // {
    //     if (eventData.dragging) return;
    //     StageCanvas.Instance.SetIndexPathForPreview(GetAddress());
    //     StageManager.Instance.Pause();
    // }
    //
    // public override void OnPointerExit(PointerEventData eventData)
    // {
    //     if (eventData.dragging) return;
    //     StageCanvas.Instance.SetIndexPathForPreview(null);
    //     StageManager.Instance.Resume();
    // }
    //
    // public override void OnPointerMove(PointerEventData eventData)
    // {
    //     if (eventData.dragging) return;
    //     StageCanvas.Instance.UpdateMousePosForPreview(eventData.position);
    // }

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
