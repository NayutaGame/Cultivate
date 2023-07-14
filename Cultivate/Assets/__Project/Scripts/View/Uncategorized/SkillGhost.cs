using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillGhost : SkillView
{
    public override ISkillModel GetSkillModel()
        => DataManager.Get<ISkillModel>(GetIndexPath());

    // protected void SetColorFromJingJie(JingJie jingJie)
    // {
    //     _image.color = CanvasManager.Instance.JingJieColors[jingJie];
    // }

    public void UpdateMousePos(Vector2 pos)
    {
        _rectTransform.position = pos;
    }
}
