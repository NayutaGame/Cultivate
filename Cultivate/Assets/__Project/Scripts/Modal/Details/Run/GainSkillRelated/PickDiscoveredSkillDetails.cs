
public class PickDiscoveredSkillDetails : RunClosureDetails
{
    public SkillGhost Skill;
    public int PickedIndex;
    
    public PickDiscoveredSkillDetails(SkillGhost skill, int pickedIndex)
    {
        Skill = skill;
        PickedIndex = pickedIndex;
    }
}
