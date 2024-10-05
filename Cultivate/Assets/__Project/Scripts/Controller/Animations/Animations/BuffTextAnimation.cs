using DG.Tweening;
using UnityEngine;

public class BuffTextAnimation : Animation
{
    public GainBuffDetails GainBuffDetails;

    public BuffTextAnimation(bool isAwait, GainBuffDetails gainBuffDetails) : base(isAwait, gainBuffDetails.Induced)
    {
        GainBuffDetails = gainBuffDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnText));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnText()
    {
        GainBuffDetails d = GainBuffDetails;

        GameObject gao = GameObject.Instantiate(StageManager.Instance.FloatTextVFXPrefab, d.Tgt.Slot().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        gao.GetComponent<FloatTextVFX>().Text.text = $"{d._buffEntry.GetName()} +{d._stack}";
    }
}
