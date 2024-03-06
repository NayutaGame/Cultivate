
using DG.Tweening;
using UnityEngine;

public class BuffTweenDescriptor : StageTweenDescriptor
{
    public GainBuffDetails GainBuffDetails;

    public BuffTweenDescriptor(bool isAwait, GainBuffDetails gainBuffDetails) : base(isAwait)
    {
        GainBuffDetails = gainBuffDetails.Clone();
    }

    public override Tween GetTween()
    {
        return DOTween.Sequence()
            .AppendCallback(SpawnBuffVFX)
            .AppendCallback(SpawnBuffedText);
    }

    private void SpawnBuffVFX()
    {
        GainBuffDetails d = GainBuffDetails;

        GameObject prefab = d._buffEntry.Friendly
            ? StageManager.Instance.BuffVFXPrefab
            : StageManager.Instance.DebuffVFXPrefab;
        GameObject gao = GameObject.Instantiate(prefab, d.Tgt.Slot().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.Play();
    }

    private void SpawnBuffedText()
    {
        GainBuffDetails d = GainBuffDetails;

        GameObject gao = GameObject.Instantiate(StageManager.Instance.FlowTextVFXPrefab, d.Tgt.Slot().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        gao.GetComponent<FlowTextVFX>().Text.text = $"{d._buffEntry.GetName()} +{d._stack}";
    }
}
