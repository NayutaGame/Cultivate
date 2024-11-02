using DG.Tweening;
using UnityEngine;

public class GuardedVFXAnimation : Animation
{
    private GuardedDetails _guardedDetails;

    public GuardedVFXAnimation(bool isAwait, GuardedDetails guardedDetails) : base(isAwait, guardedDetails.Induced)
    {
        _guardedDetails = guardedDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnVFX));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnVFX()
    {
        StageEntity tgt = _guardedDetails.Tgt;

        GameObject gao = GameObject.Instantiate(StageManager.Instance.GuardedVFXPrefab, tgt.Model().VFXTransform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.Play();
    }
}
