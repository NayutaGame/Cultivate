
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FormationView : ItemView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text JingJieText;
    [SerializeField] private TMP_Text ConditionText;
    [SerializeField] private TMP_Text RewardText;

    public override void Refresh()
    {
        base.Refresh();
        FormationEntry e = Get<FormationEntry>();
        if (e == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (NameText != null)
            NameText.text = e.GetName();

        if (JingJieText != null)
            JingJieText.text = e.GetJingJie().ToString();

        if (ConditionText != null)
            ConditionText.text = e.GetConditionDescription();

        if (RewardText != null)
            RewardText.text = e.GetRewardDescription();
    }
}
