
using DG.Tweening;
using UnityEngine;

public class HealTweenDescriptor : TweenDescriptor
{
    private HealDetails _healDetails;

    public HealTweenDescriptor(bool isAwait, HealDetails healDetails) : base(isAwait)
    {
        _healDetails = healDetails.Clone();
    }

    public override Tween GetTween()
    {
        return DOTween.Sequence()
            .AppendCallback(SpawnHealVFX);
    }

    private void SpawnHealVFX()
    {
        StageEntity tgt = _healDetails.Tgt;
        int value = _healDetails.Value;

        GameObject gao = GameObject.Instantiate(StageManager.Instance.HealVFXPrefab, tgt.Slot().transform.position,
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
