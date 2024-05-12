
using System;
using Spine.Unity;

public class DamagedAnimation : Animation
{
    private DamageDetails _damageDetails;

    public DamagedAnimation(bool isAwait, DamageDetails damageDetails) : base(isAwait)
    {
        _damageDetails = damageDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        StageEntity tgt = _damageDetails.Tgt;
        EntitySlot slot = tgt.Slot();
        SkeletonAnimation skeletonAnimation = slot.Skeleton;
        
        return new SpineHandle(this, Array.Empty<float>(), skeletonAnimation, PlayAnimation);
    }

    private void PlayAnimation()
    {
        StageEntity tgt = _damageDetails.Tgt;
        EntitySlot slot = tgt.Slot();
        slot.Skeleton.AnimationState.SetAnimation(0, "hit", false);
        slot.Skeleton.AnimationState.AddAnimation(0, "idle", true, 0);
    }
}
