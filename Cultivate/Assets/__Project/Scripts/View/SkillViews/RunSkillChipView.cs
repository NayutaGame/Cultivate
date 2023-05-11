using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSkillChipView : RunSkillView
{
    public override void SetColor(Color color)
    {
        _image.color = color;
    }

    public override void SetCardFace(Sprite cardFace)
    {
        _image.sprite = cardFace;
    }
}
