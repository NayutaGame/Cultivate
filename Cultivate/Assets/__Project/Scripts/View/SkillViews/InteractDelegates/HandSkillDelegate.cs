
using UnityEngine.EventSystems;

public class HandSkillDelegate : InteractDelegate
{
    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        AudioManager.Play("CardHover");

        PlayFollowAnimation(PivotDelegate.HoverPivot);

        CanvasManager.Instance.SkillAnnotation.SetAddress(AddressDelegate.GetAddress());
        // CanvasManager.Instance.SkillAnnotation.SetAddress(SkillView.GetAddress());
        CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        PlayFollowAnimation(PivotDelegate.IdlePivot);

        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
        CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    public void PointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.SkillAnnotation.UpdateMousePos(eventData.position);
    }

    public void BeginDrag(PointerEventData eventData)
    {
        eventData.pointerDrag = gameObject;

        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
        CanvasManager.Instance.SkillAnnotation.Refresh();

        ItemView.CanvasGroup.alpha = 0;

        CanvasManager.Instance.SkillGhost.RectTransform.position = SkillView.RectTransform.position;

        CanvasManager.Instance.SkillGhost.CanvasGroup.alpha = 1;
        CanvasManager.Instance.SkillGhost.SetAddress(AddressDelegate.GetAddress());
        CanvasManager.Instance.SkillGhost.Refresh();
        CanvasManager.Instance.SkillGhost.GoToPivot(PivotDelegate.FollowPivot);

        PivotDelegate.RaycastTarget = false;

        // SkillSlot slot = Get<SkillSlot>();
        // if (slot.Skill == null)
        // {
        //     eventData.pointerDrag = null;
        //     return;
        // }
        //
        // eventData.pointerDrag = gameObject;
        //
        // CanvasManager.Instance.SkillAnnotation.SetAddress(null);
        // CanvasManager.Instance.SkillAnnotation.Refresh();
        //
        // SkillView.CanvasGroup.alpha = 0;
        // SkillView.CanvasGroup.blocksRaycasts = false;
        //
        // CanvasManager.Instance.SlotGhost.CanvasGroup.alpha = 1;
        // CanvasManager.Instance.SlotGhost.RectTransform.position = SkillView.RectTransform.position;
        // CanvasManager.Instance.SlotGhost.SetAddress(GetAddress());
        // CanvasManager.Instance.SlotGhost.Refresh();
        // CanvasManager.Instance.SlotGhost.GoToPivot(PivotDelegate.FollowPivot);
        //
        // // _animationHandle?.Kill();
        // // FollowAnimationAnchored f = new FollowAnimationAnchored() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = MousePivot };
        // // _animationHandle = f.GetHandle();
        // // _animationHandle.Restart();
    }

    public void EndDrag(PointerEventData eventData)
    {
        ItemView.CanvasGroup.alpha = 1;
        ItemView.RectTransform.position = CanvasManager.Instance.SkillGhost.RectTransform.position;

        CanvasManager.Instance.SkillGhost.CanvasGroup.alpha = 0;
        CanvasManager.Instance.SkillGhost.SetAddress(null);
        CanvasManager.Instance.SkillGhost.Refresh();

        PlayFollowAnimation(PivotDelegate.IdlePivot);

        PivotDelegate.RaycastTarget = true;

        // // _animationHandle?.Kill();
        // // FollowAnimationAnchored f = new FollowAnimationAnchored() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = IdlePivot };
        // // _animationHandle = f.GetHandle();
        // // _animationHandle.Restart();
        //
        // SkillView.CanvasGroup.alpha = 1;
        // SkillView.CanvasGroup.blocksRaycasts = true;
        //
        // SkillView.RectTransform.position = CanvasManager.Instance.SlotGhost.RectTransform.position;
        // GoToPivot(PivotDelegate.IdlePivot);
        //
        // CanvasManager.Instance.SlotGhost.CanvasGroup.alpha = 0;
        // CanvasManager.Instance.SlotGhost.SetAddress(null);
        // CanvasManager.Instance.SlotGhost.Refresh();
    }

    public void Drag(PointerEventData eventData)
    {
        PivotDelegate.FollowPivot.position = eventData.position;
        if (CanvasManager.Instance.SkillGhost.IsAnimating)
            return;
        CanvasManager.Instance.SkillGhost.RectTransform.position = PivotDelegate.FollowPivot.position;

        // PivotDelegate.FollowPivot.position = eventData.position;
        // if (_animationHandle != null && _animationHandle.active)
        //     return;
        // SkillView.RectTransform.position = PivotDelegate.FollowPivot.position;
        //
        // PivotDelegate.FollowPivot.position = eventData.position;
        // if (CanvasManager.Instance.SlotGhost.IsAnimating)
        //     return;
        // CanvasManager.Instance.SkillGhost.RectTransform.position = PivotDelegate.FollowPivot.position;
    }
}
