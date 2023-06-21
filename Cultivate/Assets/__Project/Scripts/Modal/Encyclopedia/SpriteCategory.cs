using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCategory : Category<SpriteEntry>
{
    public SpriteCategory()
    {
        List = new()
        {
            new("奇遇", "Images/NodeIcons/Adventure"),
            new("战斗", "Images/NodeIcons/Battle"),
            new("Boss", "Images/NodeIcons/Boss"),
            new("事件", "Images/NodeIcons/Event"),
            new("金钱", "Images/NodeIcons/JinQian"),
            new("人参", "Images/NodeIcons/RenShen"),
            new("人参果", "Images/NodeIcons/RenShenGuo"),
            new("商店", "Images/NodeIcons/Shop"),
            new("算卦", "Images/NodeIcons/SuanGua"),
            new("温泉", "Images/NodeIcons/WenQuan"),
            new("悟道", "Images/NodeIcons/WuDao"),
            new("修炼", "Images/NodeIcons/XiuLian"),
            new("以物易物", "Images/NodeIcons/YiWuYiWu"),
        };
    }

    public override SpriteEntry Default() => this["MissingSprite"];
}
