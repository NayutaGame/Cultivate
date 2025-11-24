
using System.Collections.Generic;

public class RoomCategory : Category<RoomEntry>
{
    public RoomCategory()
    {
        AddRange(new List<RoomEntry>()
        {
            new(id:                       "MapNode01_001",
                name:                     "Void凌云峰"),
            
            new(id:                       "MapNode02_001",
                name:                     "Location凌云峰1"),
            new(id:                       "MapNode02_002",
                name:                     "Location凌云峰2"),
            new(id:                       "MapNode02_003",
                name:                     "Location凌云峰3"),
            
            new(id:                       "MapNode03_001",
                name:                     "Visitor徐福"),
            new(id:                       "MapNode03_002",
                name:                     "Visitor彼此卿"),
            new(id:                       "MapNode03_003",
                name:                     "Visitor风雨晴"),
            new(id:                       "MapNode03_004",
                name:                     "Visitor子非鱼"),
            new(id:                       "MapNode03_005",
                name:                     "Visitor子非燕"),
            
            new(id:                       "MapNode04_001",
                name:                     "Character徐福1"),
        });
    }
}
