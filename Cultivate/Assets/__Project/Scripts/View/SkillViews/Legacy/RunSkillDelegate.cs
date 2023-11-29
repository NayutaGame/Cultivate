
using UnityEngine.EventSystems;

public class RunSkillDelegate : InteractDelegate,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler
{
    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        // _animationHandle?.Kill();
        // _animationHandle = ContentTransform.DOAnchorPos(HoverPivot.anchoredPosition, 0.15f);
        // _animationHandle.Restart();

        CanvasManager.Instance.SkillAnnotation.SetAddress(AddressDelegate.GetAddress());
        CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        // _animationHandle?.Kill();
        // _animationHandle = ContentTransform.DOAnchorPos(IdlePivot.anchoredPosition, 0.15f);
        // _animationHandle.Restart();

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
        // CanvasGroup.blocksRaycasts = false;
        // _animationHandle?.Kill();
        // FollowAnimationAnchored f = new FollowAnimationAnchored() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = MousePivot };
        // _animationHandle = f.GetHandle();
        // _animationHandle.Restart();

        // RunCanvas.Instance.SetIndexPathForSkillPreview(null);
        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);
    }

    public void EndDrag(PointerEventData eventData)
    {
        // CanvasGroup.blocksRaycasts = true;
        // _animationHandle?.Kill();
        // FollowAnimationAnchored f = new FollowAnimationAnchored() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = IdlePivot };
        // _animationHandle = f.GetHandle();
        // _animationHandle.Restart();

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();
    }

    public void Drag(PointerEventData eventData)
    {
        // MousePivot.position = eventData.position;
        // if (_animationHandle != null && _animationHandle.active)
        //     return;
        // ContentTransform.position = MousePivot.position;
    }
}
