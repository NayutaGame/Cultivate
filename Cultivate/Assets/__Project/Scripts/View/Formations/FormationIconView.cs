
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FormationIconView : SimpleView
{
    [SerializeField] private TMP_Text ProgressText;
    [SerializeField] private Image Glow;
    [SerializeField] private Image Background;
    [SerializeField] private Image Icon;

    public override void Refresh()
    {
        base.Refresh();

        IFormationModel formation = Get<IFormationModel>();
        bool formationIsNull = formation == null;
        gameObject.SetActive(!formationIsNull);
        if (formationIsNull)
            return;
        
        if (formation is RunFormation rf)
        {
            if (ProgressText != null)
            {
                JingJie nextActivatingJingJie = rf.GetNextActivatingJingJie();
                int progress = rf.GetProgress();
                int requirement = formation.GetRequirementFromJingJie(nextActivatingJingJie);
                ProgressText.text = $"{progress}/{requirement}";
            }
        }
        
        JingJie? activatedJingJie = formation.GetActivatedJingJie();
        SpriteEntry spriteEntry = activatedJingJie == null ? "未激活阵法背景" : $"{activatedJingJie.Value.Name}阵法背景";
        Background.sprite = spriteEntry.Sprite;
        Glow.sprite = Encyclopedia.SpriteCategory["普通阵法发光"].Sprite;
        Icon.sprite = formation.GetSprite().Sprite;
    }
}
