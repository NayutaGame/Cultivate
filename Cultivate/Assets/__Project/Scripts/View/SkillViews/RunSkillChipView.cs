using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RunSkillChipView : RunSkillView
{
    [SerializeField] private Image SelectionImage;
    [SerializeField] private TMP_Text TypeText;
    public override void SetSelected(bool selected)
    {
        base.SetSelected(selected);
        if (SelectionImage != null)
            SelectionImage.color = new Color(1, 1, 1, selected ? 1 : 0);
    }

    public override void SetSkillTypeCollection(SkillTypeCollection skillTypeCollection)
    {
        base.SetSkillTypeCollection(skillTypeCollection);

        if(TypeText != null)
            TypeText.text = skillTypeCollection.ToString();
    }

    public override void SetColor(Color color)
    {
        _image.color = color;
    }

    public override void SetCardFace(Sprite cardFace)
    {
        _image.sprite = cardFace;
    }
}
