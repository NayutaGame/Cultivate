
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UndamagedTextAnimation : Animation
{
    private DamageDetails _damageDetails;

    public UndamagedTextAnimation(bool isAwait, DamageDetails damageDetails) : base(isAwait, damageDetails.Induced)
    {
        _damageDetails = damageDetails.ShallowClone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnText));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnText()
    {
        StageEntity tgt = _damageDetails.Tgt;

        GameObject gao = GameObject.Instantiate(StageManager.Instance.FloatTextVFXPrefab, tgt.Model().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);

        TMP_Text text = gao.GetComponent<FloatTextVFX>().Text;
        text.text = "无伤害";
        text.color = Color.red;
        gao.transform.localScale = Vector3.zero;
        DOTween.Sequence()
            .Append(gao.transform.DOScale(3, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(1, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
    }
}
