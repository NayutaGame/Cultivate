
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamagedTweenDescriptor : TweenDescriptor
{
    private DamageDetails _damageDetails;

    public DamagedTweenDescriptor(bool isAwait, DamageDetails damageDetails) : base(isAwait)
    {
        _damageDetails = damageDetails.Clone();
    }

    public override Tween GetTween()
    {
        return DOTween.Sequence()
            .AppendCallback(SpawnDamagedText);
    }

    private void SpawnDamagedText()
    {
        StageEntity tgt = _damageDetails.Tgt;
        int value = _damageDetails.Value;

        GameObject gao = GameObject.Instantiate(StageManager.Instance.FlowTextVFXPrefab, tgt.Slot().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);

        TMP_Text text = gao.GetComponent<FlowTextVFX>().Text;
        text.text = value.ToString();
        text.color = Color.red;
        gao.transform.localScale = Vector3.zero;
        DOTween.Sequence()
            .Append(gao.transform.DOScale(3, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(1, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
    }
}
