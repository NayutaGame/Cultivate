
using UnityEngine;

public interface ISkillModel
{
    int GetCurrCounter();
    int GetMaxCounter();
    Sprite GetSprite();
    Sprite GetWuXingSprite();
    string GetName();
    SkillTypeComposite GetSkillTypeComposite();
    string GetExplanation();
    string GetTrivia();
    
    JingJie GetJingJie();
    CostDescription GetCostDescription(JingJie showingJingJie);
    string GetHighlight(JingJie showingJingJie);
    Sprite GetJingJieSprite(JingJie showingJingJie);
    JingJie NextJingJie(JingJie showingJingJie);
    
    Color GetColor();
}
