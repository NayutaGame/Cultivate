
using UnityEngine;
using UnityEngine.UI;

public class StageSkillView : DelegatingView
{
    [SerializeField] private Image CounterImage;
    [SerializeField] public RectTransform TimelineScale;

    public override void Refresh()
    {
        base.Refresh();

        // if (!gameObject.activeSelf)
        //     return;

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
}
