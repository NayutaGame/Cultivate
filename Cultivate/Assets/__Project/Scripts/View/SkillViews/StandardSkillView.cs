
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StandardSkillView : SkillView, IInteractable,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler
{
    [SerializeField] private Image _blackFill;

    private static readonly float IdleAlpha = 0;
    private static readonly float HoverAlpha = 0.2f;

    private Tween _animationHandle;

    private void OnEnable()
    {
        _blackFill.color = new Color(1, 1, 1, IdleAlpha);
    }

    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        AudioManager.Play("CardHover");

        SetBlackFillColor(HoverAlpha);

        CanvasManager.Instance.SkillAnnotation.SetAddress(GetAddress());
        CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        SetBlackFillColor(IdleAlpha);

        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
        CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    public void PointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.SkillAnnotation.UpdateMousePos(eventData.position);
    }

    public void SetBlackFillColor(float alpha)
    {
        _animationHandle?.Kill();
        _animationHandle = _blackFill.DOFade(alpha, 0.15f);
        _animationHandle.SetAutoKill().Restart();
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

    #endregion
}
