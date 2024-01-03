
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimateBehaviour : MonoBehaviour
{
    public ComplexView ComplexView;

    public enum TransitionType
    {
        None,
        Pivot,
        BlackFill,
    }

    [SerializeField] private TransitionType _transitionType;
    private Tween _handle;

    [Header("Pivot")]
    [SerializeField] private RectTransform IdlePivot;
    [SerializeField] private RectTransform HoverPivot;

    [Header("BlackFill")]
    [SerializeField] private Image _blackFill;
    [SerializeField] private float IdleAlpha = 0f;
    [SerializeField] private float HoverAlpha = 0.2f;

    private void OnEnable()
    {
        ComplexView.HoverNeuron.Add(AnimateToHover);
        ComplexView.UnhoverNeuron.Add(AnimateToIdle);

        SetToIdle();
    }

    private void OnDisable()
    {
        ComplexView.HoverNeuron.Remove(AnimateToHover);
        ComplexView.UnhoverNeuron.Remove(AnimateToIdle);
    }

    private void SetToIdle()
    {
        switch (_transitionType)
        {
            case TransitionType.Pivot:
                SetDisplay(IdlePivot);
                break;
            case TransitionType.BlackFill:
                SetBlackFill(IdleAlpha);
                break;
        }
    }

    private void AnimateToIdle(InteractBehaviour ib, PointerEventData eventData)
        => AnimateToIdle();

    public void AnimateToIdle()
    {
        switch (_transitionType)
        {
            case TransitionType.Pivot:
                AnimateDisplay(IdlePivot);
                break;
            case TransitionType.BlackFill:
                AnimateBlackFill(IdleAlpha);
                break;
        }
    }

    private void AnimateToHover(InteractBehaviour ib, PointerEventData eventData)
    {
        switch (_transitionType)
        {
            case TransitionType.Pivot:
                AnimateDisplay(HoverPivot);
                break;
            case TransitionType.BlackFill:
                AnimateBlackFill(HoverAlpha);
                break;
        }
    }



    public void SetDisplay(RectTransform end)
    {
        _handle?.Kill();
        ComplexView.SetDisplayTransform(end);
    }

    public void AnimateDisplay(RectTransform start, RectTransform end)
    {
        SetDisplay(start);
        AnimateDisplay(end);
    }

    public void AnimateDisplay(RectTransform end)
    {
        _handle?.Kill();
        FollowAnimation f = new FollowAnimation(ComplexView.GetDisplayTransform(), end);
        _handle = f.GetHandle();
        _handle.SetAutoKill().Restart();
    }

    public void SetBlackFill(float end)
    {
        _handle?.Kill();
        _blackFill.color = new Color(_blackFill.color.r, _blackFill.color.g, _blackFill.color.b, end);
    }

    public void AnimateBlackFill(float start, float end)
    {
        SetBlackFill(start);
        AnimateBlackFill(end);
    }

    public void AnimateBlackFill(float end)
    {
        _handle?.Kill();
        _handle = _blackFill.DOFade(end, 0.15f);
        _handle.SetAutoKill().Restart();
    }















    [Header("Drag")]
    [SerializeField] private RectTransform FollowPivot;


    public void BeginDrag(InteractBehaviour ib, PointerEventData eventData)
    {
        CanvasManager.Instance.SkillAnnotation.SetAddressToNull(ib, eventData);

        // SetRaycastable(false);
        // SetOpaque(false);
        CanvasManager.Instance.SkillGhost.BeginDrag(ComplexView.AddressBehaviour.GetAddress(),
            ComplexView.GetDisplayTransform(), FollowPivot);
    }

    public void EndDrag(InteractBehaviour ib, PointerEventData eventData)
    {
        // SetRaycastable(true);
        // SetOpaque(true);
        AnimateDisplay(CanvasManager.Instance.SkillGhost.AddressBehaviour.RectTransform, IdlePivot);
        CanvasManager.Instance.SkillGhost.EndDrag();
    }

    public void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        CanvasManager.Instance.SkillGhost.Drag(FollowPivot, eventData.position);
    }



















    private Tween _selectHandle;

    public void AnimateSelect(Image target, bool value)
    {
        _selectHandle?.Kill();
        _selectHandle = target.DOFade(value ? 1 : 0, 0.15f);
        _selectHandle.Restart();
    }
}
