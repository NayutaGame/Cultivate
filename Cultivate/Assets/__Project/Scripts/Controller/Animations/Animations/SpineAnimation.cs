
using System;
using Spine;
using Spine.Unity;
using UnityEngine.Assertions;

public class SpineAnimation : Animation
{
    public enum IntervalType
    {
        Separator,
        StartToEnd,
    }
    
    private SkeletonAnimation _skeleton;
    private string _animationName;
    private IntervalType _intervalType;
    private int _attackTimes;

    public SpineAnimation(SkeletonAnimation skeleton, string animationName, IntervalType intervalType, int attackTimes, bool isAwait, bool induced) : base(isAwait, induced)
    {
        _skeleton = skeleton;
        _animationName = animationName;
        _intervalType = intervalType;
        _attackTimes = attackTimes;
    }

    public override AnimationHandle GetHandle()
    {
        Spine.Skeleton skeleton = _skeleton.Skeleton;
        Spine.Animation animation = skeleton.Data.FindAnimation(_animationName);

        return new SpineHandle(this,
            _intervalType == IntervalType.Separator
                ? GetSeparatedIntervalsForAnimation(animation)
                : GetContinuousIntervalsForAnimation(animation), _skeleton, PlayAnimation);
    }

    public override bool InvolvesCharacterAnimation()
        => true;

    private void PlayAnimation()
    {
        _skeleton.AnimationState.SetAnimation(0, _animationName, false);
        _skeleton.AnimationState.AddAnimation(0, "idle", true, 0);
    }
    
    private float[] GetSeparatedIntervalsForAnimation(Spine.Animation animation)
    {
        EventTimeline eventTimeline = animation.Timelines.Find(t => t is EventTimeline) as EventTimeline;

        if (eventTimeline.Events.Length == 0)
            return new float[1] { animation.Duration };
        
        float[] intervals = new float[eventTimeline.Events.Length + 1];
        
        intervals[0] = eventTimeline.Events[0].Time;
        
        for (int i = 1; i < eventTimeline.Events.Length; i++)
        {
            intervals[i] = eventTimeline.Events[i].Time - intervals[i - 1];
        }
        
        intervals[^1] = animation.Duration - intervals[^2];

        return intervals;
    }
    
    private float[] GetContinuousIntervalsForAnimation(Spine.Animation animation)
    {
        EventTimeline eventTimeline = animation.Timelines.Find(t => t is EventTimeline) as EventTimeline;
        
        Assert.IsTrue(eventTimeline.Events.Length == 2);
        Assert.IsTrue(_attackTimes > 3);

        float[] intervals = new float[_attackTimes + 1];

        float anticipation = eventTimeline.Events[0].Time;
        float midDuration = eventTimeline.Events[1].Time - eventTimeline.Events[0].Time;
        float recover = animation.Duration - eventTimeline.Events[1].Time;
        float singleDuration = midDuration / (_attackTimes - 1);

        intervals[0] = anticipation;

        for (int i = 1; i < _attackTimes; i++)
            intervals[i] = singleDuration;
        
        intervals[_attackTimes] = recover;

        return intervals;
    }
}
