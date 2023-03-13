using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

public class TechCategory : Category<TechEntry>
{
    public TechCategory()
    {
        // for positions, x in (0 ~ 9), y in (0 ~ 7)
        List = new()
        {
            new("金", "金科技", Vector2Int.zero, 10),
            new("水", "水科技", Vector2Int.one, 10),
            new("木", "木科技", Vector2Int.one * 2, 10),
            new("火", "火科技", Vector2Int.one * 3, 10),
            new("土", "土科技", Vector2Int.one * 4, 10),
            new ("八卦", "", new(0, 5), 10),
            new ("四象", "", new(1, 5), 10, new string[]{"八卦"}),
            new ("两仪", "", new(2, 5), 10, new string[]{"四象"}),
            new ("太极", "", new(3, 5), 10, new string[]{"两仪"}),
            new("八", "第八科技", new(0, 7), 10,
                eureka: new AcquireEventDescriptor((d, runTech) =>
                {
                    return true;
                })),
        };
    }

    public void Init()
    {
        List.Do(entry => entry.Init());
    }
}
