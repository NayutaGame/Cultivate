
using TMPro;
using UnityEngine;

public class RunSkillChipView : RunSkillView
{
    [SerializeField] private TMP_Text TypeText;

    protected override void SetSkillTypeComposite(SkillTypeComposite skillTypeComposite)
    {
        base.SetSkillTypeComposite(skillTypeComposite);

        if(TypeText != null)
            TypeText.text = skillTypeComposite.ToString();
    }

    // public override void SetColor(Color color)
    // {
    //     _image.color = color;
    // }
    //
    // public override void SetCardFace(Sprite cardFace)
    // {
    //     _image.sprite = cardFace;
    // }
}
