
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunSkillView : AbstractSkillView
{
    [SerializeField] private RectTransform ContentTransform;

    [SerializeField] private RectTransform IdlePivot;
    [SerializeField] private RectTransform HoverPivot;
    [SerializeField] private RectTransform MousePivot;

    private Tween _animationHandle;

    private void OnEnable()
    {
        ContentTransform.anchoredPosition = IdlePivot.anchoredPosition;
        if (_canvasGroup != null)
            _canvasGroup.blocksRaycasts = true;
    }

    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        _animationHandle?.Kill();
        _animationHandle = ContentTransform.DOAnchorPos(HoverPivot.anchoredPosition, 0.15f);
        _animationHandle.Restart();

        RunCanvas.Instance.SkillPreview.SetAddress(GetAddress());
        RunCanvas.Instance.SkillPreview.Refresh();
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        _animationHandle?.Kill();
        _animationHandle = ContentTransform.DOAnchorPos(IdlePivot.anchoredPosition, 0.15f);
        _animationHandle.Restart();

        RunCanvas.Instance.SkillPreview.SetAddress(null);
        RunCanvas.Instance.SkillPreview.Refresh();
    }

    public void PointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        RunCanvas.Instance.SkillPreview.UpdateMousePos(eventData.position);
    }

    public void BeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation() { Obj = ContentTransform, StartPosition = ContentTransform.anchoredPosition, Follow = MousePivot };
        _animationHandle = f.GetHandle();
        _animationHandle.Restart();

        // RunCanvas.Instance.SetIndexPathForSkillPreview(null);
        // RunCanvas.Instance.CharacterPanel._state = new CharacterPanelStateDragRunChip(this);
    }

    public void EndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
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
}
