
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PenetrateSkillView : SkillView
{
    // TODO: interact behaviour separation
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
}
