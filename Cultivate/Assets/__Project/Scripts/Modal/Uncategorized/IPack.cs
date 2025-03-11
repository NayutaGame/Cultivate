
using UnityEngine;

public interface IPack
{
    string GetName();
    WuXing? GetWuXing();
    string GetDescription();
    string GetTrivia();

    Sprite GetSprite();
    bool Equipped();
    bool IsUnlocked();
    string GetUnlockCondition();
}
