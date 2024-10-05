
using DG.Tweening;
using UnityEngine;

public class FragileVFXAnimation : Animation
{
    private AttackDetails _attackDetails;

    public FragileVFXAnimation(bool isAwait, AttackDetails attackDetails) : base(isAwait, attackDetails.Induced)
    {
        _attackDetails = attackDetails;
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnVFX));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnVFX()
    {
        StageEntity tgt = _attackDetails.Tgt;

        GameObject gao = GameObject.Instantiate(StageManager.Instance.FragileVFXPrefab, tgt.Slot().VFXTransform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.Play();
    }
}
