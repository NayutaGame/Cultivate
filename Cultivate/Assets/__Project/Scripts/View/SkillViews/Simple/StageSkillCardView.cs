
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StageSkillCardView : SkillCardView
{
    [SerializeField] private Image CounterImage;

    public override void Refresh()
    {
        base.Refresh();

        if (!gameObject.activeSelf)
            return;

        ISkill skill = Get<ISkill>();
        SetCounter(skill.GetCurrCounter(), skill.GetMaxCounter());
    }

    private void SetCounter(int currCounter, int maxCounter)
    {
        if (maxCounter == 0)
        {
            CounterImage.fillAmount = 0;
            return;
        }
        CounterImage.fillAmount = (float)currCounter / maxCounter;
    }

    public Tween GetExpandTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(GetViewTransform().DOScale(1, 0.6f).SetEase(Ease.InOutQuad));
        return seq;
    }

    public Tween GetShrinkTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(GetViewTransform().DOScale(0.5f, 0.6f).SetEase(Ease.InOutQuad));
        return seq;
    }
}
