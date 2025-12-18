
public class RoomOption
{
    public RoomEntry RoomEntry;
    public Description Description;
    
    public RoomOption(RoomEntry roomEntry, string description)
    {
        RoomEntry = roomEntry;
        Description = new(description);
    }
}