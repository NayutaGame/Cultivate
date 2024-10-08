
using System;
using Spine;
using Spine.Unity;

public class SpineAnimation : Animation
{
    private SkeletonAnimation _skeleton;
    private string _animationName;
    private bool _useIntervals;

    public SpineAnimation(SkeletonAnimation skeleton, string animationName, bool useIntervals, bool isAwait, bool induced) : base(isAwait, induced)
    {
        _skeleton = skeleton;
        _animationName = animationName;
        _useIntervals = useIntervals;
    }

    public override AnimationHandle GetHandle()
    {
        Spine.Skeleton skeleton = _skeleton.Skeleton;
        Spine.Animation animation = skeleton.Data.FindAnimation(_animationName);
        
        return new SpineHandle(this, _useIntervals ? GetIntervalsForAnimation(animation) : Array.Empty<float>(), _skeleton, PlayAnimation);
    }

    public override bool InvolvesCharacterAnimation()
        => true;

    private void PlayAnimation()
    {
        _skeleton.AnimationState.SetAnimation(0, _animationName, false);
        _skeleton.AnimationState.AddAnimation(0, "idle", true, 0);
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
