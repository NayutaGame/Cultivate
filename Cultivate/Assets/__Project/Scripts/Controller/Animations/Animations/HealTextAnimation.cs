
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HealTextAnimation : Animation
{
    private HealDetails _healDetails;

    public HealTextAnimation(bool isAwait, HealDetails healDetails) : base(isAwait)
    {
        _healDetails = healDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnText));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnText()
    {
        StageEntity tgt = _healDetails.Tgt;
        int value = _healDetails.Value;
    
        GameObject gao = GameObject.Instantiate(StageManager.Instance.FloatTextVFXPrefab, tgt.Slot().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
    
        TMP_Text text = gao.GetComponent<FloatTextVFX>().Text;
        text.text = value.ToString();
        text.color = Color.green;
        gao.transform.localScale = Vector3.zero;
        DOTween.Sequence()
            .Append(gao.transform.DOScale(3, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(1, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
    }
}
