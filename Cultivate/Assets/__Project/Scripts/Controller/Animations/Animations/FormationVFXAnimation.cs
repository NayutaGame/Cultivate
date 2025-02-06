
using DG.Tweening;
using UnityEngine;

public class FormationVFXAnimation : Animation
{
    private IStageModel _model;

    public FormationVFXAnimation(GainFormationDetails gainFormationDetails, bool isAwait) : base(isAwait, gainFormationDetails.Induced)
    {
        _model = gainFormationDetails.Owner.Model();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnBuffVFX));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnBuffVFX()
    {
        GameObject gao = GameObject.Instantiate(GetPrefab(), _model.VFXTransform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        
        VFX vfx = gao.GetComponent<VFX>();
        vfx.Play();
    }

    private GameObject GetPrefab()
    {
        return StageManager.Instance.BuffVFXPrefab;
    }
}
