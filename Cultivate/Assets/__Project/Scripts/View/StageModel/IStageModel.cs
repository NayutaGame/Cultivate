
using UnityEngine;

public abstract class IStageModel : MonoBehaviour
{
    public abstract void Opening();

    public abstract void SetSpeed(float speed);
    
    public abstract Animation GetAnimationFromBuffSelf(bool induced);
    public abstract Animation GetAnimationFromEvaded(bool induced);
    public abstract Animation GetAnimationFromAttack(bool induced);
    public abstract Animation GetAnimationFromDamaged(bool induced);
    public abstract Animation GetAnimationFromGainArmor(bool induced);
    public abstract Animation GetAnimationFromHeal(bool induced);
    public abstract Animation GetAnimationFromGuard(bool induced);
    public abstract Animation GetAnimationFromUnguard(bool induced);

    // buffSelf
    // buffOppo		buffedByOppo
    // debuffSelf
    // debuffOppo	debuffedByOppo
    //
    // armorSelf
    // armorOppo	armoredByOppo
    // dearmorSelf
    // dearmorOppo	dearmoredByOppo
    //
    // healSelf
    // healOppo		healedByOppo
    //
    // manaSelf
    //
    // burn
    // channel
    //
    // attack
    // preAttacked	guard
    //              still
    //              unguard
    // damaged
}
