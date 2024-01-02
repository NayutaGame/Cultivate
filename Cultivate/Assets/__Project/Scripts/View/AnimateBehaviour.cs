
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
    private Tween _animationHandle;

    [Header("Pivot")]
    [SerializeField] private RectTransform IdlePivot;
    [SerializeField] private RectTransform HoverPivot;

    [Header("BlackFill")]
    [SerializeField] private Image _blackFill;
    [SerializeField] private float IdleAlpha = 0f;
    [SerializeField] private float HoverAlpha = 0.2f;

    private void OnEnable()
    {
        ComplexView.HoverNeuron.Add(GoToHover);
        ComplexView.UnhoverNeuron.Add(GoToIdle);
        GoToIdleImmediately();
    }

    private void OnDisable()
    {
        ComplexView.HoverNeuron.Remove(GoToHover);
        ComplexView.UnhoverNeuron.Remove(GoToIdle);
    }

    private void GoToIdleImmediately()
    {
        _animationHandle?.Kill();
        ComplexView.SetDisplayTransform(IdlePivot);
        _blackFill.color = new Color(1, 1, 1, IdleAlpha);
    }

    private void GoToIdle(InteractBehaviour ib, PointerEventData eventData)
        => GoToIdle();

    public void GoToIdle()
    {
        SetPivot(IdlePivot);
        SetBlackFillColor(IdleAlpha);
    }

    private void GoToHover(InteractBehaviour ib, PointerEventData eventData)
    {
        SetPivot(HoverPivot);
        SetBlackFillColor(HoverAlpha);
    }






    private void SetPivot(RectTransform pivot)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(ComplexView.GetDisplayTransform(), pivot);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }

    private void SetBlackFillColor(float alpha)
    {
        _animationHandle?.Kill();
        _animationHandle = _blackFill.DOFade(alpha, 0.15f);
        _animationHandle.SetAutoKill().Restart();
    }

    private void SetPivotWithoutAnimation(RectTransform pivot)
    {
        _animationHandle?.Kill();
        ComplexView.SetDisplayTransform(pivot);
    }

    private void SetStartAndPivot(RectTransform start, RectTransform pivot)
    {
        SetPivotWithoutAnimation(start);
        SetPivot(pivot);
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
        SetStartAndPivot(CanvasManager.Instance.SkillGhost.AddressBehaviour.RectTransform, IdlePivot);
        CanvasManager.Instance.SkillGhost.EndDrag();
    }

    public void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        CanvasManager.Instance.SkillGhost.Drag(FollowPivot, eventData.position);
    }



















    private Tween _selectHandle;

    public void SetSelected(bool selected)
    {
        // _selected = selected;
        //
        // _selectHandle?.Kill();
        // _selectHandle = SelectionImage.DOFade(_selected ? 1 : 0, 0.15f);
        // _selectHandle.Restart();
    }
}
