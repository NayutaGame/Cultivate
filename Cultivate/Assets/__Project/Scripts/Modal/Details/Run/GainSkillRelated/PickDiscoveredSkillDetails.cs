
public class PickDiscoveredSkillDetails : RunClosureDetails
{
    public SkillReference Skill;
    public int PickedIndex;
    
    public PickDiscoveredSkillDetails(SkillReference skill, int pickedIndex)
    {
        Skill = skill;
        PickedIndex = pickedIndex;
    }
}
