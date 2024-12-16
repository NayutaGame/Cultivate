using DG.Tweening;
using UnityEngine;

public class FormationTextAnimation : Animation
{
    public GainFormationDetails GainFormationDetails;

    public FormationTextAnimation(bool isAwait, GainFormationDetails gainFormationDetails) : base(isAwait, gainFormationDetails.Induced)
    {
        GainFormationDetails = gainFormationDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnText));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnText()
    {
        GainFormationDetails d = GainFormationDetails;

        GameObject gao = GameObject.Instantiate(StageManager.Instance.FloatTextVFXPrefab, d.Owner.Model().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        gao.GetComponent<FloatTextVFX>().Text.text = $"{d._formation.GetName()}";
    }
}
