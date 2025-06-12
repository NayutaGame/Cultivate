
using DG.Tweening;
using UnityEngine;

public class EvadedVFXAnimation : Animation
{
    private IStageModel _model;
    private int _orient;

    public EvadedVFXAnimation(EvadedDetails evadedDetails, bool isAwait) : base(isAwait, evadedDetails.Induced)
    {
        _model = evadedDetails.Tgt.Model();
        _orient = -(evadedDetails.Tgt.Index * 2 - 1);
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnVFX));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnVFX()
    {
        GameObject gao = GameObject.Instantiate(GetPrefab(), _model.FootTransform.position + _orient * Vector3.right,
            Quaternion.identity, StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.Play();
    }

    private GameObject GetPrefab()
    {
        return StageManager.Instance.DodgeVFXPrefab;
    }
}
