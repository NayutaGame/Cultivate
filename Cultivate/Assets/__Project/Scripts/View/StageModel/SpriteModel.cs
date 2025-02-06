
using UnityEngine;

public class SpriteModel : IStageModel
{
    public SpriteRenderer SR;

    public override void SetSpeed(float speed) { }

    public override Animation GetAnimationFromEntering()
    {
        return new EmptyAnimation();
    }

    public override Animation GetAnimationFromBuffSelf(bool induced)
    {
        return new BuffSelfTweenAnimation(this, true, induced);
    }

    public override Animation GetAnimationFromEvaded(bool induced)
    {
        return new EvadedTweenAnimation(this, false, induced);
    }

    public override Animation GetAnimationFromAttack(bool induced, int times)
    {
        return new AttackTweenAnimation(this, true, induced);
    }

    public override Animation GetAnimationFromDamaged(bool induced)
    {
        return new DamagedTweenAnimation(this, false, induced);
    }

    public override Animation GetAnimationFromGainArmor(bool induced)
    {
        return new GainArmorTweenAnimation(this, true, induced);
    }

    public override Animation GetAnimationFromHeal(bool induced)
    {
        return new HealTweenAnimation(this, true, induced);
    }

    public override Animation GetAnimationFromGuard(bool induced)
    {
        return new EmptyAnimation();
    }

    public override Animation GetAnimationFromUnguard(bool induced)
    {
        return new EmptyAnimation();
    }

    public override Animation GetAnimationFromRecover()
    {
        return new EmptyAnimation();
    }
}
