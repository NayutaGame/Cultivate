using DG.Tweening;
using UnityEngine;

public class LoseArmorVFXAnimation : Animation
{
    private IStageModel _model;
    private LoseArmorDetails _loseArmorDetails;

    public LoseArmorVFXAnimation(LoseArmorDetails loseArmorDetails, bool isAwait) : base(isAwait, loseArmorDetails.Induced)
    {
        _model = loseArmorDetails.Tgt.Model();
        _loseArmorDetails = loseArmorDetails.ShallowClone();
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
        vfx.SetIntensity(IntensityFromValue(_loseArmorDetails.Value));
        vfx.Play();
    }

    private float IntensityFromValue(int value)
    {
        return Mathf.InverseLerp(0, 100, value);
    }

    private GameObject GetPrefab()
    {
        return StageManager.Instance.LoseArmorVFXPrefab;
    }
}
