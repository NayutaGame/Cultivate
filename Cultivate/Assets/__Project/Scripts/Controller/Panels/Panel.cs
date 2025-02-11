
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    [NonSerialized] public RectTransform RectTransform;
    public Animator Animator;
    
    public virtual void Configure()
    {
        RectTransform ??= GetComponent<RectTransform>();
        Animator ??= InitAnimator();
    }

    protected virtual Animator InitAnimator()
    {
        // 0 for hide, 1 for show
        Animator animator = new(2, "Panel");
        animator[0, 1] = ShowTween;
        animator[-1, 0] = HideTween;
        return animator;
    }

    public virtual void Refresh() { }

    public async UniTask ToggleShowing()
        => await Animator.SetStateAsync(Animator.State != 0 ? 0 : 1);

    public virtual Tween ShowTween()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasManager.Instance.Curtain.Animator.SetStateTween(0));

    public virtual Tween HideTween()
        => DOTween.Sequence().AppendCallback(() => gameObject.SetActive(false));

    public virtual Tween HideTweenWithCurtain()
        => DOTween.Sequence()
            .Append(CanvasManager.Instance.Curtain.Animator.SetStateTween(1))
            .AppendCallback(() => gameObject.SetActive(false));
}
