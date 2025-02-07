
using Spine.Unity;

public class SpineModel : IStageModel
{
    public SkeletonAnimation Skeleton;

    public override void SetSpeed(float speed)
    {
        Skeleton.timeScale = speed;
    }

    public override Animation GetAnimationFromEntering()
    {
        // return new SpineAnimation(this, "idle", SpineAnimation.IntervalType.NoInterval, 0, true, false);
        return new SpineAnimation(this, "entering", "idle", SpineAnimation.IntervalType.NoInterval, 0, true, false);
    }

    public override Animation GetAnimationFromBuffSelf(bool induced)
    {
        return new SpineAnimation(this, "hail", "idle", SpineAnimation.IntervalType.NoInterval, 0, true, induced);
    }

    public override Animation GetAnimationFromEvaded(bool induced)
    {
        // return new EvadedTweenAnimation(BaseTransform, Transform, false, induced);
        return new SpineAnimation(this, "evade", "idle", SpineAnimation.IntervalType.NoInterval, 0, false, induced);
    }

    public override Animation GetAnimationFromAttack(bool induced, int times)
    {
        // return new AttackTweenAnimation(BaseTransform, Transform, true, induced);
        
        string animationName;
        SpineAnimation.IntervalType intervalType;
        
        if (times == 1)
        {
            animationName = "attack1";
            intervalType = SpineAnimation.IntervalType.Separator;
        }
        else if (times == 2)
        {
            animationName = "attack2";
            intervalType = SpineAnimation.IntervalType.Separator;
        }
        else if (times == 3)
        {
            animationName = "attack3";
            intervalType = SpineAnimation.IntervalType.Separator;
        }
        else
        {
            animationName = "attackn";
            intervalType = SpineAnimation.IntervalType.StartToEnd;
        }
        
        return new SpineAnimation(this, animationName, "idle", intervalType, times, true, induced);
    }

    public override Animation GetAnimationFromDamaged(bool induced)
    {
        // return new DamagedTweenAnimation(BaseTransform, Transform, false, induced);
        return new SpineAnimation(this, "damaged", "idle", SpineAnimation.IntervalType.NoInterval, 0, false, induced);
    }

    public override Animation GetAnimationFromGainArmor(bool induced)
    {
        return new GainArmorTweenAnimation(this, true, induced);
        // return new SpineAnimation(Skeleton, "hail", true, true, induced);
    }

    public override Animation GetAnimationFromHeal(bool induced)
    {
        return new HealTweenAnimation(this, true, induced);
        // return new SpineAnimation(Skeleton, "hail", true, true, induced);
    }

    public override Animation GetAnimationFromGuard(bool induced)
    {
        // return new EmptyTweenAnimation();
        return new SpineAnimation(this, "guard", "guard", SpineAnimation.IntervalType.NoInterval, 0, false, induced);
    }

    public override Animation GetAnimationFromUnguard(bool induced)
    {
        return new EmptyAnimation();
    }

    public override Animation GetAnimationFromRecover()
    {
        // return new EmptyTweenAnimation();
        return new SpineAnimation(this, "idle", "idle", SpineAnimation.IntervalType.NoInterval, 0, false, false);
    }

    public override Animation GetAnimationFromWin()
    {
        return new SpineAnimation(this, "win", null, SpineAnimation.IntervalType.NoInterval, 0, false, false);
    }

    public override Animation GetAnimationFromLose()
    {
        return new SpineAnimation(this, "lose", "loseB", SpineAnimation.IntervalType.NoInterval, 0, false, false);
    }

    public override Animation GetAnimationFromDefeat()
    {
        return new SpineAnimation(this, "defeat", "defeatB", SpineAnimation.IntervalType.NoInterval, 0, false, false);
    }
}
