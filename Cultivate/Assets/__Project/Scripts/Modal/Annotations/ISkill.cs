
using UnityEngine;

public interface ISkill
{
    int GetCurrCounter();
    int GetMaxCounter();
    Sprite GetSprite();
    WuXing? GetWuXing();
    string GetName();
    TagComposite GetTagComposite();
    string GetCascadeAnnotated();
    string GetTrivia();
    
    JingJie GetJingJie();
    CostDescription GetLiteralCostDescription(JingJie showingJingJie);
    string GetHighlight(JingJie showingJingJie);
    Sprite GetJingJieSprite(JingJie showingJingJie);
    JingJie NextJingJie(JingJie showingJingJie);
}
