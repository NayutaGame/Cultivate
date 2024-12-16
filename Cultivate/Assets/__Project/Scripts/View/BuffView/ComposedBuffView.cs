
using DG.Tweening;

public class ComposedBuffView : DelegatingView
{
    protected override Animator InitAnimator()
    {
        Animator animator = new(2);
        animator[-1, 0] = EnterHide;
        animator[-1, 1] = EnterIdle;
        return animator;
    }
    
    private Tween EnterHide()
        => DOTween.Sequence()
            .Append(GetDelegatedView().GetRect().DOScale(1.5f, 0.075f).SetEase(Ease.OutQuad))
            .Append(GetDelegatedView().GetRect().DOScale(0f, 0.075f).SetEase(Ease.InQuad));
    
    private Tween EnterIdle()
        => DOTween.Sequence()
            .Append(GetDelegatedView().GetRect().DOScale(1.5f, 0.075f).SetEase(Ease.OutQuad))
            .Append(GetDelegatedView().GetRect().DOScale(1f, 0.075f).SetEase(Ease.InQuad));
}
