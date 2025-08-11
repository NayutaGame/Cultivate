
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarView : XView
{
    [SerializeField] private Image BarIllustration;
    [SerializeField] private TMP_Text Name;

    public override void Refresh()
    {
        base.Refresh();

        AnnotatableSkill skill = Get<AnnotatableSkill>();
        Name.text = skill.GetName();
        // BackgroundImage.color = CanvasManager.Instance.JingJieColors[skill.GetJingJie()];
        // BarIllustration.sprite = ;
    }
}
