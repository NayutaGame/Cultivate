using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunSkillChipView : RunSkillView
{
    [SerializeField] private Image SelectionImage;
    public override void SetSelected(bool selected)
    {
        base.SetSelected(selected);
        if (SelectionImage != null)
            SelectionImage.color = new Color(1, 1, 1, selected ? 1 : 0);
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
