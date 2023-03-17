using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCategory : Category<NodeEntry>
{
    public NodeCategory()
    {
        List = new()
        {
            new BattleNodeEntry("Battle", "Battle"),
            new AdventureNodeEntry("Adventure", "Adventure"),
            new BossNodeEntry("Boss", "Boss"),
            new MarketNodeEntry("Market", "Market"),
        };
    }
}
