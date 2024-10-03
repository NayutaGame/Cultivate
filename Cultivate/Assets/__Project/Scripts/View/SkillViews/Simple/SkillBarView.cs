
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class SkillBarView : SimpleView
{
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private Image WuXingImage;
    [SerializeField] private TMP_Text NameText;

    public override void Refresh()
    {
        base.Refresh();

        ISkill skill = Get<ISkill>();
        BackgroundImage.color = skill.GetColor();
        WuXingImage.sprite = skill.GetWuXingSprite();
        NameText.text = skill.GetName();
    }
}
