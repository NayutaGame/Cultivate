
using TMPro;
using UnityEngine;

public class MilestoneView : XView
{
    [SerializeField] private TMP_Text LabelText;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] public CanvasGroup CanvasGroup;

    public override void Refresh()
    {
        base.Refresh();

        RunMilestone item = Get<RunMilestone>();
        LabelText.text = item.Description;
        ScoreText.text = item.ExperienceGain.ToString();
    }
}
