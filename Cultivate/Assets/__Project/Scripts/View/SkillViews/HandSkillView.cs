
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandSkillView : SkillView, IInteractable
{
    [SerializeField] private RectTransform RectTransform;
    [NonSerialized] public PivotPropagate PivotPropagate;

    private Tween _animationHandle;

    private void OnEnable()
    {
        PivotPropagate.RaycastTarget = true;
        PivotPropagate.transform.SetAsLastSibling();
        PivotPropagate.gameObject.SetActive(true);
        GoToPivot(PivotPropagate.IdlePivot);
    }

    private void OnDisable()
    {
        PivotPropagate.gameObject.SetActive(false);
    }

    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        AudioManager.Play("CardHover");

        // GoToPivot(PivotPropagate.HoverPivot);

        CanvasManager.Instance.SkillAnnotation.SetAddress(GetAddress());
        CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        // GoToPivot(PivotPropagate.IdlePivot);

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
        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
        CanvasManager.Instance.SkillAnnotation.Refresh();

        PivotPropagate.RaycastTarget = false;
        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.RefreshPivots();

        GoToPivot(PivotPropagate.MousePivot);

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);
    }

    public void EndDrag(PointerEventData eventData)
    {
        PivotPropagate.RaycastTarget = true;
        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.RefreshPivots();

        GoToPivot(PivotPropagate.IdlePivot);

        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateNormal();
    }

    public void Drag(PointerEventData eventData)
    {
        PivotPropagate.MousePivot.position = eventData.position;
        if (_animationHandle != null && _animationHandle.active)
            return;
        RectTransform.position = PivotPropagate.MousePivot.position;
    }

    public void GoToPivot(RectTransform pivot)
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
