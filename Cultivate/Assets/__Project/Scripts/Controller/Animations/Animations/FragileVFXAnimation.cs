
using DG.Tweening;
using UnityEngine;

public class FragileVFXAnimation : Animation
{
    private IStageModel _model;

    public FragileVFXAnimation(AttackDetails attackDetails, bool isAwait) : base(isAwait, attackDetails.Induced)
    {
        _model = attackDetails.Tgt.Model();
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
        return StageManager.Instance.FragileVFXPrefab;
    }
}
