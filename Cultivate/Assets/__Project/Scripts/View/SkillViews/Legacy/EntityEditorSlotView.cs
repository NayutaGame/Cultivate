
using UnityEngine;

public class EntityEditorSlotView : LegacyAddressBehaviour
{
    [SerializeField] protected SkillView SkillView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        SkillView.SetAddress(GetAddress().Append(".Skill"));

        // SkillView.ClearIsManaShortage();
        // SkillView.IsManaShortageDelegate += IsManaShortage;
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillSlot slot = Get<SkillSlot>();

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

    // private void OnEnable()
    // {
    //     ContentTransform.anchoredPosition = IdlePivot.anchoredPosition;
    //     if (CanvasGroup != null)
    //         CanvasGroup.blocksRaycasts = true;
    // }

    // private bool IsManaShortage()
    // {
    //     SkillSlot slot = Get<SkillSlot>();
    //     return slot.IsManaShortage;
    // }

    // [SerializeField] private RectTransform ContentTransform;
    //
    // [SerializeField] private RectTransform IdlePivot;
    // [SerializeField] private RectTransform HoverPivot;
    // [SerializeField] private RectTransform MousePivot;
    //
    // private Tween _animationHandle;
    //
    // public void HoverAnimation(PointerEventData eventData)
    // {
    //     if (eventData.dragging) return;
    //
    //     _animationHandle?.Kill();
    //     _animationHandle = DOTween.Sequence()
    //         .Append(ContentTransform.DOAnchorPos(HoverPivot.anchoredPosition, 0.15f))
    //         .Append(ContentTransform.DOScale(HoverPivot.localScale, 0.15f));
    //     _animationHandle.Restart();
    //
    //     RunCanvas.Instance.SkillPreview.SetAddress(SkillView.GetAddress());
    //     RunCanvas.Instance.SkillPreview.Refresh();
    // }
    //
    // public void UnhoverAnimation(PointerEventData eventData)
    // {
    //     if (eventData.dragging) return;
    //
    //     _animationHandle?.Kill();
    //     _animationHandle = DOTween.Sequence()
    //         .Append(ContentTransform.DOAnchorPos(IdlePivot.anchoredPosition, 0.15f))
    //         .Append(ContentTransform.DOScale(IdlePivot.localScale, 0.15f));
    //     _animationHandle.Restart();
    //
    //     RunCanvas.Instance.SkillPreview.SetAddress(null);
    //     RunCanvas.Instance.SkillPreview.Refresh();
    // }
    //
    // public void PointerMove(PointerEventData eventData)
    // {
    //     if (eventData.dragging) return;
    //
    //     RunCanvas.Instance.SkillPreview.UpdateMousePos(eventData.position);
    // }
    //
    // public void BeginDrag(PointerEventData eventData)
    // {
    //     CanvasGroup.blocksRaycasts = false;
    //     _animationHandle?.Kill();
    //     FollowAnimation f = new FollowAnimation() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = MousePivot };
    //     _animationHandle = f.GetHandle();
    //     _animationHandle.Restart();
    //
    //     // RunCanvas.Instance.SetIndexPathForSkillPreview(null);
    //     // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);
    // }
    //
    // public void EndDrag(PointerEventData eventData)
    // {
    //     CanvasGroup.blocksRaycasts = true;
    //     _animationHandle?.Kill();
    //     FollowAnimation f = new FollowAnimation() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = IdlePivot };
    //     _animationHandle = f.GetHandle();
    //     _animationHandle.Restart();
    //
    //     // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();
    // }
    //
    // public void Drag(PointerEventData eventData)
    // {
    //     MousePivot.position = eventData.position;
    //     if (_animationHandle != null && _animationHandle.active)
    //         return;
    //     ContentTransform.position = MousePivot.position;
    // }
}
