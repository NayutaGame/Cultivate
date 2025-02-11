
using Spine.Unity;
using UnityEngine;

public class SpineModel : IStageModel
{
    public SkeletonAnimation Skeleton;

    public override void Opening()
    {
        // Skeleton.AnimationState.SetAnimation(0, "idle", true);
    }

    public override void SetSpeed(float speed)
    {
        // Skeleton.timeScale = speed;
    }

    public override Animation GetAnimationFromBuffSelf(bool induced)
    {
        return new BuffSelfTweenAnimation(BaseTransform, Transform, true, induced);
        // return new SpineAnimation(Skeleton, "hail", true, true, induced);
    }

    public override Animation GetAnimationFromEvaded(bool induced)
    {
        return new EvadedTweenAnimation(BaseTransform, Transform, false, induced);
    }

    public override Animation GetAnimationFromAttack(bool induced)
    {
        return new AttackTweenAnimation(BaseTransform, Transform, true, induced);
        // return new SpineAnimation(Skeleton, "attack1", true, true, induced);
    }

    public override Animation GetAnimationFromDamaged(bool induced)
    {
        return new DamagedTweenAnimation(BaseTransform, Transform, false, induced);
        // return new SpineAnimation(Skeleton, "hit", false, false, induced);
    }

    public override Animation GetAnimationFromGainArmor(bool induced)
    {
        return new GainArmorTweenAnimation(BaseTransform, Transform, true, induced);
        // return new SpineAnimation(Skeleton, "hail", true, true, induced);
    }

    public override Animation GetAnimationFromHeal(bool induced)
    {
        return new HealTweenAnimation(BaseTransform, Transform, true, induced);
        // return new SpineAnimation(Skeleton, "hail", true, true, induced);
    }

    public override Animation GetAnimationFromGuard(bool induced)
    {
        return new EmptyTweenAnimation();
        // return new SpineAnimation(Skeleton, "guard", false, false, induced);
    }

    public override Animation GetAnimationFromUnguard(bool induced)
    {
        return new EmptyTweenAnimation();
        // return new SpineAnimation(Skeleton, "unguard", false, false, induced);
    }
}
