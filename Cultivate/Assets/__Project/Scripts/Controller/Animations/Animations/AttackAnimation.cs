
using DG.Tweening;
using Spine;
using Spine.Unity;

public class AttackAnimation : Animation
{
    private AttackDetails _attackDetails;

    public AttackAnimation(bool isAwait, AttackDetails attackDetails) : base(isAwait)
    {
        _attackDetails = attackDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        // Spine handle
        return new TweenHandle(this, GetAnimation()); 
    }

    private Tween GetAnimation()
    {
        StageEntity src = _attackDetails.Src;
        EntitySlot slot = src.Slot();
        slot.Skeleton.AnimationState.SetAnimation(0, "attack1", false);
        
        // float duration = slot.Skeleton.Skeleton.Data.FindAnimation("attack1").Duration;
        // slot.Skeleton.Skeleton.SetToSetupPose();

        return DOTween.Sequence().AppendInterval(GetTimeToFire());
    }
    
    private float GetTimeToFire()
    {
        StageEntity src = _attackDetails.Src;
        EntitySlot slot = src.Slot();
        SkeletonAnimation skeletonAnimation = slot.Skeleton;
        Spine.Skeleton skeleton = skeletonAnimation.Skeleton;
        Spine.Animation animation = skeleton.Data.FindAnimation("attack1");
        EventTimeline eventTimeline = animation.Timelines.Find(t => t is EventTimeline) as EventTimeline;

        // List<Spine.Event> events = eventTimeline.Events.ToList();
        return eventTimeline.Events[0].Time;
    }
}
