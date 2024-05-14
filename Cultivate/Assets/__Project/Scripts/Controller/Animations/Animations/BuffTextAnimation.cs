using DG.Tweening;
using UnityEngine;

public class BuffTextAnimation : Animation
{
    public GainBuffDetails GainBuffDetails;

    public BuffTextAnimation(bool isAwait, GainBuffDetails gainBuffDetails) : base(isAwait)
    {
        GainBuffDetails = gainBuffDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnBuffedText));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnBuffedText()
    {
        GainBuffDetails d = GainBuffDetails;

        GameObject gao = GameObject.Instantiate(StageManager.Instance.FlowTextVFXPrefab, d.Tgt.Slot().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        gao.GetComponent<FlowTextVFX>().Text.text = $"{d._buffEntry.GetName()} +{d._stack}";
    }
}
