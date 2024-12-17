
using DG.Tweening;
using TMPro;

public class ComposedBuffView : DelegatingView
{
    protected override Animator InitAnimator()
    {
        Animator animator = new(2);
        animator[-1, 0] = EnterHide;
        animator[-1, 1] = EnterIdle;
        return animator;
    }

    public override void SetAddress(Address address)
    {
        Get<Buff>()?.PingNeuron.Remove(SetIdle);
        base.SetAddress(address);
        Get<Buff>()?.PingNeuron.Add(SetIdle);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        Get<Buff>()?.PingNeuron.Remove(SetIdle);
    }

    private void SetIdle()
        => GetAnimator().SetStateAsync(1);
    
    private Tween EnterHide()
        => GetDelegatedView().GetRect().DOScale(0f, 0.075f).SetEase(Ease.InQuad);
    
    private Tween EnterIdle()
        => DOTween.Sequence()
            .Append(GetDelegatedView().GetRect().DOScale(1.5f, 0.075f).SetEase(Ease.OutQuad))
            .AppendCallback(Refresh)
            .Append(GetDelegatedView().GetRect().DOScale(1f, 0.075f).SetEase(Ease.InQuad));
}
