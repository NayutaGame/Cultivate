
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

    protected virtual Animator InitAnimator()
    {
        // 0 for hide, 1 for show
        Animator animator = new(2, "Panel");
        animator[0, 1] = EnterIdle;
        animator[-1, 0] = EnterHide;
        return animator;
    }

    public virtual void Refresh() { }

    public async UniTask ToggleShowing()
        => await _animator.SetStateAsync(GetAnimator().State != 0 ? 0 : 1);

    public virtual Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(0));

    public virtual Tween EnterHide()
        => DOTween.Sequence().AppendCallback(() => gameObject.SetActive(false));

    public virtual Tween HideTweenWithCurtain()
        => DOTween.Sequence()
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(1))
            .AppendCallback(() => gameObject.SetActive(false));
}
