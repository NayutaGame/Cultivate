
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UndamagedTweenDescriptor : StageTweenDescriptor
{
    private DamageDetails _damageDetails;

    public UndamagedTweenDescriptor(bool isAwait, DamageDetails damageDetails) : base(isAwait)
    {
        _damageDetails = damageDetails.Clone();
    }

    public override Tween GetTween()
    {
        return DOTween.Sequence()
            .AppendCallback(SpawnUndamagedText);
    }

    private void SpawnUndamagedText()
    {
        StageEntity tgt = _damageDetails.Tgt;

        GameObject gao = GameObject.Instantiate(StageManager.Instance.FlowTextVFXPrefab, tgt.Slot().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);

        TMP_Text text = gao.GetComponent<FlowTextVFX>().Text;
        text.text = "无伤害";
        text.color = Color.red;
        gao.transform.localScale = Vector3.zero;
        DOTween.Sequence()
            .Append(gao.transform.DOScale(3, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(1, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
    }
}
