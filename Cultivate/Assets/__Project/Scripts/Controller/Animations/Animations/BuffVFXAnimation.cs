
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

        GameObject prefab;

        if (d._buffEntry.GetName() == "灵气")
            prefab = StageManager.Instance.LingQiVFXPrefab;
        else
            prefab = d._buffEntry.Friendly
                ? StageManager.Instance.BuffVFXPrefab
                : StageManager.Instance.DebuffVFXPrefab;
        
        GameObject gao = GameObject.Instantiate(prefab, d.Tgt.Slot().VFXTransform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        
        VFX vfx = gao.GetComponent<VFX>();
        vfx.Play();
    }
}
