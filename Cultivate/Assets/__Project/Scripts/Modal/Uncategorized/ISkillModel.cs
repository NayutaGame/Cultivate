
using UnityEngine;

public interface ISkillModel
{
    int GetCurrCounter();
    int GetMaxCounter();
    Sprite GetSprite();
    int GetManaCost();
    string GetName();
    string GetHighlight();
    string GetExplanation();
    string GetTrivia();
    SkillTypeComposite GetSkillTypeComposite();
    Color GetColor();
    Sprite GetCardFace();
    Sprite GetJingJieSprite();
    Sprite GetWuXingSprite();
}
