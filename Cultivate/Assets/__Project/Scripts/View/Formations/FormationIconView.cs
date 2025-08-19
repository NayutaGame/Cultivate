
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FormationIconView : XView
{
    [SerializeField] private TMP_Text ProgressText;
    [SerializeField] private Image Glow;
    [SerializeField] private Image Background;
    [SerializeField] private Image Icon;

    public override void Refresh()
    {
        base.Refresh();

        AnnotatableFormation formation = Get<AnnotatableFormation>();
        
        if (formation is RunFormation rf)
        {
            ProgressText.gameObject.SetActive(true);
            if (ProgressText != null)
            {
                int progress = rf.GetProgress();
                ProgressText.text = $"{progress}";
            }
        }
        else
        {
            ProgressText.gameObject.SetActive(false);
        }

        Background.sprite = formation.GetBackgroundSprite().Sprite;
        Glow.sprite = Encyclopedia.SpriteCategory.FromName("普通阵法发光").Sprite;
        Icon.sprite = formation.GetIconSprite().Sprite;
    }
}
