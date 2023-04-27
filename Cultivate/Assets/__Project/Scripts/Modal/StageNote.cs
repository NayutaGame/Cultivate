
public class StageNote
{
    public int EntityIndex;
    public int TemporalIndex;
    public StageWaiGong WaiGong;

    public StageNote(int entityIndex, int temporalIndex, StageWaiGong waiGong)
    {
        EntityIndex = entityIndex;
        TemporalIndex = temporalIndex;
        WaiGong = waiGong;
    }

    public bool IsHome
        => EntityIndex == 0;
}
