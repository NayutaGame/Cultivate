
using System.Collections.Generic;

public class MapNodeEntry : Entry
{
    public List<RoomScript> _scripts;
    
    public MapNodeEntry(string id, string name, List<RoomScript> scripts) : base(id, name)
    {
        _scripts = scripts;
    }
}
