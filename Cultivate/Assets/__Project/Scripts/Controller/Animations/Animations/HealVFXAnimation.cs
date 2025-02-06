
using DG.Tweening;
using UnityEngine;

public class HealVFXAnimation : Animation
{
    private IStageModel _model;
    private HealDetails _healDetails;

    public HealVFXAnimation(HealDetails healDetails, bool isAwait) : base(isAwait, healDetails.Induced)
    {
        _model = healDetails.Tgt.Model();
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
        GameObject gao = GameObject.Instantiate(GetPrefab(), _model.VFXTransform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.SetIntensity(IntensityFromValue(_healDetails.Value));
        vfx.Play();
    }

    private float IntensityFromValue(int value)
    {
        return Mathf.InverseLerp(0, 100, value);
    }

    private GameObject GetPrefab()
    {
        return StageManager.Instance.HealVFXPrefab;
    }
}
