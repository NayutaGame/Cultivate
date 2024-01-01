
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StandardSkillInteractBehaviour : InteractBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler
{
    private static readonly float IdleAlpha = 0;
    private static readonly float HoverAlpha = 0.2f;

    [SerializeField] private Image _blackFill;

    private Tween _animationHandle;

    private void OnEnable()
    {
        _blackFill.color = new Color(1, 1, 1, IdleAlpha);
    }

    public void SetBlackFillColor(float alpha)
    {
        _animationHandle?.Kill();
        _animationHandle = _blackFill.DOFade(alpha, 0.15f);
        _animationHandle.SetAutoKill().Restart();
    }

    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        AudioManager.Play("CardHover");

        SetBlackFillColor(HoverAlpha);

        CanvasManager.Instance.SkillAnnotation.SetAddress(ComplexView.AddressBehaviour.GetAddress());
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        SetBlackFillColor(IdleAlpha);

        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
    }

    public void PointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.SkillAnnotation.UpdateMousePos(eventData.position);
    }
}
