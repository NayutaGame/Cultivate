
using UnityEngine;
using UnityEngine.UI;

public class StageSkillView : SlotView
{
    [SerializeField] private Image CounterImage;
    [SerializeField] public RectTransform TimelineScale;

    public override void Refresh()
    {
        base.Refresh();

        StageNote skill = Get<StageNote>();
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
