
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class SkillBarView : AddressBehaviour
{
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private Image WuXingImage;
    [SerializeField] private TMP_Text NameText;

    public override void Refresh()
    {
        base.Refresh();

        ISkillModel skill = Get<ISkillModel>();
        BackgroundImage.color = skill.GetColor();
        WuXingImage.sprite = skill.GetWuXingSprite();
        NameText.text = skill.GetName();
    }
}
