
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GuardedTextAnimation : Animation
{
    private GuardedDetails _guardedDetails;

    public GuardedTextAnimation(bool isAwait, GuardedDetails guardedDetails) : base(isAwait)
    {
        _guardedDetails = guardedDetails.Clone();
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .AppendCallback(SpawnText));
    }
    
    public override bool InvolvesCharacterAnimation() => false;

    private void SpawnText()
    {
        StageEntity tgt = _guardedDetails.Tgt;
    
        GameObject gao = GameObject.Instantiate(StageManager.Instance.FlowTextVFXPrefab, tgt.Slot().transform.position,
            Quaternion.identity, StageManager.Instance.VFXPool);
    
        TMP_Text text = gao.GetComponent<FlowTextVFX>().Text;
        text.text = "完全防御";
        text.color = Color.yellow;
        gao.transform.localScale = Vector3.zero;
        DOTween.Sequence()
            .Append(gao.transform.DOScale(3, 0.3f).SetEase(Ease.OutCubic))
            .Append(gao.transform.DOScale(1, 0.7f).SetEase(Ease.InCubic))
            .SetAutoKill().Restart();
    }
}
