using DG.Tweening;
using UnityEngine;

public class GuardedVFXAnimation : Animation
{
    private IStageModel _model;

    public GuardedVFXAnimation(GuardedDetails guardedDetails, bool isAwait) : base(isAwait, guardedDetails.Induced)
    {
        _model = guardedDetails.Tgt.Model();
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
        vfx.Play();
    }

    private GameObject GetPrefab()
    {
        return StageManager.Instance.GuardedVFXPrefab;
    }
}
