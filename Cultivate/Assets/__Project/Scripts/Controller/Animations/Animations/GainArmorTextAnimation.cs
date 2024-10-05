
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GainArmorTextAnimation : Animation
{
    public GainArmorDetails _gainArmorDetails;

    public GainArmorTextAnimation(bool isAwait, GainArmorDetails gainArmorDetails) : base(isAwait, gainArmorDetails.Induced)
    {
        _gainArmorDetails = gainArmorDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnText));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnText()
    {
        StageEntity tgt = _gainArmorDetails.Tgt;
        int value = _gainArmorDetails.Value;
    
        GameObject gao = GameObject.Instantiate(StageManager.Instance.FloatTextVFXPrefab, tgt.Slot().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
    
        TMP_Text text = gao.GetComponent<FloatTextVFX>().Text;
        text.text = $"护甲+{value}";
        text.color = Color.yellow;
        gao.transform.localScale = Vector3.zero;
        DOTween.Sequence()
            .Append(gao.transform.DOScale(3, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(1, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
    }
}
