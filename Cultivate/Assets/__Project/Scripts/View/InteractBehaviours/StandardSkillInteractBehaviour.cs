
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StandardSkillInteractBehaviour : InteractBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler
{
    public static readonly float IdleAlpha = 0;
    public static readonly float HoverAlpha = 0.2f;

    [SerializeField] public Image _blackFill;

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

    public void HoverAnimation(InteractBehaviour ib, PointerEventData eventData)
    {
        AudioManager.Play("CardHover");
        SetBlackFillColor(HoverAlpha);
        CanvasManager.Instance.SkillAnnotation.SetAddressFromIB(ib, eventData);
    }

    public void UnhoverAnimation(InteractBehaviour ib, PointerEventData eventData)
    {
        SetBlackFillColor(IdleAlpha);
        CanvasManager.Instance.SkillAnnotation.SetAddressToNull(ib, eventData);
    }

    public void PointerMove(InteractBehaviour ib, PointerEventData eventData)
    {
        CanvasManager.Instance.SkillAnnotation.UpdateMousePos(ib, eventData);
    }
}
