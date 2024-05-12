
using DG.Tweening;

public class PreAttackedAnimation : Animation
{
    private AttackDetails _attackDetails;

    public PreAttackedAnimation(bool isAwait, AttackDetails attackDetails) : base(isAwait)
    {
        _attackDetails = attackDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, GetAnimation());
    }

    private Tween GetAnimation()
    {
        StageEntity tgt = _attackDetails.Tgt;
        EntitySlot slot = tgt.Slot();
        if (tgt.Armor > 0)
        {
            slot.Skeleton.AnimationState.SetAnimation(0, "guard", false);
        }
        else if (tgt.Armor == 0)
        {
            slot.Skeleton.AnimationState.SetAnimation(0, "still", false);
        }
        else
        {
            slot.Skeleton.AnimationState.SetAnimation(0, "unguard", false);
        }
        return DOTween.Sequence();
    }
}
