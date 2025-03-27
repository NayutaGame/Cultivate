
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    private RectTransform _rect;
    public RectTransform GetRect() => _rect;

    private Animator _animator;
    public Animator GetAnimator() => _animator;

    private bool _hasAwoken;
    
    public virtual void Awake()
    {
        CheckAwake();
    }

    public void CheckAwake()
    {
        if (_hasAwoken)
            return;
        _hasAwoken = true;
        AwakeFunction();
    }
    
    public virtual void AwakeFunction()
    {
        _rect ??= GetComponent<RectTransform>();
        _animator ??= InitAnimator();
    }
    
    public static readonly int ANY = -1;
    public static readonly int HIDE = 0;
    public static readonly int IDLE = 1;

    protected virtual Animator InitAnimator()
    {
        Animator animator = new(2, "Panel");
        animator[HIDE, IDLE] = EnterIdle;
        animator[ANY, HIDE] = EnterHide;
        return animator;
    }

    public virtual void Refresh() { }

    public async UniTask ToggleShowing()
        => await _animator.SetStateAsync(GetAnimator().State != 0 ? 0 : 1);

    public virtual Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(HIDE));

    public virtual Tween EnterHide()
        => DOTween.Sequence().AppendCallback(() => gameObject.SetActive(false));

    public virtual Tween HideTweenWithCurtain()
        => DOTween.Sequence()
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(IDLE))
            .AppendCallback(() => gameObject.SetActive(false));
}
