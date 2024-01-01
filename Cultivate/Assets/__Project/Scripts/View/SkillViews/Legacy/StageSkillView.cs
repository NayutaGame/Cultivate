
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSkillView : SkillView
{
    [SerializeField] private Image CounterImage;

    public override void Refresh()
    {
        base.Refresh();

        if (!gameObject.activeSelf)
            return;

        ISkillModel skill = Get<ISkillModel>();
        SetCounter(skill.GetCurrCounter(), skill.GetMaxCounter());
    }

    private void SetCounter(int currCounter, int maxCounter)
    {
        CounterImage.fillAmount = (float)currCounter / maxCounter;
    }

    public Tween GetExpandTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(RectTransform.DOScale(1, 0.6f).SetEase(Ease.InOutQuad));
        return seq;
    }

    public Tween GetShrinkTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(RectTransform.DOScale(0.5f, 0.6f).SetEase(Ease.InOutQuad));
        return seq;
    }
}
