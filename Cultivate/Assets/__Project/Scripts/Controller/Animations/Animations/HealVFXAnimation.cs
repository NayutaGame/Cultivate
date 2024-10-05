
using DG.Tweening;
using UnityEngine;

public class HealVFXAnimation : Animation
{
    private HealDetails _healDetails;

    public HealVFXAnimation(bool isAwait, HealDetails healDetails) : base(isAwait, healDetails.Induced)
    {
        _healDetails = healDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnVFX));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnVFX()
    {
        StageEntity tgt = _healDetails.Tgt;
        int value = _healDetails.Value;

        GameObject gao = GameObject.Instantiate(StageManager.Instance.HealVFXPrefab, tgt.Slot().VFXTransform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.SetIntensity(IntensityFromValue(value));
        vfx.Play();
    }

    private float IntensityFromValue(int value)
    {
        return Mathf.InverseLerp(0, 100, value);
    }
}
