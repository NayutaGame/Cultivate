using DG.Tweening;
using UnityEngine;

public class GainArmorVFXAnimation : Animation
{
    private GainArmorDetails _gainArmorDetails;

    public GainArmorVFXAnimation(bool isAwait, GainArmorDetails gainArmorDetails) : base(isAwait)
    {
        _gainArmorDetails = gainArmorDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnVFX));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnVFX()
    {
        // StageEntity tgt = _gainArmorDetails.Tgt;
        // int value = _gainArmorDetails.Value;
        //
        // GameObject gao = GameObject.Instantiate(StageManager.Instance.HealVFXPrefab, tgt.Slot().VFXTransform.position,
        //     Quaternion.identity, StageManager.Instance.VFXPool);
        // VFX vfx = gao.GetComponent<VFX>();
        // vfx.SetIntensity(IntensityFromValue(value));
        // vfx.Play();
    }

    private float IntensityFromValue(int value)
    {
        return Mathf.InverseLerp(0, 100, value);
    }
}
