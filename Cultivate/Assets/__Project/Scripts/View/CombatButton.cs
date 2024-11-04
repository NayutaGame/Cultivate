
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Tween = DG.Tweening.Tween;

public class CombatButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] public RectTransform _target;

    private Animator _animator;
    private Tween _handle;

    [HideInInspector] public Neuron<PointerEventData> ClickNeuron = new();
    private bool _isAttractive;
    public void SetAttractive(bool value)
    {
        _isAttractive = value;
        if (_animator.State != 3)
            _animator.SetStateAsync(!_isAttractive ? 1 : 2);
    }

    public void Configure()
    {
        _animator ??= InitAnimator();
        
        ClickNeuron.Join(ClickVFX);
    }

    protected virtual Animator InitAnimator()
    {
        // 0 for no state, 1 for idle, 2 for attractive, 3 for hover
        Animator animator = new(4, "CombatButton");
        animator[0, 1] = IdleTween;
        animator[2, 1] = IdleTween;
        animator[3, 1] = IdleTween;
        animator[0, 2] = AttractiveTween;
        animator[1, 2] = AttractiveTween;
        animator[3, 2] = AttractiveTween;
        animator[0, 3] = HoverTween;
        animator[1, 3] = HoverTween;
        animator[2, 3] = HoverTween;
        
        return animator;
    }

    public void OnPointerEnter(PointerEventData eventData)
        => _animator.SetStateAsync(3);

    public void OnPointerExit(PointerEventData eventData)
        => _animator.SetStateAsync(!_isAttractive ? 1 : 2);

    public void OnPointerClick(PointerEventData eventData)
        => ClickNeuron.Invoke(eventData);

    private Tween IdleTween()
        => TweenAnimation.Jump(_target);
    
    private Tween AttractiveTween()
        => TweenAnimation.Beats(_target);

    private Tween HoverTween()
        => _target.DOScale(1.2f * Vector3.one, 0.2f).SetEase(Ease.OutQuad);

    private void ClickVFX(PointerEventData d)
    {
        
    }
}
