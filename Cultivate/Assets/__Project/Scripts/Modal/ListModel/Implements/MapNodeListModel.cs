
using CLLibrary;

public class MapNodeListModel : ListModel<MapNode>
{
    public MapNodeListModel()
    {
        Encyclopedia.MapNodeCategory.Do(mapNodeEntry =>
        {
            Add(new MapNode(mapNodeEntry));
        });
    }
}
