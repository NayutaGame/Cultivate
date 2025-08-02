
public class PlacementDetails : RunClosureDetails
{
    public RunEntity Owner;
    public SkillEntry OverridingSkillEntry;
    public JingJie OverridingJingJie;

    public PlacementDetails(RunEntity owner)
    {
        Owner = owner;
        OverridingSkillEntry = Encyclopedia.SkillCategory.FromId("Skill0001");
        OverridingJingJie = JingJie.LianQi;
    }
}
