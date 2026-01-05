
using System.Collections.Generic;

public class LocationCategory : Category<LocationEntry>
{
    public LocationCategory()
    {
        AddRange(new List<LocationEntry>()
        {
            new(id:                       "Location0001",
                name:                     "凌云峰"),
            new(id:                       "Location0002",
                name:                     "逍遥海"),
            new(id:                       "Location0003",
                name:                     "天机阁"),
            new(id:                       "Location0004",
                name:                     "长明殿"),
            new(id:                       "Location0005",
                name:                     "环岳岭"),
            new(id:                       "Location0006",
                name:                     "易宝斋"),
            new(id:                       "Location0007",
                name:                     "剑池"),
            new(id:                       "Location0008",
                name:                     "风雨楼"),
            new(id:                       "Location0009",
                name:                     "百草堂"),
            new(id:                       "Location0010",
                name:                     "星宫"),
            new(id:                       "Location0011",
                name:                     "大椿树"),
            new(id:                       "Location0012",
                name:                     "蓬莱阁"),
            new(id:                       "Location0013",
                name:                     "桃花林"),
            new(id:                       "Location0014",
                name:                     "明心庐"),
            new(id:                       "Location0015",
                name:                     "坠星湖"),
        });
    }
}
