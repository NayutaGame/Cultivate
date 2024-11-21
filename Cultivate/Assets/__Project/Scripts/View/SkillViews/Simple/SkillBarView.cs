
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class SkillBarView : XView
{
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private Image WuXingImage;
    [SerializeField] private TMP_Text NameText;

    public override void Refresh()
    {
        base.Refresh();

        ISkill skill = Get<ISkill>();
        BackgroundImage.color = CanvasManager.Instance.JingJieColors[skill.GetJingJie()];
        WuXingImage.color = CanvasManager.Instance.GetWuXingColor(skill.GetWuXing());
        NameText.text = skill.GetName();
    }
}
