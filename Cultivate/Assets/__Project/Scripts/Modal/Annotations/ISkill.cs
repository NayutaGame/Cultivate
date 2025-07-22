
using UnityEngine;

public interface ISkill
{
    int GetCurrCounter();
    int GetMaxCounter();
    Sprite GetSprite();
    WuXing? GetWuXing();
    string GetName();
    TagComposite GetTagComposite();
    string GetTrivia();
    
    JingJie GetJingJie();
    Sprite GetJingJieSprite(JingJie showingJingJie);
    JingJie NextJingJie(JingJie showingJingJie);
}
