
using Spine;
using Spine.Unity;

public class HealCharacterAnimation : Animation
{
    private HealDetails _healDetails;

    public HealCharacterAnimation(bool isAwait, HealDetails healDetails) : base(isAwait)
    {
        _healDetails = healDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        StageEntity src = _healDetails.Src;
        EntitySlot slot = src.Slot();
        SkeletonAnimation skeletonAnimation = slot.Skeleton;
        Spine.Skeleton skeleton = skeletonAnimation.Skeleton;
        Spine.Animation animation = skeleton.Data.FindAnimation("hail");
        
        return new SpineHandle(this, GetIntervalsForAnimation(animation), skeletonAnimation, PlayAnimation);
    }

    public override bool InvolvesCharacterAnimation() => true;

    private void PlayAnimation()
    {
        StageEntity src = _healDetails.Src;
        EntitySlot slot = src.Slot();
        slot.Skeleton.AnimationState.SetAnimation(0, "hail", false);
        slot.Skeleton.AnimationState.AddAnimation(0, "idle", true, 0);
    }
    
    private float[] GetIntervalsForAnimation(Spine.Animation animation)
    {
        EventTimeline eventTimeline = animation.Timelines.Find(t => t is EventTimeline) as EventTimeline;

        float[] intervals = new float[eventTimeline.Events.Length + 1];
        for (int i = 0; i < eventTimeline.Events.Length; i++)
        {
            if (i == 0)
            {
                intervals[i] = eventTimeline.Events[i].Time;
            }
            else
            {
                intervals[i] = eventTimeline.Events[i].Time - intervals[i - 1];
            }
        }

        if (eventTimeline.Events.Length == 0)
        {
            intervals[^1] = animation.Duration;
        }
        else
        {
            intervals[^1] = animation.Duration - intervals[^2];
        }

        return intervals;
    }
}
