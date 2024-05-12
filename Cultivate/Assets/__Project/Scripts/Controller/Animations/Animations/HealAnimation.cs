
using DG.Tweening;
using UnityEngine;

public class HealAnimation : Animation
{
    private HealDetails _healDetails;

    public HealAnimation(bool isAwait, HealDetails healDetails) : base(isAwait)
    {
        _healDetails = healDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnHealVFX));
    }

    private void SpawnHealVFX()
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
