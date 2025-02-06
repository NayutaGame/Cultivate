
using DG.Tweening;
using UnityEngine;

public class HitVFXAnimation : Animation
{
    private IStageModel _model;
    private AttackDetails _attackDetails;

    public HitVFXAnimation(AttackDetails attackDetails, bool isAwait) : base(isAwait, attackDetails.Induced)
    {
        _model = attackDetails.Tgt.Model();
        _attackDetails = attackDetails.ShallowClone();
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
        WuXing wuXing = _attackDetails.WuXing ?? WuXing.Jin;
        int value = _attackDetails.Value;

        int orient = -(src.Index * 2 - 1);
        GameObject gao = GameObject.Instantiate(GetPrefab(wuXing), _model.VFXTransform.position + -0.5f * orient * Vector3.right,
            Quaternion.identity, StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.SetIntensity(IntensityFromValue(value));
        vfx.Play();
    }

    private float IntensityFromValue(int value)
    {
        return Mathf.InverseLerp(0, 100, value);
    }

    private GameObject GetPrefab(WuXing wuXing)
    {
        return StageManager.Instance.HitVFXFromWuXing[wuXing];
    }
}
