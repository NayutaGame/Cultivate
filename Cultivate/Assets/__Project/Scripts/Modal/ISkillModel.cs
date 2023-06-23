using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillModel
{
    int GetManaCost();
    string GetName();
    string GetAnnotatedDescription(string evaluated = null);
    SkillTypeCollection GetSkillTypeCollection();
    Color GetColor();
    Sprite GetCardFace();
    Sprite GetJingJieSprite();
    string GetAnnotationText();
}
