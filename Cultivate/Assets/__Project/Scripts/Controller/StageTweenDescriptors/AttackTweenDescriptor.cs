
using DG.Tweening;
using UnityEngine;

public class AttackTweenDescriptor : StageTweenDescriptor
{
    private AttackDetails _attackDetails;

    public AttackTweenDescriptor(bool isAwait, AttackDetails attackDetails) : base(isAwait)
    {
        _attackDetails = attackDetails.Clone();
    }

    public override Tween GetTween()
    {
        return DOTween.Sequence()
            .Append(GetAttackTween())
            .AppendCallback(SpawnPiercingVFX)
            .AppendInterval(0.15f);
    }

    private Tween GetAttackTween()
    {
        StageEntity src = _attackDetails.Src;

        Transform slotTransform = src.Slot().transform;
        Transform entityTransform = src.Slot().EntityTransform;
        int orient = -(src.Index * 2 - 1);

        return entityTransform.DOMove(slotTransform.position + Vector3.right * orient * 1.2f, 0.15f).SetEase(Ease.InQuad);
    }

    private void SpawnPiercingVFX()
    {
        StageEntity src = _attackDetails.Src;
        WuXing wuXing = _attackDetails.WuXing ?? WuXing.Jin;
        int value = _attackDetails.Value;

        int orient = -(src.Index * 2 - 1);
        GameObject gao = GameObject.Instantiate(StageManager.Instance.PiercingVFXFromWuXing[wuXing], src.Slot().transform.position + 2 * orient * Vector3.right,
            Quaternion.Euler(0, src.Index * 180, 0), StageManager.Instance.VFXPool);
        VFX vfx = gao.GetComponent<VFX>();
        vfx.SetIntensity(IntensityFromValue(value));
        vfx.Play();
    }

    private float IntensityFromValue(int value)
    {
        return Mathf.InverseLerp(0, 100, value);
    }
}
