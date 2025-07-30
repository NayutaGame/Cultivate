
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarView : XView
{
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private Image WuXingImage;
    [SerializeField] private TMP_Text NameText;

    public override void Refresh()
    {
        base.Refresh();

        RunSkill skill = Get<RunSkill>();
        BackgroundImage.color = CanvasManager.Instance.JingJieColors[skill.GetJingJie()];
        WuXingImage.color = CanvasManager.Instance.GetWuXingColor(skill.GetWuXing());
        NameText.text = skill.GetName();
    }
}
