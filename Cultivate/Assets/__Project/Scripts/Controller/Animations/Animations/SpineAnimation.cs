
using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine.Assertions;

public class SpineAnimation : Animation
{
    public enum IntervalType
    {
        NoInterval,
        Separator,
        StartToEnd,
    }

    public static Dictionary<IntervalType, Func<Spine.Animation, int, float[]>> Operator =
        new()
        {
            { IntervalType.NoInterval, GetDurationForAnimation },
            { IntervalType.Separator, GetSeparatedIntervalsForAnimation },
            { IntervalType.StartToEnd, GetContinuousIntervalsForAnimation },
        };
    
    private SkeletonAnimation _skeleton;
    private string _animationName;
    private IntervalType _intervalType;
    private int _attackTimes;

    public SpineAnimation(SpineModel model, string animationName, IntervalType intervalType, int attackTimes, bool isAwait, bool induced) : base(isAwait, induced)
    {
        _skeleton = model.Skeleton;
        _animationName = animationName;
        _intervalType = intervalType;
        _attackTimes = attackTimes;
    }

    public override AnimationHandle GetHandle()
    {
        Spine.Skeleton skeleton = _skeleton.Skeleton;
        Spine.Animation animation = skeleton.Data.FindAnimation(_animationName);

        return new SpineHandle(this, Operator[_intervalType](animation, _attackTimes), _skeleton, PlayAnimation);
    }

    public override bool InvolvesCharacterAnimation()
        => true;

    private void PlayAnimation()
    {
        _skeleton.AnimationState.SetAnimation(0, _animationName, false);
        if (_animationName != "guard")
            _skeleton.AnimationState.AddAnimation(0, "idle", true, 0);
    }

    private static float[] GetDurationForAnimation(Spine.Animation animation, int attackTimes)
    {
        return new[] { animation?.Duration ?? 0 };
    }
    
    private static float[] GetSeparatedIntervalsForAnimation(Spine.Animation animation, int attackTimes)
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
    
    private static float[] GetContinuousIntervalsForAnimation(Spine.Animation animation, int attackTimes)
    {
        EventTimeline eventTimeline = animation.Timelines.Find(t => t is EventTimeline) as EventTimeline;
        
        Assert.IsTrue(eventTimeline.Events.Length == 2);
        Assert.IsTrue(attackTimes > 3);

        float[] intervals = new float[attackTimes + 1];

        float anticipation = eventTimeline.Events[0].Time;
        float midDuration = eventTimeline.Events[1].Time - eventTimeline.Events[0].Time;
        float recover = animation.Duration - eventTimeline.Events[1].Time;
        float singleDuration = midDuration / (attackTimes - 1);

        intervals[0] = anticipation;

        for (int i = 1; i < attackTimes; i++)
            intervals[i] = singleDuration;
        
        intervals[attackTimes] = recover;

        return intervals;
    }
}
