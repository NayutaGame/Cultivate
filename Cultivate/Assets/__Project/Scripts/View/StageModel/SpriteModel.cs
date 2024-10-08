
using UnityEngine;

public class SpriteModel : IStageModel
{
    public Transform BaseTransform;
    public Transform Transform;
    public SpriteRenderer SR;
    
    public override Animation GetAnimationFromBuffSelf(bool induced)
    {
        return new BuffSelfTweenAnimation(BaseTransform, Transform, true, induced);
    }

    public override Animation GetAnimationFromEvaded(bool induced)
    {
        return new EvadedTweenAnimation(BaseTransform, Transform, false, induced);
    }

    public override Animation GetAnimationFromAttack(bool induced)
    {
        return new AttackTweenAnimation(BaseTransform, Transform, true, induced);
    }

    public override Animation GetAnimationFromDamaged(bool induced)
    {
        return new DamagedTweenAnimation(BaseTransform, Transform, false, induced);
    }

    public override Animation GetAnimationFromGainArmor(bool induced)
    {
        return new GainArmorTweenAnimation(BaseTransform, Transform, true, induced);
    }

    public override Animation GetAnimationFromHeal(bool induced)
    {
        return new HealTweenAnimation(BaseTransform, Transform, true, induced);
    }

    public override Animation GetAnimationFromGuard(bool induced)
    {
        return new EmptyTweenAnimation();
    }

    public override Animation GetAnimationFromUnguard(bool induced)
    {
        return new EmptyTweenAnimation();
    }
}
