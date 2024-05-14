
using DG.Tweening;
using UnityEngine;

public class HitVFXAnimation : Animation
{
    private AttackDetails _attackDetails;

    public HitVFXAnimation(bool isAwait, AttackDetails attackDetails) : base(isAwait)
    {
        _attackDetails = attackDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnVFX));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnVFX()
    {
        StageEntity src = _attackDetails.Src;
        StageEntity tgt = _attackDetails.Tgt;
        WuXing wuXing = _attackDetails.WuXing ?? WuXing.Jin;
        int value = _attackDetails.Value;

        int orient = -(src.Index * 2 - 1);
        GameObject gao = GameObject.Instantiate(StageManager.Instance.HitVFXFromWuXing[wuXing], tgt.Slot().VFXTransform.position + -0.5f * orient * Vector3.right,
            Quaternion.identity, StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.SetIntensity(IntensityFromValue(value));
        vfx.Play();
    }

    // private Tween GetAttackedTween()
    // {
    //     StageEntity src = _attackDetails.Src;
    //     StageEntity tgt = _attackDetails.Tgt;
    //
    //     Transform entityTransform = tgt.Slot().EntityTransform;
    //     int orient = -(src.Index * 2 - 1);
    //
    //     return entityTransform
    //         .DOShakeRotation(0.6f, 10 * orient * Vector3.back, 10, 90, true, ShakeRandomnessMode.Harmonic)
    //         .SetEase(Ease.InQuad);
    // }

    private float IntensityFromValue(int value)
    {
        return Mathf.InverseLerp(0, 100, value);
    }
}
