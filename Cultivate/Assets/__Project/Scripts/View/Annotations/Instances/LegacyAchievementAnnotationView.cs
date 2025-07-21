
using TMPro;
using UnityEngine;

public class LegacyAchievementAnnotationView : XView
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text UnlockStatusText;
    [SerializeField] private TMP_Text ConditionDescriptionText;

    public override void Refresh()
    {
        base.Refresh();

        AchievementProfile achievement = Get<AchievementProfile>();

        bool hasValue = achievement != null;
        gameObject.SetActive(hasValue);
        if (!hasValue)
            return;
        
        NameText.text = achievement.GetEntry().GetName();
        UnlockStatusText.text = achievement.IsUnlocked() ? "已解锁" : "未解锁";
        ConditionDescriptionText.text = achievement.GetEntry().GetConditionDescription();
    }
}
