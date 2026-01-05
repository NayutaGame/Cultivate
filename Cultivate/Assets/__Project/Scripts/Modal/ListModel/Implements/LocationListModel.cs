
using CLLibrary;

public class LocationListModel : ListModel<Location>
{
    public LocationListModel()
    {
        Encyclopedia.LocationCategory.Do(locationEntry =>
        {
            Add(new Location(locationEntry));
        });
    }
}
