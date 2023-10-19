
using DG.Tweening;
using UnityEngine;

public class BuffTweenDescriptor : TweenDescriptor
{
    public BuffDetails _buffDetails;

    public BuffTweenDescriptor(bool isAwait, BuffDetails buffDetails) : base(isAwait)
    {
        _buffDetails = buffDetails.Clone();
    }

    public override Tween GetTween()
    {
        return DOTween.Sequence()
            .AppendCallback(SpawnBuffVFX)
            .AppendCallback(SpawnBuffedText);
    }

    private void SpawnBuffVFX()
    {
        BuffDetails d = _buffDetails;

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
        BuffDetails d = _buffDetails;

        GameObject gao = GameObject.Instantiate(StageManager.Instance.FlowTextVFXPrefab, d.Tgt.Slot().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
        gao.GetComponent<FlowTextVFX>().Text.text = $"{d._buffEntry.Name} +{d._stack}";
    }
}
