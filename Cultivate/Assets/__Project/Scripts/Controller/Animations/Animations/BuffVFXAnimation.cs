
using DG.Tweening;
using UnityEngine;

public class BuffVFXAnimation : Animation
{
    public GainBuffDetails GainBuffDetails;

    public BuffVFXAnimation(bool isAwait, GainBuffDetails gainBuffDetails) : base(isAwait, gainBuffDetails.Induced)
    {
        GainBuffDetails = gainBuffDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnBuffVFX));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnBuffVFX()
    {
        GainBuffDetails d = GainBuffDetails;

        GameObject prefab = GetPrefab(d._buffEntry);
        
        GameObject gao = GameObject.Instantiate(prefab, d.Tgt.Model().VFXTransform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        
        VFX vfx = gao.GetComponent<VFX>();
        vfx.Play();
    }

    private GameObject GetPrefab(BuffEntry buffEntry)
    {
        if (buffEntry.GetName() == "灵气")
            return StageManager.Instance.LingQiVFXPrefab;

        if (buffEntry.Friendly)
            return StageManager.Instance.BuffVFXPrefab;
        
        return StageManager.Instance.DebuffVFXPrefab;
    }
}
