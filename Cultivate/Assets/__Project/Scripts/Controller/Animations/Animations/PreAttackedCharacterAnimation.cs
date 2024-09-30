
using System;
using Spine.Unity;

public class PreAttackedCharacterAnimation : Animation
{
    private AttackDetails _attackDetails;

    public PreAttackedCharacterAnimation(bool isAwait, AttackDetails attackDetails) : base(isAwait)
    {
        _attackDetails = attackDetails.ShallowClone();
    }

    public override AnimationHandle GetHandle()
    {
        StageEntity src = _attackDetails.Src;
        EntitySlot slot = src.Slot();
        SkeletonAnimation skeletonAnimation = slot.Skeleton;
        
        return new SpineHandle(this, Array.Empty<float>(), skeletonAnimation, PlayAnimation);
    }
    
    public override bool InvolvesCharacterAnimation() => true;

    private void PlayAnimation()
    {
        StageEntity tgt = _attackDetails.Tgt;
        EntitySlot slot = tgt.Slot();

        string animationName;
        
        if (tgt.Armor > 0)
        {
            animationName = "guard";
        }
        else if (tgt.Armor == 0)
        {
            animationName = "still";
        }
        else
        {
            animationName = "unguard";
        }
        
        slot.Skeleton.AnimationState.SetAnimation(0, animationName, false);
        slot.Skeleton.AnimationState.AddAnimation(0, "idle", true, 0);
    }
}
