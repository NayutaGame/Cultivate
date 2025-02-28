
public class PickDiscoveredSkillDetails : RunClosureDetails
{
    public SkillEntryDescriptor Skill;
    public int PickedIndex;
    
    public PickDiscoveredSkillDetails(SkillEntryDescriptor skill, int pickedIndex)
    {
        Skill = skill;
        PickedIndex = pickedIndex;
    }
}
