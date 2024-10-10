
using UnityEngine;

public interface ISkill
{
    int GetCurrCounter();
    int GetMaxCounter();
    Sprite GetSprite();
    WuXing? GetWuXing();
    string GetName();
    SkillTypeComposite GetSkillTypeComposite();
    string GetExplanation();
    string GetTrivia();
    
    JingJie GetJingJie();
    CostDescription GetCostDescription(JingJie showingJingJie);
    string GetHighlight(JingJie showingJingJie);
    Sprite GetJingJieSprite(JingJie showingJingJie);
    JingJie NextJingJie(JingJie showingJingJie);
}
