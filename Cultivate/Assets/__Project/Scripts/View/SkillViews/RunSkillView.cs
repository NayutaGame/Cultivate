
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunSkillView : AbstractSkillView
{
    [SerializeField] private RectTransform ContentTransform;

    [SerializeField] private RectTransform IdlePivot;
    [SerializeField] private RectTransform HoverPivot;
    [SerializeField] private RectTransform MousePivot;

    private Tweener _animationHandle;

    private void OnEnable()
    {
        ContentTransform.anchoredPosition = IdlePivot.anchoredPosition;
        _canvasGroup.blocksRaycasts = true;
    }

    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        _animationHandle?.Kill();
        _animationHandle = ContentTransform.DOAnchorPos(HoverPivot.anchoredPosition, 0.15f);
        _animationHandle.Restart();

        // RunCanvas.Instance.SetIndexPathForSkillPreview(GetAddress());
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        _animationHandle?.Kill();
        _animationHandle = ContentTransform.DOAnchorPos(IdlePivot.anchoredPosition, 0.15f);
        _animationHandle.Restart();

        // RunCanvas.Instance.SetIndexPathForSkillPreview(null);
    }

    public void BeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = MousePivot };
        _animationHandle = f.GetHandle();
        _animationHandle.Restart();
        // IInteractable drag = eventData.pointerDrag.GetComponent<IInteractable>();
        // if(drag == null || drag.GetDelegate() == null || !drag.GetDelegate().CanDrag(drag))
        // {
        //     eventData.pointerDrag = null;
        //     RunCanvas.Instance.SetIndexPathForSkillPreview(null);
        //     return;
        // }

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);

        // RunCanvas.Instance.SkillGhost.SetAddress(GetAddress());
        // RunCanvas.Instance.SkillGhost.Refresh();
        //
        // if (_image != null)
        //     _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 0.5f);
        //
        // RunCanvas.Instance.SetIndexPathForSkillPreview(null);
    }

    public void EndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = IdlePivot };
        _animationHandle = f.GetHandle();
        _animationHandle.Restart();
    }

    public void Drag(PointerEventData eventData)
    {
        MousePivot.position = eventData.position;
        if (_animationHandle != null && _animationHandle.active)
            return;
        ContentTransform.position = MousePivot.position;
    }

    // {
    //     // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();
    //
    //     RunCanvas.Instance.SkillGhost.SetAddress(null);
    //     RunCanvas.Instance.SkillGhost.Refresh();
    //     RunCanvas.Instance.Refresh();
    //
    //     if (_image != null)
    //         _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a * 2f);
    //
    //     RunCanvas.Instance.Refresh();
    // }

    // {
    //     RunCanvas.Instance.SkillGhost.UpdateMousePos(eventData.position);
    // }
}
