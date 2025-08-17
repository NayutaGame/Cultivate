
public class GachaDetails : RunClosureDetails
{
    public SkillEntryDescriptor SkillEntryDescriptor;
    public int GachaIndex;

    public DeckIndex DeckIndex;
    
    public GachaDetails(SkillEntryDescriptor skillEntryDescriptor, int gachaIndex)
    {
        SkillEntryDescriptor = skillEntryDescriptor;
        GachaIndex = gachaIndex;
    }
}
