
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandSkillView : SkillView, IInteractable
{
    [SerializeField] private RectTransform RectTransform;
    [NonSerialized] public HandPivot HandPivot;

    private Tween _animationHandle;

    private void OnEnable()
    {
        HandPivot.gameObject.SetActive(true);
        GoToPivot(HandPivot.IdlePivot);
    }

    private void OnDisable()
    {
        HandPivot.gameObject.SetActive(false);
    }

    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        AudioManager.Play("CardHover");

        GoToPivot(HandPivot.HoverPivot);

        CanvasManager.Instance.SkillPreview.SetAddress(GetAddress());
        CanvasManager.Instance.SkillPreview.Refresh();
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        GoToPivot(HandPivot.IdlePivot);

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
        CanvasManager.Instance.SkillPreview.SetAddress(null);
        CanvasManager.Instance.SkillPreview.Refresh();

        HandPivot.BlockRaycasts = false;

        GoToPivot(HandPivot.MousePivot);

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);
    }

    public void EndDrag(PointerEventData eventData)
    {
        HandPivot.BlockRaycasts = true;

        GoToPivot(HandPivot.IdlePivot);

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();
    }

    public void Drag(PointerEventData eventData)
    {
        HandPivot.MousePivot.position = eventData.position;
        if (_animationHandle != null && _animationHandle.active)
            return;
        RectTransform.position = HandPivot.MousePivot.position;
    }

    private void GoToPivot(RectTransform pivot)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(RectTransform, pivot);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }

    #region IInteractable

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate) => InteractDelegate = interactDelegate;

    #endregion
}
