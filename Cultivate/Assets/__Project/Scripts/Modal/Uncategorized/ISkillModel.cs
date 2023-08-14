using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillModel
{
    int GetCurrCounter();
    int GetMaxCounter();
    Sprite GetSprite();
    int GetManaCost();
    string GetName();
    string GetAnnotatedDescription(string evaluated = null);
    SkillTypeComposite GetSkillTypeComposite();
    Color GetColor();
    Sprite GetCardFace();
    Sprite GetJingJieSprite();
    Sprite GetWuXingSprite();
    string GetAnnotationText();
}
