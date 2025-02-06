using DG.Tweening;
using UnityEngine;

public class GainArmorVFXAnimation : Animation
{
    private IStageModel _model;
    private GainArmorDetails _gainArmorDetails;

    public GainArmorVFXAnimation(GainArmorDetails gainArmorDetails, bool isAwait) : base(isAwait, gainArmorDetails.Induced)
    {
        _model = gainArmorDetails.Tgt.Model();
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
        GameObject gao = GameObject.Instantiate(GetPrefab(), _model.VFXTransform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.SetIntensity(IntensityFromValue(_gainArmorDetails.Value));
        vfx.Play();
    }

    private float IntensityFromValue(int value)
    {
        return Mathf.InverseLerp(0, 100, value);
    }

    private GameObject GetPrefab()
    {
        return StageManager.Instance.GainArmorVFXPrefab;
    }
}
