
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class EntityEditorSkillBarView : AddressBehaviour
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private Image WuXingImage;

    public override void Refresh()
    {
        base.Refresh();

        ISkillModel skill = Get<ISkillModel>();
        NameText.text = skill.GetName();
        BackgroundImage.color = skill.GetColor();
        WuXingImage.sprite = skill.GetWuXingSprite();
    }
}
