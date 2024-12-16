
using DG.Tweening;
using UnityEngine;

public class FormationVFXAnimation : Animation
{
    public GainFormationDetails GainFormationDetails;

    public FormationVFXAnimation(bool isAwait, GainFormationDetails gainFormationDetails) : base(isAwait, gainFormationDetails.Induced)
    {
        GainFormationDetails = gainFormationDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnBuffVFX));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnBuffVFX()
    {
        GainFormationDetails d = GainFormationDetails;

        GameObject prefab = GetPrefab();
        
        GameObject gao = GameObject.Instantiate(prefab, d.Owner.Model().VFXTransform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        
        VFX vfx = gao.GetComponent<VFX>();
        vfx.Play();
    }

    private GameObject GetPrefab()
    {
        return StageManager.Instance.BuffVFXPrefab;
    }
}
