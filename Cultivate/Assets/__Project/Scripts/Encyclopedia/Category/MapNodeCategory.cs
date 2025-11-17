
using System.Collections.Generic;

public class MapNodeCategory : Category<MapNodeEntry>
{
    public MapNodeCategory()
    {
        AddRange(new List<MapNodeEntry>()
        {
            new(id:                       "MapNode0001",
                name:                     "凌云峰"),
            new(id:                       "MapNode0002",
                name:                     "逍遥海"),
            new(id:                       "MapNode0003",
                name:                     "天机阁"),
            new(id:                       "MapNode0004",
                name:                     "长明殿"),
            new(id:                       "MapNode0005",
                name:                     "环岳岭"),
            new(id:                       "MapNode0006",
                name:                     "易宝斋"),
            new(id:                       "MapNode0007",
                name:                     "剑池"),
            new(id:                       "MapNode0008",
                name:                     "风雨楼"),
            new(id:                       "MapNode0009",
                name:                     "百草堂"),
            new(id:                       "MapNode0010",
                name:                     "星宫"),
            new(id:                       "MapNode0011",
                name:                     "大椿树"),
            new(id:                       "MapNode0012",
                name:                     "蓬莱阁"),
            new(id:                       "MapNode0013",
                name:                     "桃花林"),
            new(id:                       "MapNode0014",
                name:                     "明心庐"),
            new(id:                       "MapNode0015",
                name:                     "坠星湖"),
        });
    }
}
