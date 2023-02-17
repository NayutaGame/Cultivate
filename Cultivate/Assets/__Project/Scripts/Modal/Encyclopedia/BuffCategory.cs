using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCategory : Category<BuffEntry>
{
    public BuffCategory()
    {
        _list = new()
        {
            new ("灵气", "可以消耗灵气使用技能", BuffStackRule.Add, true),
        };
    }
}
