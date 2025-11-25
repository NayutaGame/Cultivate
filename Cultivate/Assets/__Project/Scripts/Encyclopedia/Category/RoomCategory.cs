
using PuppyDragon.uNody.Logic;
using UnityEngine;

public class RoomCategory : Category<RoomEntry>
{
    public RoomCategory()
    {
        LogicGraph[] graphs = Resources.LoadAll<LogicGraph>("RoomGraphs");
        
        foreach (LogicGraph graph in graphs)
        {
            if (graph == null)
                continue;
            
            string id = graph.name;
            string name = graph.name;
            string roomPath = $"RoomGraphs/{name}";
            
            Add(new RoomEntry(id, name, roomPath, graph));
        }
    
        RefreshDict();
    }
}
